using HutongGames.PlayMaker;
using MechSquad.View;
using UnityEngine;

namespace MechSquad.Fsm
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