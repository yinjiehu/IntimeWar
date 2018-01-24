using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class LeavePhotonRoom : FsmStateAction
    {
		public FsmEvent OnLeftAndJoinLobby;
		public FsmEvent OnLeftRoomError;

		public override void OnEnter()
		{
			base.OnEnter();
            
			if(PhotonNetwork.insideLobby)
				Fsm.Event(OnLeftAndJoinLobby);
			else if(!PhotonNetwork.LeaveRoom())
                Fsm.Event(OnLeftRoomError);
        }

        public void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
        }

        public void OnJoinedLobby()
        {
            Debug.Log("Connect to photon server successed. join the lobby.");
			Fsm.Event(OnLeftAndJoinLobby);
        }
	}
}