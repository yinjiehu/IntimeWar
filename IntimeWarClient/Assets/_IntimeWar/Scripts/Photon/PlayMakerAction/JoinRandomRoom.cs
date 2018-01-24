using ExitGames.Client.Photon;
using HutongGames.PlayMaker;
using System;
using UnityEngine;

namespace IntimeWar.RealTime
{
    [ActionCategory("MechSquad_Photon")]
    public class JoinRandomRoom : FsmStateAction
    {
		public FsmEvent OnJoinSuccess;
        public FsmEvent OnJoinFailed;
		public FsmString ErrorMessage;

		public override void OnEnter()
        {
            base.OnEnter();
            
            if(!PhotonNetwork.JoinRandomRoom())
            {
            }
        }


        public void OnCreatedRoom()
        {
            Debug.LogFormat("Create room sucess.");
            Fsm.Event(OnJoinSuccess);
        }

		public void OnJoinedRoom() 
		{
			Debug.LogFormat("OnJoinedRoom.");
			Fsm.Event(OnJoinSuccess);
		}

		public void OnLeftRoom()
        {
            Debug.LogFormat("OnLeftRoom.");
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            Fsm.Event(OnJoinFailed);
        }

		public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
			ErrorMessage.Value = JsonUtil.SerializeArgs(codeAndMsg);
			Debug.LogErrorFormat("Join opened battle room failed. {0}", ErrorMessage.Value);
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			Fsm.Event(OnJoinFailed);
		}

        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            ErrorMessage.Value = JsonUtil.SerializeArgs(codeAndMsg);
            Debug.LogErrorFormat("Join opened battle room failed. {0}", ErrorMessage.Value);
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            Fsm.Event(OnJoinFailed);
        }

        public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            ErrorMessage.Value = JsonUtil.SerializeArgs(codeAndMsg);
            Debug.LogFormat("OnPhotonRandomJoinFailed. {0}", ErrorMessage.Value);
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;
            options.IsOpen = true;
            options.IsVisible = true;
            
            PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), options, null);
        }

    }
}