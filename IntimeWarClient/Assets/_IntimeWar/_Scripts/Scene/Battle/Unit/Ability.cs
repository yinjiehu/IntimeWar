using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace  MechSquad.Battle
{
	public class Ability : MonoBehaviour, IUnitAbility, IUnitAbilityUpdate, IUnitAbilityDestroy
	{
		public virtual string Name { get { return name; } }
		
		[SerializeField]
		bool _sendSynchronization;
		public bool IsSyncAbility { get { return _sendSynchronization; } }

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
				Top,
				Head,
				MainFireControl,
				LeftHandAttachment,
				RightHandAttachment,
				LeftShoulderAttachment,
				RightShoulderAttachment,
				LeftFoot,
				RightFoot,
				Custom,
			}
			public FollowTypeEnum FollowType;
			public string CustomFollowName;
			
			public bool FollowModelPosition;
			public bool FollowModelRotation;
		}
		[SerializeField]
		FollowUnitModel _followParameter;
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
			switch (_followParameter.FollowType)
			{
				case FollowUnitModel.FollowTypeEnum.None:
					break;
				case FollowUnitModel.FollowTypeEnum.ModelRoot:
					_followTarget = _unit.Model;
					break;
				case FollowUnitModel.FollowTypeEnum.Top:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().Top;
					break;
				case FollowUnitModel.FollowTypeEnum.Head:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().Head;
					break;
				case FollowUnitModel.FollowTypeEnum.MainFireControl:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().MainFireControl;
					break;
				case FollowUnitModel.FollowTypeEnum.LeftHandAttachment:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().LeftHandAttachment;
					break;
				case FollowUnitModel.FollowTypeEnum.RightHandAttachment:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().RightHandAttachment;
					break;
				case FollowUnitModel.FollowTypeEnum.LeftShoulderAttachment:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().LeftShoulderAttachment;
					break;
				case FollowUnitModel.FollowTypeEnum.RightShoulderAttachment:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().RightShoulderAttachment;
					break;
				case FollowUnitModel.FollowTypeEnum.LeftFoot:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().LeftFoot;
					break;
				case FollowUnitModel.FollowTypeEnum.RightFoot:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().RightFoot;
					break;
				case FollowUnitModel.FollowTypeEnum.Custom:
					_followTarget = _unit.GetAbility<UnitPositionDefine>().GetPosition(_followParameter.CustomFollowName);
					break;
			}

			if (_followTarget != null && _followParameter.FollowModelPosition)
				transform.position = _followTarget.position;
			if (_followTarget != null && _followParameter.FollowModelRotation)
				transform.rotation = _followTarget.rotation;
		}

		public virtual void OnUpdate()
		{
			if (_followTarget != null && _followParameter.FollowModelPosition)
				transform.position = _followTarget.position;
			if (_followTarget != null && _followParameter.FollowModelRotation)
				transform.rotation = _followTarget.rotation;
		}

		public virtual void BeforeDestroy()
		{
		}

		public void CallRPC(string methodName, params object[] args)
		{
			_unit.SendAbilityRPC(this, methodName, args);
		}
	}
}