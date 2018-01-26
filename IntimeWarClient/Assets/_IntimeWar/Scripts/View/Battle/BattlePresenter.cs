using System.Linq;
using YJH.Unit;

namespace IntimeWar.View
{
    public static class BattlePresenter
    {
        public static BattleView.Model GetBattleViewModel()
        {
            var playerStatus = GlobalCache.GetPlayerStatus();
            if (!PhotonNetwork.offlineMode)
            {
                var myTeam = PhotonNetwork.player.GetUnitTeam();
                var data = new BattleView.Model()
                {
                    Money = playerStatus.Money,
                    SelfTeamScore = PhotonNetwork.room.GetRoomKillCountByTeam(myTeam),
                    EnemyTeamScore = PhotonNetwork.room.GetRoomKillCountByTeam(myTeam == Team.A ? Team.B : Team.A),

                    KillNumber = 0,
                    SelfTeams = new System.Collections.Generic.List<BattleView.Model.PlayerInfo>() { new BattleView.Model.PlayerInfo() { NickName = "11", Level = 1 } },
                    EnemyTeams = new System.Collections.Generic.List<BattleView.Model.PlayerInfo>() { new BattleView.Model.PlayerInfo() { NickName = "11", Level = 1 } },
                };
                return data;

            }
            else
            {
                var data = new BattleView.Model()
                {
                    Money = 0,
                    SelfTeamScore = 0,
                    EnemyTeamScore = 0,
                    KillNumber = 0,
                    SelfTeams = new System.Collections.Generic.List<BattleView.Model.PlayerInfo>() { new BattleView.Model.PlayerInfo() { NickName = "11", Level = 1 } },
                    EnemyTeams = new System.Collections.Generic.List<BattleView.Model.PlayerInfo>() { new BattleView.Model.PlayerInfo() { NickName = "11", Level = 1 } },
                };
                return data;
            }

        }

        public static short GetMode20TeamScore(byte team)
        {
            var allPlayerList = PhotonNetwork.playerList.OrderBy(p => p.ID).ToList();
            short score = 0;
            for (var i = 0; i < allPlayerList.Count; i++)
            {
                var p = allPlayerList[i];
                if (team == p.GetUnitTeam())
                {
                    score += p.GetKillCount();
                }
            }
            return score;
        }
    }
}