using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	public class LeaveBattleMode10 : FsmStateAbility
	{
		public FsmEvent _nextEvent;

		public override void OnEnter()
		{
			base.OnEnter();

			BattleScene.Instance.PauseAllUnit();
			if (PhotonNetwork.isMasterClient)
			{
				PhotonHelper.SetMode10RoomBattleState(Mode10RoomStateEnum.Garage);
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (PhotonNetwork.offlineMode)
				return;

			if (PhotonNetwork.room.GetMode10RoomBattleState() == Mode10RoomStateEnum.Garage)
				Fsm.Event(_nextEvent);
		}
	}
}