using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using IntimeWar.Battle;

namespace YJH.Unit
{
	public class Ability : MonoBehaviour, IUnitAbility, IUnitAbilityUpdate, IUnitAbilityDestroy
	{
        string _abilityName;
        public virtual string AbilityID
        {
            get
            {
                if (string.IsNullOrEmpty(_abilityName))
                {
                    _abilityName = string.Format("{0}_{1}", name, GetType().Name);
                }
                return _abilityName;
            }
        }

        protected BattleUnit _unit;
        public BattleUnit Unit { get { return _unit; } }

        protected bool _animatorBasedBahaviour;

        [Serializable]
        public struct FollowUnitModel
        {
            public enum FollowTypeEnum
            {
                None,
                ModelRoot,
                Custom,
            }
            public FollowTypeEnum FollowType;
            public string CustomFollowName;

            public bool FollowModelPosition;
            public bool FollowModelRotation;
        }
        [SerializeField]
        protected FollowUnitModel _followParameter;
        Transform _followTarget;

        public virtual void SetupInstance(BattleUnit unit)
        {
            _unit = unit;
        }

        public virtual void Init()
        {
        }

        public virtual void LateInit()
        {
            InitFollowTarget();
        }

        public virtual void OnUpdate()
        {
            UpdateSendPermanentSynchronization();
        }

        protected virtual void LateUpdate()
        {
            if (_unit != null && _unit.Initialized)
                UpdateFollowTarget();
        }

        public virtual void BeforeDestroy()
        {
        }

        public void CallRPC(string methodName, params object[] args)
        {
            _unit.SendAbilityRPC(this, methodName, args);
        }

        #region Follow Target
        void InitFollowTarget()
        {
            switch (_followParameter.FollowType)
            {
                case FollowUnitModel.FollowTypeEnum.None:
                    break;
                case FollowUnitModel.FollowTypeEnum.ModelRoot:
                    _followTarget = _unit.Model;
                    break;
            }

            if (_followTarget != null && _followParameter.FollowModelPosition)
                transform.position = _followTarget.position;
            if (_followTarget != null && _followParameter.FollowModelRotation)
                transform.rotation = _followTarget.rotation;
        }
        void UpdateFollowTarget()
        {
            if (_followTarget != null && _followParameter.FollowModelPosition)
                transform.position = _followTarget.position;
            if (_followTarget != null && _followParameter.FollowModelRotation)
                transform.rotation = _followTarget.rotation;
        }
        #endregion

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

        public virtual void OnInitSynchronization(object data)
        {
        }

        public virtual object OnSendPermanentSynchronization()
        {
            return null;
        }
        #endregion
    }
}