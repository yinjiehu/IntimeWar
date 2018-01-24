using HutongGames.PlayMaker;
using View;
using YJH.Unit;
using IntimeWar;

namespace IntimeWar.Battle
{
    public class CheckVictory : FsmStateAbility
    {
        public FsmEvent _allDeadEvent;

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (PhotonNetwork.offlineMode)
                return;

            var myTeam = PhotonNetwork.player.GetUnitTeam();
            var selfTeamScore = PhotonNetwork.room.GetRoomKillCountByTeam(myTeam);
            var enemyTeamScore = PhotonNetwork.room.GetRoomKillCountByTeam(myTeam == Team.A ? Team.B : Team.A);
            if (selfTeamScore >= 10 || enemyTeamScore >= 10)
            {
                BattleScene.Instance.PauseAllUnit();
                Fsm.Event(_allDeadEvent);
            }

            var list = PhotonNetwork.playerList;
            if (list.Length < 2)
                return;

            var teamA_allExist = true;
            var teamB_allExist = true;

            for (var i = 0; i < list.Length; i++)
            {
                var p = list[i];
                var unit = UnitManager.Instance.GetPlayerUnitByActorID(p.ID);


                if (p.GetUnitTeam() == Team.A)
                {
                    teamA_allExist = false;
                }
                else if (p.GetUnitTeam() == Team.B)
                {
                    teamB_allExist = false;
                }
            }

            if (teamA_allExist || teamB_allExist)
            {
                BattleScene.Instance.PauseAllUnit();
                Fsm.Event(_allDeadEvent);
            }

        }
    }
}