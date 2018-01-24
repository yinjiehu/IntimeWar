using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YJH.Unit
{
	public class AllInput : Ability, IUnitMobilityInput, IUnitAttachmentInput
	{
		public virtual string Name { get { return name; } }
		
		[SerializeField]
		bool _sendSynchronization;
		public bool IsSyncAbility { get { return _sendSynchronization; } }

		protected BattleUnit _unit;
		public BattleUnit Unit { get { return _unit; } }

        public bool Enabled { set; get; }

        public Vector3? NormalizedMoveDirection { set; get; }
        public bool[] AttachmentPress { set; get; }

        public bool[] AttachmentHolding { set; get; }

        public bool[] AttachmentRelease { set; get; }

        public bool[] AttachmentClicked { set; get; }

        public bool MainFireControlPress { set; get; }

        public bool MainFireControlHoding { set; get; }

        public bool MainFireControlRelease { set; get; }

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