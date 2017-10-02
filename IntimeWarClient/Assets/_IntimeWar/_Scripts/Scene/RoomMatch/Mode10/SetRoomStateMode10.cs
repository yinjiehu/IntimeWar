using HutongGames.PlayMaker;
using System.Collections.Generic;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Room")]
	public class SetRoomStateMode10 : FsmStateAction
	{
		public Mode10RoomStateEnum _state;

		public override void OnEnter()
		{
			base.OnEnter();
			if(!PhotonNetwork.offlineMode && PhotonNetwork.isMasterClient)
				PhotonHelper.SetMode10RoomBattleState(_state);

			Finish();
		}
	}
}