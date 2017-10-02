using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace  MechSquad.Battle
{
	public abstract class UnitMobility : Ability
	{
		[SerializeField]
		bool _useCharacterController;

		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected Vector3 _normalizedMoveDirection;

		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected Vector3 _targetWorldPosition;

		public virtual Vector3 CurrentMoveDirection { get { return _normalizedMoveDirection; } }

		CharacterController _controller;

		public override void LateInit()
		{
			base.LateInit();
			if (_useCharacterController)
				_controller = _unit.Model.GetComponent<CharacterController>();
		}

		public virtual void SetMoveDirection(Vector3 moveDirection)
		{
			_normalizedMoveDirection = moveDirection.normalized;
			_targetWorldPosition = _unit.Model.position + _normalizedMoveDirection * 10f;
		}
		public virtual void SetTargetPosition(Vector3 targetWorldPosition)
		{
			_targetWorldPosition = targetWorldPosition;
			_normalizedMoveDirection = (targetWorldPosition - _unit.Model.position).normalized;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			CalculateMoving();

			_normalizedMoveDirection = Vector3.zero;
			_targetWorldPosition = Vector3.zero;
		}

		protected abstract void CalculateMoving();

		public void Move(Vector3 relativePosition)
		{
			if (_useCharacterController)
			{
				if(_controller.enabled)
					_controller.Move(relativePosition);
			}
			else
			{
				_unit.Model.Translate(relativePosition, Space.World);
			}
		}

		public void MoveWithGravity(Vector3 relativePosition)
		{
			relativePosition.y += -20 * Time.deltaTime;
			Move(relativePosition);
		}
	}
}