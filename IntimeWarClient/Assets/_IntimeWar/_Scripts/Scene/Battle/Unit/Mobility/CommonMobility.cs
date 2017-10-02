using UnityEngine;

namespace MechSquad.Battle
{
	public class CommonMobility : UnitMobility
	{
		[SerializeField]
		protected float _moveSpeed = 30;
		public float MoveSpeed { get { return _moveSpeed; } }

		protected override void CalculateMoving()
		{
			if (_normalizedMoveDirection != Vector3.zero)
			{
				var distance = Vector3.Distance(_unit.Model.position, _targetWorldPosition);
				var maxCanMoveDistance = _moveSpeed * Time.deltaTime;
				if (maxCanMoveDistance > distance)
				{
					MoveWithGravity(_normalizedMoveDirection * distance);
				}
				else
				{
					MoveWithGravity(_normalizedMoveDirection * maxCanMoveDistance);
				}
			}
			else
			{
				MoveWithGravity(Vector3.zero);
			}
			_normalizedMoveDirection = Vector3.zero;
		}
	}
}