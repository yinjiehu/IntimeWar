using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class StartOpenedBattle : FsmStateAction
	{
		public string ToLoadSceneName;
		
        public FsmString ErrorMessage;
		
		public FsmEvent OnError;

		public override void OnEnter()
		{
			base.OnEnter();
			//PhotonCustomEventReceiver.RegistCustomEvent<S2CStartOpenedBattle>(OnReceiveStartBattle);

			//PhotonCustomEventSender.RaisePhotonCustomEvent(new C2SStartOpenedBattle()
			//{
			//});
		}

		public override void OnExit()
		{
			base.OnExit();
			//PhotonCustomEventReceiver.UnregistCustomEvent<S2CStartOpenedBattle>();
		}

		void OnReceiveStartBattle()
		{
			Finish();
		}
		
		public void OnLeftRoom()
		{
			Debug.LogErrorFormat("left match making room unexpected");
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			Fsm.Event(OnError);
		}

		public void OnConnectionFail(DisconnectCause cause)
		{
			ErrorMessage.Value = cause.ToString();
			Debug.LogErrorFormat("Connection failed when join openned battle. {0}", cause.ToString());
			Fsm.Event(OnError);
		}

		//public class a : PunBehaviour
		//{
		//	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		//	{
		//		base.OnPhotonJoinRoomFailed(codeAndMsg);
		//	}
		//	public override void OnConnectionFail(DisconnectCause cause)
		//	{
		//		base.OnConnectionFail(cause);
		//	}
		//}
	}
}