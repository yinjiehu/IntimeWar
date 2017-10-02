using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace MechSquad.Battle
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
		Dictionary<IUnitAbility, string> _abilityToIdMapping = new Dictionary<IUnitAbility, string>();

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
				for (int i = 0; i < _abilitityList.Count; i++)
				{
					var ability = _abilitityList[i];
					if (ability.IsSyncAbility && ability is Component)
						_view.ObservedComponents.Add((Component)ability);
				}

				var animatorIndex = _view.ObservedComponents.FindIndex(c => c is PhotonAnimatorView);
				if(animatorIndex >= 0)
				{
					var v = _view.ObservedComponents[animatorIndex];
					_view.ObservedComponents.RemoveAt(animatorIndex);
					_view.ObservedComponents.Add(v);
				}
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
					if(ab != null)
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
			for(var i = 0; i < fsms.Length; i++)
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
			var id = string.Format("{0}_{1}", a.Name, a.GetType().Name);
			if (_idToAbilityMapping.ContainsKey(id))
			{
				Debug.LogErrorFormat(this, "id dupicate {0}", id);
				Debug.Break();
			}
			_idToAbilityMapping.Add(id, a);
			_abilityToIdMapping.Add(a, id);
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

		public IUnitAbility GetAbility(string abilityName)
		{
			for(var i = 0; i< _abilitityList.Count; i++)
			{
				if (_abilitityList[i].Name == abilityName)
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

		public T GetAbility<T>(string abilityName)
		{
			for (var i = 0; i < _abilitityList.Count; i++)
			{
				if (_abilitityList[i].Name == abilityName && _abilitityList[i] is T)
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
				//var str = JsonUtilNet.Serialize(parameters);
				if (MechSquadPreference.LogLevel >= LogLevelEnum.Debug)
					Debug.LogFormat(this, "Send    unit rpc. {0} : {1}.{2}.{3} : {4}",
						_view.viewID, this.name, ability.Name, methodName, parameters.ToStringFull());

				PhotonNetwork.RPC(_view, "RPCAbilityMethod", PhotonTargets.All, false, _abilityToIdMapping[ability], methodName, parameters);
				PhotonNetwork.SendOutgoingCommands();
			}
		}

		[PunRPC]
		void RPCAbilityMethod(string abilityID, string methodName, object[] parameters)
		{
			IUnitAbility ability;
			if (!_idToAbilityMapping.TryGetValue(abilityID, out ability))
			{
				//Debug.LogErrorFormat(this, "Receiver unit ability rpc. Can not find ability by id {0}, in unit {1}, method: {2}. paramters : {3}",
				//	abilityID, name, methodName, JsonUtil.SerializeArgs(parameters));
				return;
			}

			//if (MechSquadPreference.LogLevel >= LogLevelEnum.Debug)
			//	Debug.LogFormat(this, "Receive unit rpc. {0} : {1}.{2}.{3} : {4} ",
			//		_view.viewID, this.name, ability.Name, methodName, JsonUtil.SerializeArgs(parameters));

			CallAbilityMethod(ability, methodName, parameters);
		}

		void CallAbilityMethod(IUnitAbility ability, string methodName, object[] parameters)
		{
			var methodInfo = ability.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				//Debug.LogErrorFormat(this, "Can not find method {0} in unit {1}, ability {2}, parameters {3}",
				//	methodName, name, ability.Name, JsonUtil.SerializeArgs(parameters));
				return;
			}

			//var parameterInfo = methodInfo.GetParameters();

			////foreach (var p in parameters)
			////{
			////	var type = p.GetType();
			////	Debug.Log("type : " + type.FullName + "  value : " + p);
			////}
			////foreach(var p in methodInfo.GetParameters())
			////{
			////	Debug.Log("method paramet type : " + p.ParameterType.FullName);
			////}

			//if (parameterInfo.Length != parameters.Length)
			//{
			//	Debug.LogErrorFormat(this, "rpc call {0}.{1} failed. parameter length not equal", ability.Name, methodName);
			//	return;
			//}
			//for (var i = 0; i < parameterInfo.Length; i++)
			//{
			//	if (parameterInfo[i].ParameterType == typeof(float) && parameters[i].GetType() == typeof(double))
			//	{
			//		parameters[i] = (float)((double)parameters[i]);
			//	}
			//}
			try
			{
				methodInfo.Invoke(ability, parameters);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				//Debug.LogErrorFormat("Error occured when call rpc method {0} in unit {1}, ability {2}, parameters {3}",
				//	methodName, name, ability.Name, JsonUtil.SerializeArgs(parameters));
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

		//public void OnUnitInactive()
		//{
		//	_unitState = UnitStateEnum.Inactive;
		//}

		public void OnUnitDestroy()
		{
			for (var i = 0; i < _abilitityList.Count; i++)
			{
				if(_abilitityList[i] is IUnitAbilityDestroy)
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
			//if (stream.isReading)
			//{
			//	IsDead = (bool)stream.ReceiveNext();
			//}
			//else
			//{

			//}
		}
	}
}