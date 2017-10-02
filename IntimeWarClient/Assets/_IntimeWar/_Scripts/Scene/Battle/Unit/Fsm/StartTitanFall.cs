using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class StartTitanFall : FsmStateAbility
	{
		TitanFall _fallAbility;

		public override void OnEnter()
		{
			base.OnEnter();
			_fallAbility = _unit.GetAbility<TitanFall>();
			_fallAbility.StartFall(_unit.Model.position);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_fallAbility.CurrentState == TitanFall.StateEnum.FallStun)
			{
				Finish();
			}
		}
	}
}