using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using YJH.Unit.Event;
using IntimeWar.Battle;
using IntimeWar;

namespace YJH.Unit
{
    public enum UnitStateEnum
    {
        Paused,
        Normal,
        Inactive
    }

    public class BattleUnit : MonoBehaviour, IPunObservable
    {
        static int _seqNo = -2000;
        public static int AllocateSeqNo() { return _seqNo--; }

        bool _initialized;
        public bool Initialized { get { return _initialized; } }

        [SerializeField]
        bool _noSequenceUnit;
        public bool NoSequenceUnit { get { return _noSequenceUnit; } }

        //UnitStateEnum _unitState = UnitStateEnum.Paused;
        //public UnitStateEnum CurrentState { set { _unitState = value; } get { return _unitState; } }
        //public bool IsNormal { get { return _unitState == UnitStateEnum.Normal; } }
        //public bool IsInactive { get { return _unitState == UnitStateEnum.Inactive; } }
        public bool IsDead { get { return STS.IsDead; } }

        [SerializeField]
        protected UnitInfo _unitInfo;
        public UnitInfo Info { get { return _unitInfo; } }

        public bool IsSpawnFromUnit { get { return _unitInfo.IsSpawnFromUnit; } }

        public string UnitTag { get { return _unitInfo.Tag; } }
        public byte Team { get { return _unitInfo.Team; } }
        public float Level { get { return _unitInfo.Level; } }
        public int SeqNo { get { return _unitInfo.SeqNo; } }
        public int ActorID { get { return _unitInfo.ActorID; } }
        public string TypeID { get { return _unitInfo.TypeID; } }

        [SerializeField]
        Animator _animator;
        public Animator Animator { get { return _animator; } }

        PlayMakerFSM _playMakerFsm;
        public PlayMakerFSM Fsm { set { _playMakerFsm = value; } get { return _playMakerFsm; } }

        [SerializeField]
        protected List<GameObject> _abilityPrefabs;
        List<IUnitAbility> _abilitityList = new List<IUnitAbility>();

        public UnitBody Body { private set; get; }
        public UnitMobility Mobility { private set; get; }
        public UnitEventDispatcher EventDispatcher { private set; get; }
        public AIInput AI { private set; get; }

        UnitStatusManager _statusManager;
        public UnitStatusManager STS { get { return _statusManager; } }

        PhotonView _view;
        public PhotonView View { get { return _view; } }
        public bool IsControlByThisClient
        {
            get
            {
                if (_view == null)
                    return true;

                if (_view != null)
                    return _view.IsControlByThisClient();

                return false;
            }
        }

        public bool IsPlayerForThisClient
        {
            get
            {
                return _view != null && !_view.isSceneView && _view.CreatorActorNr == PhotonNetwork.player.ID;
            }
        }

        Dictionary<string, IUnitAbility> _idToAbilityMapping = new Dictionary<string, IUnitAbility>();

        public Dictionary<string, IUnitAbility> AbilityMap
        {
            set { _idToAbilityMapping = value; }
            get { return _idToAbilityMapping; }
        }

        [SerializeField]
        Transform _modelRoot;
        public Transform Model
        {
            get
            {
                if (_modelRoot == null)
                    return transform;
                return _modelRoot;
            }
        }

        UnitInitialParameter _initialparameter;
        public UnitInitialParameter InitialParameter { get { return _initialparameter; } }

        public struct UnitCreateArgs
        {
            public byte Team;
            public string Tag;
            public float Level;
            public UnitInfo SpawnFrom;
            public UnitInitialParameter InitialParameter;
        }

        public void Init(UnitCreateArgs args)
        {
            if (_initialized)
            {
                Debug.LogErrorFormat(this, "unit is already initialized!");
                return;
            }
            _initialized = true;

            _unitInfo = new UnitInfo
            {
                Team = args.Team,
                Tag = args.Tag,
                Level = args.Level,
                SpawnFrom = args.SpawnFrom,
            };

            _initialparameter = args.InitialParameter;

            _view = GetComponent<PhotonView>();

            _unitInfo.TypeID = name;
            _unitInfo.Unit = this;
            if (!_noSequenceUnit)
            {
                if (_view != null)
                {
                    _unitInfo.SeqNo = _view.viewID;
                    _unitInfo.ActorID = _view.CreatorActorNr;
                }
                else
                {
                    _unitInfo.SeqNo = AllocateSeqNo();
                    _unitInfo.ActorID = 0;
                }

                name = string.Format("{0, 0:d4}_{1}", SeqNo, _unitInfo.TypeID);
            }

            _statusManager = gameObject.AddComponent<UnitStatusManager>();

            InitCommonAbilities();
            InitAnimatorStateAbilities();
            InitPlayerMakerStateAbilities();

            Body = GetAbility<UnitBody>();
            Mobility = GetAbility<UnitMobility>();
            EventDispatcher = GetAbility<UnitEventDispatcher>();
            AI = GetAbility<AIInput>();

            if (EventDispatcher == null)
            {
                EventDispatcher = gameObject.AddComponent<UnitEventDispatcher>();
                _abilitityList.Add(EventDispatcher);
                EventDispatcher.SetupInstance(this);
            }

            for (int i = 0, count = _abilitityList.Count; i < count; i++)
            {
                var a = _abilitityList[i];
                CreateAbilityIDMap(a);
            }

            for (int i = 0, count = _abilitityList.Count; i < count; i++)
            {
                var ability = _abilitityList[i];
                ability.Init();
            }

            for (int i = 0, count = _abilitityList.Count; i < count; i++)
            {
                var ability = _abilitityList[i];
                ability.LateInit();
            }

            if (_view != null)
            {
                var allSyncData = SpawnerManager.Instance.GetPermanentSyncDataForAllAbility(_view.viewID);
                if (allSyncData != null)
                {
                    for (int i = 0; i < _abilitityList.Count; i++)
                    {
                        var ability = _abilitityList[i];
                        if (ability is IPunObservable && ability is Component && !_view.ObservedComponents.Contains((Component)ability))
                            _view.ObservedComponents.Add((Component)ability);

                        object syncData;
                        if (allSyncData.TryGetValue(ability.AbilityID, out syncData))
                        {
                            ability.OnInitSynchronization(syncData);
                        }
                    }
                }
                //var animatorIndex = _view.ObservedComponents.FindIndex(c => c is PhotonAnimatorView);
                //if(animatorIndex >= 0)
                //{
                //	var v = _view.ObservedComponents[animatorIndex];
                //	_view.ObservedComponents.RemoveAt(animatorIndex);
                //	_view.ObservedComponents.Add(v);
                //}
            }
        }

        void InitCommonAbilities()
        {
            var childrenAbilites = GetComponentsInChildren<IUnitAbility>(true);
            for (int i = 0; i < childrenAbilites.Length; i++)
            {
                var com = (Component)childrenAbilites[i];
                if (com.gameObject == gameObject || com.GetComponentInParent<BattleUnit>() == this)
                {
                    _abilitityList.Add(childrenAbilites[i]);
                }
            }

            for (int i = 0; i < _abilityPrefabs.Count; i++)
            {
                var prefab = _abilityPrefabs[i];
                if (prefab != null && prefab.GetComponent<IUnitAbility>() != null)
                {
                    GameObject go = Instantiate(prefab);
                    go.transform.SetParent(transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;

                    var ab = go.GetComponent<IUnitAbility>();
                    if (ab != null)
                        _abilitityList.Add(ab);
                }
            }

            for (int i = 0, count = _abilitityList.Count; i < count; i++)
            {
                var ability = _abilitityList[i];
                ability.SetupInstance(this);
            }
        }

        void InitAnimatorStateAbilities()
        {
            if (_animator != null)
            {
                var stateAbility = _animator.GetBehaviours<AnimatorStateAbility>();
                foreach (var s in stateAbility)
                {
                    _abilitityList.Add(s);
                    s.SetupInstance(this);
                }
            }
        }

        void InitPlayerMakerStateAbilities()
        {
            var fsms = GetComponentsInChildren<PlayMakerFSM>();
            for (var i = 0; i < fsms.Length; i++)
            {
                var fsm = fsms[i];
                for (var j = 0; j < fsm.FsmStates.Length; j++)
                {
                    var s = fsm.FsmStates[j];
                    SetupFsmStates(s);
                }
            }
        }
        void SetupFsmStates(HutongGames.PlayMaker.FsmState state)
        {
            for (var j = 0; j < state.Actions.Length; j++)
            {
                var action = state.Actions[j];
                if (action is IUnitAbility)
                {
                    var ab = (IUnitAbility)action;
                    _abilitityList.Add(ab);
                    ab.SetupInstance(this);
                }
            }
        }

        public void Resume()
        {
            Fsm.SendEvent("Resume");
        }
        public void Pause()
        {
            Fsm.SetState("Pause");
        }

        public void EnableAI()
        {
            AI.EnableAI();
        }
        public void DisableAI()
        {
            AI.DisableAI();
        }

        public void CreateAbilityIDMap(IUnitAbility a)
        {
            if (_idToAbilityMapping.ContainsKey(a.AbilityID))
            {
                Debug.LogErrorFormat(this, "id dupicate {0}", a.AbilityID);
                Debug.Break();
            }
            _idToAbilityMapping.Add(a.AbilityID, a);
        }

        public void AddAbilityToListOnSetup(IUnitAbility a)
        {
            _abilitityList.Add(a);
            a.SetupInstance(this);
        }
        public void AddAbilityToListOnInit(IUnitAbility a)
        {
            _abilitityList.Add(a);
            a.SetupInstance(this);
            CreateAbilityIDMap(a);

            a.Init();
        }
        public void AddAblityAndAfterInit(IUnitAbility a)
        {
            _abilitityList.Add(a);
            a.SetupInstance(this);
            CreateAbilityIDMap(a);

            a.Init();
            a.LateInit();
        }

        public IUnitAbility GetAbilityByID(string abilityID)
        {
            for (var i = 0; i < _abilitityList.Count; i++)
            {
                if (_abilitityList[i].AbilityID == abilityID)
                    return _abilitityList[i];
            }

            return null;
        }

        public T GetAbility<T>()
        {
            for (var i = 0; i < _abilitityList.Count; i++)
            {
                if (_abilitityList[i] is T)
                    return (T)_abilitityList[i];
            }

            return default(T);
        }

        public IEnumerable<T> GetAllAbilities<T>()
        {
            var ret = _abilitityList.Where(a => a is T).Select(a => (T)a);
            return ret;
        }

        public T GetAbility<T>(string abilityID)
        {
            for (var i = 0; i < _abilitityList.Count; i++)
            {
                if (_abilitityList[i].AbilityID == abilityID && _abilitityList[i] is T)
                    return (T)_abilitityList[i];
            }

            return default(T);
        }
        public T GetAbility<T>(Func<T, bool> match)
        {
            for (var i = 0; i < _abilitityList.Count; i++)
            {
                if (_abilitityList[i] is T && match((T)_abilitityList[i]))
                    return (T)_abilitityList[i];
            }

            return default(T);
        }

        public void SendAbilityRPC(IUnitAbility ability, string methodName, object[] parameters)
        {
            if (PhotonNetwork.offlineMode || _view == null || !PhotonNetwork.connected)
            {
                CallAbilityMethod(ability, methodName, parameters);
            }
            else
            {

                PhotonNetwork.RPC(_view, "RPCAbilityMethod", PhotonTargets.All, false, ability.AbilityID, methodName, parameters);
                PhotonNetwork.SendOutgoingCommands();
            }
        }

        [PunRPC]
        void RPCAbilityMethod(string abilityID, string methodName, object[] parameters)
        {
            IUnitAbility ability;
            if (!_idToAbilityMapping.TryGetValue(abilityID, out ability))
            {
                return;
            }


            CallAbilityMethod(ability, methodName, parameters);
        }

        void CallAbilityMethod(IUnitAbility ability, string methodName, object[] parameters)
        {
            var methodInfo = ability.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (methodInfo == null)
            {
                return;
            }

            try
            {
                methodInfo.Invoke(ability, parameters);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public virtual void TriggerEvent(BattleUnitEvent @event)
        {
            if (EventDispatcher != null)
                EventDispatcher.TriggerEvent(@event);
        }

        void Update()
        {
            if (!_initialized)
                return;

            for (var i = 0; i < _abilitityList.Count; i++)
            {
                var a = _abilitityList[i];
                if (a is IUnitAbilityUpdate)
                    ((IUnitAbilityUpdate)a).OnUpdate();
            };
        }

        public void OnUnitDestroy()
        {
            for (var i = 0; i < _abilitityList.Count; i++)
            {
                if (_abilitityList[i] is IUnitAbilityDestroy)
                {
                    ((IUnitAbilityDestroy)_abilitityList[i]).BeforeDestroy();
                }
            }
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                var viewID = _view.viewID;
                PhotonNetwork.manuallyAllocatedViewIds.Remove(viewID);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }
    }
}