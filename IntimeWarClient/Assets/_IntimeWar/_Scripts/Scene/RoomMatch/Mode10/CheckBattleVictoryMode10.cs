using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	public class CheckBattleVictoryMode10 : FsmStateAbility
	{
		public FsmEvent _allDeadEvent;

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (PhotonNetwork.offlineMode)
				return;

			var list = PhotonNetwork.playerList;
			if (list.Length < 2)
				return;

			var teamA_allDead = true;
			var teamB_allDead = true;

			for (var i = 0; i < list.Length; i++)
			{
				var p = list[i];
				var unit = UnitManager.Instance.GetPlayerUnitByActorID(p.ID);


				if(p.GetUnitTeam()  == Team.A && p.GetMode10PlayerBattleState() == Mode10PlayerStateEnum.Alive)
				{
					teamA_allDead = false;
				}
				else if(p.GetUnitTeam() == Team.B && p.GetMode10PlayerBattleState() == Mode10PlayerStateEnum.Alive)
				{
					teamB_allDead = false;
				}
			}

			if (teamA_allDead || teamB_allDead)
			{
				BattleScene.Instance.PauseAllUnit();
				Fsm.Event(_allDeadEvent);
			}
		}
	}
}