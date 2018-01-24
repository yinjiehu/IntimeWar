using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections.Generic;

namespace IntimeWar.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class DisplayRoomList : FsmStateAction
	{
		public int _count;
		
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (PhotonNetwork.room != null)
			{
				if (PhotonNetwork.room.PlayerCount > 1)
				{
					Finish();
				}
			}
			else
			{
				Debug.LogErrorFormat("room is null!");
			}
		}
	}
}