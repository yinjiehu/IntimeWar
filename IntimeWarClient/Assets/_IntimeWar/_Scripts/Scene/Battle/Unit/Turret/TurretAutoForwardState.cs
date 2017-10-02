using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	[ActionCategory("MechSquad_Unit")]
	public class TurretAutoForwardState : FsmStateAbility
	{
		public FsmVector3 _currentDirection;
		public FsmVector3 _targetDirection;

		public float _turnSpeed;
		public FsmEvent _toFreeAimingEvent;
		
		public override void OnLateUpdate()
		{
			base.OnLateUpdate();

			if (_unit != null && _unit.Initialized && _unit.IsControlByThisClient)
			{
				if (_targetDirection.Value == Vector3.zero)
				{
					_currentDirection.Value = RotateTowards(_currentDirection.Value, _unit.Model.forward);
				}
				else
				{
					Fsm.Event(_toFreeAimingEvent);
				}
			}
		}

		Vector3 RotateTowards(Vector3 from, Vector3 to)
		{
			if (from == to)
			{
				return to;
			}

			var angle = Quaternion.FromToRotation(from, to).eulerAngles.y;
			bool rotateClockwise = true;
			if (angle > 180)
			{
				rotateClockwise = false;
				angle = 360 - angle;
			}

			var needSec = angle / _turnSpeed;
			if (needSec <= Time.deltaTime)
			{
				return to;
			}

			var toRotateAngle = _turnSpeed * Time.deltaTime * (rotateClockwise ? 1 : -1);

			var currentRotation = Quaternion.FromToRotation(Vector3.forward, from);
			var currentRotationEuler = currentRotation.eulerAngles;
			currentRotationEuler.y += toRotateAngle;

			return Quaternion.Euler(currentRotationEuler) * Vector3.forward;

		}
	}
}