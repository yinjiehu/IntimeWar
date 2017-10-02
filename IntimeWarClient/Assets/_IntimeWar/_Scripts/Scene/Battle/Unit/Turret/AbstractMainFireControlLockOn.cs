using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

namespace  MechSquad.Battle
{
	public abstract class AbstractMainFireControlLockOn : Ability, IMainFireControlSystem
	{
		public abstract Transform TurretRoot { get; }
		public abstract Vector3 CurrentTurretDirection { get; }
		public abstract void SetAsCameraFollow();

		AimingModeEnum _aimingMode = AimingModeEnum.Auto;
		public AimingModeEnum CurrentAimingMode { get { return _aimingMode; } }

		[SerializeField]
		float _semiAutoTrackAngle = 15f;

		Vector3 _aimDirection;
		BattleUnit _currentTarget;

		protected RaidarSystem _raidar;

		public override void Init()
		{
			base.Init();
			_raidar = _unit.GetAbility<RaidarSystem>();
		}

		public void SwitchAimingMode(AimingModeEnum type)
		{
			_aimingMode = type;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			UpdateLockOnTarget();
		}

		protected virtual void UpdateLockOnTarget()
		{
			var enemyInfo = _raidar.GetSharedTargets();

			if (_aimingMode == AimingModeEnum.Manual)
			{
				_currentTarget = null;
			}
			else if (_aimingMode == AimingModeEnum.SemiAuto)
			{
				var aimDirection = _aimDirection == Vector3.zero ? CurrentTurretDirection : _aimDirection;

				if (_currentTarget == null || !_currentTarget.STS.CanBeLockedOn || !enemyInfo.ContainsKey(_currentTarget.SeqNo))
				{
					_currentTarget = GetTargetInMinAngle(enemyInfo, _semiAutoTrackAngle);
				}
				else
				{
					var toTargetDirection = _currentTarget.Model.position - _unit.Model.position;
					if (Vector3.Angle(aimDirection, toTargetDirection) > _semiAutoTrackAngle)
						_currentTarget = GetTargetInMinAngle(enemyInfo, _semiAutoTrackAngle);
				}
			}
			else
			{
				if (_currentTarget == null || !_currentTarget.STS.CanBeLockedOn || !enemyInfo.ContainsKey(_currentTarget.SeqNo))
					_currentTarget = GetTargetInMinAngle(enemyInfo);
			}
		}

		BattleUnit GetTargetInMinAngle(Dictionary<int, BattleUnit> enemyInfo, float angleLimit = 360)
		{
			var direction = _aimDirection == Vector3.zero ? CurrentTurretDirection : _aimDirection;

			var aimAngle = Quaternion.FromToRotation(Vector3.forward, direction).eulerAngles.y;
			float minAngle = float.MaxValue;
			BattleUnit minTarget = null;
			using (var itr = enemyInfo.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					var unit = itr.Current.Value;
					if (unit == null || !unit.STS.CanBeLockedOn)
						continue;

					var angle = Mathf.Abs(GetAngleForSelfToTarget(unit) - aimAngle);
					if (angle < angleLimit && angle < minAngle)
					{
						minAngle = angle;
						minTarget = unit;
					}
				}
			}
			return minTarget;
		}

		public void SetAimingDirection(Vector3 direction)
		{
			_aimDirection = direction;
		}

		public Vector3 GetAimingDirection()
		{
			if (_aimingMode == AimingModeEnum.Manual)
			{
				return _aimDirection;
			}

			if (_aimingMode == AimingModeEnum.Auto)
			{
				if (_currentTarget == null || !_currentTarget.STS.CanBeLockedOn)
				{
					//if (_aimDirection == Vector3.zero)
					//	return CurrentTurretDirection;
					//else
					return _aimDirection;
				}

				return _currentTarget.Model.position - _unit.Model.position;
			}

			if (_currentTarget == null || !_currentTarget.STS.CanBeLockedOn)
				return _aimDirection;

			return _currentTarget.Model.position - _unit.Model.position;
		}

		public BattleUnit GetAimingTarget()
		{
			return _currentTarget;
		}

		public BattleUnit SwitchTarget()
		{
			var enemyInfo = _raidar.GetSharedTargets();

			float currentAngle = 0;
			if (_currentTarget != null && _currentTarget.STS.CanBeLockedOn)
			{
				BattleUnit unit;
				if (enemyInfo.TryGetValue(_currentTarget.SeqNo, out unit))
				{
					currentAngle = GetAngleForSelfToTarget(_currentTarget);
				}
			}

			float minAngle = float.MaxValue;
			BattleUnit minUnit = null;
			using (var itr = enemyInfo.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					var unit = itr.Current.Value;
					if (unit == null || !unit.STS.CanBeLockedOn || unit == _currentTarget)
						continue;

					var tempAngle = GetAngleForSelfToTarget(unit);
					if (tempAngle < currentAngle)
						tempAngle += 360;
					if (unit != _currentTarget && tempAngle < minAngle)
					{
						minAngle = tempAngle;
						minUnit = unit;
					}
				}
			}

			if (minUnit != null)
				_currentTarget = minUnit;

			return _currentTarget;
		}

		float GetAngleForSelfToTarget(BattleUnit target)
		{
			return Quaternion.FromToRotation(Vector3.forward, target.Model.position - _unit.Model.position).eulerAngles.y;
		}
	}
}