using HutongGames.PlayMaker;
using UnityEngine;

namespace View.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class SetFogAllCleared : FsmStateAction
	{
        public override void OnEnter()
        {
            base.OnEnter();

			//FogOfWar
			//Camera.main.GetComponent<FogOfWar>().SetAll(0);

            Finish();
        }
	}
}