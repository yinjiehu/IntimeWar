using UnityEngine;

namespace MechSquad.Battle
{
	public class MechMobilityLimited : UnitMobility
	{
		[SerializeField]
		protected float _maxSpeed = 30;
		public float MaxSpeed { get { return _maxSpeed; } }

		[SerializeField]
		float _turnSpeed = 360f;

		bool _isForward;

		Animator _animator;
		IMainFireControlSystem _turret;
		
		public override void LateInit()
		{
			base.LateInit();
			_animator = _unit.Animator;
			_turret = _unit.GetAbility<IMainFireControlSystem>();
		}

		protected override void CalculateMoving()
		{
			if (!_unit.IsControlByThisClient)
				return;

			if (_normalizedMoveDirection != Vector3.zero)
			{
				CheckForwad();
				Turning();
				Moving();
			}
			else
			{
				_animator.SetFloat("WalkingSpeed", 0);
			}
		}

		void CheckForwad()
		{
			var angleToTurretDirection = Vector3.Angle(_normalizedMoveDirection, _turret.CurrentTurretDirection);
			_isForward = angleToTurretDirection < 180f;
		}

		void Turning()
		{
			var targetDirection = _isForward ? _normalizedMoveDirection : _normalizedMoveDirection * -1;

			var currentDirection = _unit.Model.transform.forward;
			if (currentDirection == targetDirection)
				return;

			var angle = Vector3.Angle(currentDirection, targetDirection);
			var needSec = angle / _turnSpeed;
			if(needSec <= Time.deltaTime)
			{
				_unit.Model.transform.forward = targetDirection;
			}
			else
			{
				var targetRotation = Quaternion.FromToRotation(Vector3.forward, targetDirection);
				_unit.Model.transform.rotation = Quaternion.Lerp(_unit.Model.transform.rotation, targetRotation, Time.deltaTime / needSec);
			}
		}


		//float _normalizedAccelerate = 1;
		//Vector3 _normalizedcurrentDirection = Vector3.zero;

		void Moving()
		{
			var distance = Vector3.Distance(_unit.Model.position, _targetWorldPosition);
			var maxCanMoveDistance = _maxSpeed * Time.deltaTime;
			if (maxCanMoveDistance > distance)
			{
				_unit.Model.position = _targetWorldPosition;
			}
			else
			{
				MoveWithGravity(_normalizedMoveDirection * maxCanMoveDistance);
			}
		}
	}
}