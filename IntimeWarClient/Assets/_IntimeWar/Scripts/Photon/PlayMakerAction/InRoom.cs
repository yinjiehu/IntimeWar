using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class InRoom : FsmStateAction
	{
        public FsmEvent Next;

		public override void OnEnter()
		{
			base.OnEnter();
            
		}

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(PhotonNetwork.room.PlayerCount >=2)
            {
                Fsm.Event(Next);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}