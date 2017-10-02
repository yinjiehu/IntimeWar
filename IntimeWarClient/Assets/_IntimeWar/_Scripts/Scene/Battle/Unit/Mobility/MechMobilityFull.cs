using UnityEngine;

namespace MechSquad.Battle
{
	public class MechMobilityFull : UnitMobility
	{
		[SerializeField]
		protected float _maxSpeed = 30;
		public float MaxSpeed { get { return _maxSpeed; } }

		[SerializeField]
		float _turnSpeed = 360f;

		Animator _animator;
		public override void Init()
		{
			base.Init();
			_maxSpeed = _unit.InitialParameter.GetParameter(ConstParameter.MaxSpeed);
		}

		public override void LateInit()
		{
			base.LateInit();
			_animator = _unit.Animator;
		}

		protected override void CalculateMoving()
		{
			if (!_unit.IsControlByThisClient)
				return;

			if (_normalizedMoveDirection != Vector3.zero)
			{
				Turning();
				Moving();
			}
			else
			{
				_animator.SetFloat("WalkingSpeed", 0);
				MoveWithGravity(Vector3.zero);
			}
		}

		void Turning()
		{
			var currentDirection = _unit.Model.transform.forward;
			if (currentDirection == _normalizedMoveDirection)
				return;

			var angle = Quaternion.FromToRotation(currentDirection, _normalizedMoveDirection).eulerAngles.y;
			bool rotateClockwise = true;
			if (angle > 180)
			{
				rotateClockwise = false;
				angle = 360 - angle;
			}
			
			var needSec = angle / _turnSpeed;
			if (needSec <= Time.deltaTime)
			{
				_unit.Model.transform.forward = _normalizedMoveDirection;
			}
			else
			{
				var toRotateAngle = _turnSpeed * Time.deltaTime * (rotateClockwise ? 1 : -1);
				_unit.Model.transform.Rotate(Vector3.up, toRotateAngle);
			}
		}


		//float _normalizedAccelerate = 1;
		//Vector3 _normalizedcurrentDirection = Vector3.zero;

		void Moving()
		{
			_animator.SetFloat("WalkingSpeed", 1);

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