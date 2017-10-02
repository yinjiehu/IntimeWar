using HutongGames.PlayMaker;
using System.Collections.Generic;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class LoadOrInitialPlayerStatus : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();

			DemoPlayerStatus.LoadPlayerPrefOrInit();

			Finish();
		}
	}
}