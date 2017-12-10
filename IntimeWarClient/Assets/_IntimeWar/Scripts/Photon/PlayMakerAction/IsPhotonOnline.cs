using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class IsPhotonOnline : FsmStateAction
	{
		public FsmEvent Online;
		public FsmEvent Offline;
		
        public override void OnEnter()
        {
            base.OnEnter();

			if (PhotonNetwork.connected)
			{
				Fsm.Event(Online);
			}
			else
			{
				PhotonNetwork.offlineMode = true;
				Fsm.Event(Offline);
			}
            Finish();
        }
	}
}