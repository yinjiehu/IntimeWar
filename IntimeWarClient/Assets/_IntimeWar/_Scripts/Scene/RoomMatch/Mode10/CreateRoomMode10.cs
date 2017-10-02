using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
    [ActionCategory("MechSquad_Room")]
    public class CreateRoomMode10 : FsmStateAction
    {
        public FsmInt PlayerTTL;

		public bool AddClientVersionToRoomID;
        public FsmString ToJoinRoomID;
        public FsmString ToJoinPluginName;

		string _actualRoomID;

		public FsmEvent OnJoinSuccess;
        public FsmEvent OnJoinFailed;
		public FsmString ErrorMessage;

		public override void OnEnter()
        {
            base.OnEnter();

			_actualRoomID = AddClientVersionToRoomID ? ToJoinRoomID.Value + "_" + Application.version : ToJoinRoomID.Value;

			var roomOption = new RoomOptions();
            roomOption.PublishUserId = true;

			roomOption.IsVisible = true;
			roomOption.IsOpen = true;

            roomOption.PlayerTtl = PlayerTTL.Value;
			roomOption.Plugins = new string[] { ToJoinPluginName.Value };

            Debug.LogFormat("To create room {0}", _actualRoomID);
            
            if (!PhotonNetwork.CreateRoom(_actualRoomID, roomOption, PhotonNetwork.lobby))
            {
                ErrorMessage = "Create room error";
                Fsm.Event(OnJoinFailed);
            }
        }
		
		public void OnJoinedRoom()
		{
			Debug.LogFormat("Joined room after create sucess {0}", _actualRoomID);

			PhotonHelper.SetGameMode(GameModeEnum.Mode10);
			PhotonNetwork.room.SetPropertiesListedInLobby(new string[] { PhotonHelper.GameModeKkey, MechSquadShared.RoomPropertyKey.BattleState });
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

  //      public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
  //      {
  //          if ((short)codeAndMsg[0] == ErrorCode.JoinFailedFoundInactiveJoiner)
  //          {
  //              Debug.LogFormat("JoinFailedFoundInactiveJoiner, try to rejoin room...");
  //              Fsm.Event(OnShouldCallReJoin);
  //          }
  //          else
  //          {
  //              ErrorMessage.Value = JsonUtil.SerializeArgs(codeAndMsg);
  //              Debug.LogErrorFormat("Join opened battle room failed. {0}", ErrorMessage.Value);
  //              if (PhotonNetwork.connected)
  //              {
  //                  PhotonNetwork.Disconnect();
  //              }
  //              Fsm.Event(OnJoinFailed);
  //          }
		//}
		public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
			//ErrorMessage.Value = JsonUtil.SerializeArgs(codeAndMsg);
			//Debug.LogErrorFormat("Create mode10 battle room failed. {0}", ErrorMessage.Value);
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			Fsm.Event(OnJoinFailed);
		}
	}
}