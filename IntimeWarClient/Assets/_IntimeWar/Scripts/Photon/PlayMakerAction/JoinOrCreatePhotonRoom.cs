using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
    [ActionCategory("MechSquad_Photon")]
    public class JoinOrCreatePhotonRoom : FsmStateAction
    {
        public FsmInt PlayerTTL;

		public bool AddClientVersionToRoomID;
        public FsmString ToJoinRoomID;
        public FsmString ToJoinPluginName;
		public FsmBool RoomVisible = true;
		public FsmBool RoomOpen = true;

		string _actualRoomID;

		public FsmEvent OnJoinSuccess;
        
        public FsmEvent OnShouldCallReJoin;
        public FsmEvent OnJoinFailed;
		public FsmString ErrorMessage;

		public override void OnEnter()
        {
            base.OnEnter();

			_actualRoomID = AddClientVersionToRoomID ? ToJoinRoomID.Value + "_" + Application.version : ToJoinRoomID.Value;

			var roomOption = new RoomOptions();
            roomOption.PublishUserId = true;

			roomOption.IsVisible = RoomVisible.Value;
			roomOption.IsOpen = RoomOpen.Value;

			roomOption.PlayerTtl = PlayerTTL.Value;
            roomOption.Plugins = new string[] { ToJoinPluginName.Value };

            Debug.LogFormat("To join or crate room {0}", _actualRoomID);
            
            if (!PhotonNetwork.JoinOrCreateRoom(_actualRoomID, roomOption, PhotonNetwork.lobby))
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