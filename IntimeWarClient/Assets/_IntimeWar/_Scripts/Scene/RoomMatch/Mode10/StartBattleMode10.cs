using HutongGames.PlayMaker;
using MechSquadShared;
using System.Linq;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Room")]
	public class StartBattleMode10 : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (PhotonNetwork.isMasterClient)
			{
				if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace))
					Debug.Log("raise custom event : MatchedBattleStart");

				PhotonCustomEventSender.RaiseResetBattleEvent();
				PhotonHelper.SetMode10SpawnPositionReverse(UnityEngine.Random.Range(0, 2) == 0);
				PhotonHelper.SetMode10RoomBattleState(Mode10RoomStateEnum.Loading);
			}

			PhotonHelper.SetMode10PlayerBattleState(Mode10PlayerStateEnum.Alive);
		}
		
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (PhotonNetwork.room.GetMode10RoomBattleState() == Mode10RoomStateEnum.Loading
				&& PhotonNetwork.playerList.All(p => p.GetMode10PlayerBattleState() == Mode10PlayerStateEnum.Alive))
			{
				Finish();
			}
		}
	}
}