using HutongGames.PlayMaker;
using MechSquadShared;
using System.Linq;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class SetRoomVisibleAndOpen : FsmStateAction
	{
		public FsmBool _roomVisible;
		public FsmBool _roomOpen;

		public override void OnEnter()
		{
			base.OnEnter();
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.room.IsVisible = _roomVisible.Value;
				PhotonNetwork.room.IsOpen = _roomOpen.Value;
			}

			Finish();
		}
	}
}