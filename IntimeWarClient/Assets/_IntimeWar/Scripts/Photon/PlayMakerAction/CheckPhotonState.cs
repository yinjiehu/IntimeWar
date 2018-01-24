using HutongGames.PlayMaker;
using View;
using UnityEngine;

namespace IntimeWar.Fsm
{
	[ActionCategory("MechSquad_Photon")]
	public class CheckPhotonState : FsmStateAction
	{
		public FsmEvent _notConnect;
		public FsmEvent _inLobby;
		public FsmEvent _inRoom;

		public override void OnEnter()
		{
			base.OnEnter();

			if (!PhotonNetwork.connected)
				Fsm.Event(_notConnect);
			else if (PhotonNetwork.insideLobby)
				Fsm.Event(_inLobby);
			else if(PhotonNetwork.inRoom)
				Fsm.Event(_inRoom);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (!PhotonNetwork.connected)
				Fsm.Event(_notConnect);
			else if (PhotonNetwork.insideLobby)
				Fsm.Event(_inLobby);
			else if (PhotonNetwork.inRoom)
				Fsm.Event(_inRoom);
		}
	}
}