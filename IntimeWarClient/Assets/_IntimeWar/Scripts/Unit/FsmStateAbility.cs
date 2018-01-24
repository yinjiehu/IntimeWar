using MechSquad.Battle;
using System.Linq;
using UnityEngine;

namespace YJH.Unit
{
	public abstract class FsmStateAbility : HutongGames.PlayMaker.FsmStateAction, IUnitAbility
	{
		protected BattleUnit _unit;
		protected Animator _animator;

		[SerializeField]
		bool _sendSynchronization;
		public bool IsSyncAbility { get { return _sendSynchronization; } }

		public bool IsRunning { private set; get; }

        protected string _abilityID;
        public string AbilityID
        {
            get
            {
                if (string.IsNullOrEmpty(_abilityID))
                {
                    _abilityID = string.Format("{0}$[{1}]_{2}", State.Name, State.Actions.ToList().IndexOf(this), GetType().Name);
                }
                return _abilityID;
            }
        }

        public virtual void SetupInstance(BattleUnit unit)
		{
			_unit = unit;
			_animator = _unit.Animator;
		}

		public virtual void Init()
		{

		}

		public virtual void LateInit()
		{
		}

        #region Permanent Synchronization
        public virtual float PermanentSynchronizeInterval { get { return -1; } }
        float _permanentSynchronizationElapsedTime;

        void UpdateSendPermanentSynchronization()
        {
            if (PermanentSynchronizeInterval > 0)
            {
                _permanentSynchronizationElapsedTime += Time.deltaTime;
                if (_permanentSynchronizationElapsedTime > PermanentSynchronizeInterval)
                {
                    _permanentSynchronizationElapsedTime = 0;
                    var data = OnSendPermanentSynchronization();
                    if (data != null)
                    {
                        SpawnerManager.Instance.SendPermanentSyncDataForAbility(_unit.View.viewID, AbilityID, data);
                    }
                }
            }
        }

        public virtual void OnInitSynchronization(object obj)
        {
        }

        public virtual object OnSendPermanentSynchronization()
        {
            return null;
        }
        #endregion
    }
}