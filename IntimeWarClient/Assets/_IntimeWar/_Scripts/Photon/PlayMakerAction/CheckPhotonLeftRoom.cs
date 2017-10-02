using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class CheckPhotonLeftRoom : FsmStateAction
	{
		public FsmEvent LeftRoomEvent;

		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}

		public void OnLeftRoom()
		{
			Debug.LogErrorFormat("Photon left room");
			Fsm.Event(LeftRoomEvent);
		}
	}
}