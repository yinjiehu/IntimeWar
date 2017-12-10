using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
    [ActionCategory("MechSquad_Photon")]
    public class JoinPhotonRoom : FsmStateAction
    {
        //public FsmInt PlayerTTL;
		public bool AddClientVersionToRoomID;
		//      public FsmString ToJoinRoomID;
		//      public FsmString ToJoinPluginName;
		//public FsmBool RoomVisible = true;
		//public FsmBool RoomOpen = true;

		string _actualRoomID;

		public FsmEvent OnJoinSuccess;
        
        public FsmEvent OnShouldCallReJoin;
        public FsmEvent OnJoinFailed;
		public FsmString ErrorMessage;

		public override void OnEnter()
        {
            base.OnEnter();

            _actualRoomID = "";
			
            Debug.LogFormat("To join room {0}", _actualRoomID);
            
            if (!PhotonNetwork.JoinRoom(_actualRoomID))
            {
                ErrorMessage = "Join room error";
                Fsm.Event(OnJoinFailed);
            }
        }

        public void OnJoinedRoom()
        {
            Debug.LogFormat("joined room sucess {0}", _actualRoomID);
            Fsm.Event(OnJoinSuccess);
        }

        public void OnLeftRoom()
        {
            Debug.LogErrorFormat("left room unexpected");
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            Fsm.Event(OnJoinFailed);
        }

        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            if ((short)codeAndMsg[0] == ErrorCode.JoinFailedFoundInactiveJoiner)
            {
                Debug.LogFormat("JoinFailedFoundInactiveJoiner, try to rejoin room...");
                Fsm.Event(OnShouldCallReJoin);
            }
            else
            {
                ErrorMessage.Value = JsonUtil.SerializeArgs(codeAndMsg);
                Debug.LogErrorFormat("Join opened battle room failed. {0}", ErrorMessage.Value);
                if (PhotonNetwork.connected)
                {
                    PhotonNetwork.Disconnect();
                }
                Fsm.Event(OnJoinFailed);
            }
        }
    }
}