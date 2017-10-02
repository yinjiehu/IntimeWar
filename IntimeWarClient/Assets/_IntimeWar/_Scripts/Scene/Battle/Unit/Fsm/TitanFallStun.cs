using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class TitanFallStun : FsmStateAbility
	{
		TitanFall _fallAbility;

		public override void OnEnter()
		{
			base.OnEnter();
			_fallAbility = _unit.GetAbility<TitanFall>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_fallAbility.CurrentState == TitanFall.StateEnum.None)
			{
				Finish();
			}
		}
	}
}