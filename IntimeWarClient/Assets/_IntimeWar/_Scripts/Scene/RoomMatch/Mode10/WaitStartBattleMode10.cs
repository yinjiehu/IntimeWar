using HutongGames.PlayMaker;
using System.Linq;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Room")]
	public class WaitStartBattleMode10 : FsmStateAction
	{
		public FsmEvent _event;

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (PhotonNetwork.room.GetMode10RoomBattleState() == Mode10RoomStateEnum.Loading)
			{
				Fsm.Event(_event);
			}
		}
	}
}