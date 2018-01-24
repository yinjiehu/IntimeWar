using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
    [ActionCategory("MechSquad_Photon")]
	public class ConnectToPhoton : FsmStateAction
	{
		//public FsmString ServerAddress;
		public FsmInt ServerPort;
		public FsmString PlayerNickName;

		public const string AppID = "4eab3524-c699-4953-aad2-c5bbf341ebc8";

		public FsmEvent OnJoinLobby;
		public FsmEvent OnReachMaxCCU;
		public FsmEvent OnConnectionError;
		public FsmString ErrorMessage;

		public override void OnEnter()
		{
			base.OnEnter();

            PhotonNetwork.offlineMode = false;
			if (PhotonNetwork.connected)
			{
				Debug.LogWarning("Photon server is already connected");
				PhotonNetwork.Disconnect();
				return;
            }

			var serverAddress = "192.168.1.9";
			Debug.LogFormat("connect to photon server {0} {1}", serverAddress, ServerPort.Value);

            PhotonNetwork.sendRate = 10;
            PhotonNetwork.sendRateOnSerialize = 10;
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.automaticallySyncScene = false;
            PhotonNetwork.playerName = PlayerNickName.Value;

            //var authData = new AuthenticationValues();
            //authData.AuthType = CustomAuthenticationType.Custom;
            //authData.AddAuthParameter("uid", Save.Account.Uid);
            //authData.AddAuthParameter("token", Save.Account.Token);
            //authData.AddAuthParameter("serverID", Save.CurrentServerId);
            //authData.AddAuthParameter("vehicleID", Save.Player.Garage.CurrentSelectedID);
            //PhotonNetwork.AuthValues = authData;
			
            PhotonNetwork.ConnectToMaster(serverAddress, ServerPort.Value, AppID, Application.version);
		}

        public void OnDisconnectedFromPhoton()
        {
            ErrorMessage = "登录对战服务器失败";
            Fsm.Event(OnConnectionError);
        }


        private void OnConnectedToPhoton()
        {
            Debug.Log("connected to photon");
        }

        public void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
			ErrorMessage.Value = "登录对战服务器失败\n" + cause.ToString();
			Debug.LogError("OnFailedToConnectToPhoton : " + cause.ToString());

			Fsm.Event(OnConnectionError);
		}
		
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.LogFormat("auth successed !");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.LogError("OnCustomAuthenticationFailed : " + debugMessage);

            //if (debugMessage == PvpReturnCode.P1000L)
            //{
            //    ErrorMessage = PvpReturnCode.P1000L.Message;
            //}
            //else if (debugMessage == PvpReturnCode.P1010L)
            //{
            //    ErrorMessage = PvpReturnCode.P1010L.Message;
            //}
            //else if (debugMessage == PvpReturnCode.P1020L)
            //{
            //    ErrorMessage = PvpReturnCode.P1010L.Message;
            //}
            //else
            {
                ErrorMessage.Value = "认证失败";
            }
			Fsm.Event(OnConnectionError);
		}
		
		public void OnPhotonMaxCccuReached()
		{
			ErrorMessage.Value = "对战服务器忙，请稍后再试";
			Fsm.Event(OnReachMaxCCU);
		}

		public void OnJoinedLobby()
		{
			Debug.Log("Connect to photon server successed. join the lobby.");
            Fsm.Event(OnJoinLobby);
		}
    }
}