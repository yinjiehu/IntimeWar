using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class DisconnectFromPhoton : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();

			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}

			Finish();
		}
	}
}