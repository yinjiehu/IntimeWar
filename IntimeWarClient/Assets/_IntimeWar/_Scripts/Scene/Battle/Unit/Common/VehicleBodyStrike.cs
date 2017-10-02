using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;
using UnityEngine.UI;

namespace MechSquad.Battle
{
	public class VehicleBodyStrike : CollisionTiggerProcessor
	{
		Collider _collider;

		public override void LateInit()
		{
			base.LateInit();

			for (var i = 0; i < _actualCollisionEventTriggers.Count; i++)
			{
				_actualCollisionEventTriggers[i].DisableAllColliders();
			}

			_unit.STS.BodyStrikeEnable.EvOnValueChange += OnBodyStrikeStatusChange;
		}

		private void OnBodyStrikeStatusChange(bool value)
		{
			if (value)
			{
				for (var i = 0; i < _actualCollisionEventTriggers.Count; i++)
				{
					_actualCollisionEventTriggers[i].EnableAllColliders();
				}
			}
			else
			{
				for (var i = 0; i < _actualCollisionEventTriggers.Count; i++)
				{
					_actualCollisionEventTriggers[i].DisableAllColliders();
				}
			}
		}

		public override void ProcessCollisionEvent(CollisionEventReceiver otherUnit, Collision collision)
		{
			otherUnit.OnEvent(new VehicleBodyStrikeEvent()
			{
				SourcePosition = _unit.Model.position,
				Attacker = _unit.Info,
			});
		}
	}
}