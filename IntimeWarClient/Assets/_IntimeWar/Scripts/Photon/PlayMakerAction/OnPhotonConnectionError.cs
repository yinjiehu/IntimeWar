using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class OnPhotonConnectionError : FsmStateAction
	{
		public FsmString ErrorMessage;
		public FsmEvent ConnectionFailEvent;
		
        public override void OnEnter()
        {
            base.OnEnter();

			if (!PhotonNetwork.connected)
			{
				Debug.LogErrorFormat("Connection failed on enter {0}.", State.Name);
				Fsm.Event(ConnectionFailEvent);
			}

            Finish();
        }

        public void OnConnectionFail(DisconnectCause cause)
		{
			ErrorMessage.Value = cause.ToString();
			Debug.LogErrorFormat("Connection failed at {0} . caused by {1}", State.Name, cause.ToString());
			Fsm.Event(ConnectionFailEvent);
		}
	}
}