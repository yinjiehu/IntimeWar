using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	[HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
	public class TestFsmBug : FsmStateAbility
	{
		public override void OnEnter()
		{
			base.OnEnter();

			if (_unit.IsPlayerForThisClient)
			{
				Debug.LogFormat("---------------------------------{0}. enter state {1}--------------------------", _unit.name, State.Name);
			}

			if(_unit.IsPlayerForThisClient && Fsm.ActiveState.Name == "Normal" && Fsm.PreviousActiveState.Name == "Paused")
			{
				Debug.Break();
			}


			Finish();
		}
		
	}
}