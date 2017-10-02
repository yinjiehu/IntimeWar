
using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class SetFogAllCleared : FsmStateAction
	{
        public override void OnEnter()
        {
            base.OnEnter();


            Finish();
        }
	}
}