using MechSquad.Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MechSquad.View
{
	public class Mode10Presenter : BaseView
	{
		static List<Color> allyPlayerColor = new List<Color>();

		public static Mode10CurrentPlayerInfoView.Model GetPlayersInfo()
		{
			var data = new Mode10CurrentPlayerInfoView.Model();
			data.Allies = new List<Mode10CurrentPlayerInfoElement.Model>();
			data.Enemies = new List<Mode10CurrentPlayerInfoElement.Model>();

			var selfTeam = PhotonNetwork.player.GetTeam();
			var allPlayerList = PhotonNetwork.playerList.OrderBy(p => p.ID).ToList();

			for (var i = 0; i < allPlayerList.Count; i++)
			{
				var p = allPlayerList[i];

				var unit = UnitManager.Instance.GetPlayerUnitByActorID(p.ID);
				var m = new Mode10CurrentPlayerInfoElement.Model()
				{
					ActorID = p.ID,
					DisplayText = string.IsNullOrEmpty(p.NickName) ? "" : p.NickName.Substring(0, 1),
					ShowDeadFlag = unit == null || unit.IsDead,					
				};
				if (selfTeam == p.GetTeam())
				{
					data.Allies.Add(m);
				}
				else
				{
					data.Enemies.Add(m);
				}
			}

			return data;
		}

		public static Mode10ResultView.Model GetBattleResultInfo()
		{
			var data = new Mode10ResultView.Model();
			data.Allies = new List<Mode10ResultPlayerElement.Model>();
			data.Enemies = new List<Mode10ResultPlayerElement.Model>();

			var selfTeam = PhotonNetwork.player.GetTeam();
			var allPlayerList = PhotonNetwork.playerList.OrderBy(p => p.ID).ToList();
			for (var i = 0; i < allPlayerList.Count; i++)
			{
				var p = allPlayerList[i];
				var m = new Mode10ResultPlayerElement.Model()
				{
					Self = p.ID == PhotonNetwork.player.ID,
					NickName = p.NickName,
					KillCount = p.GetKillCount().ToString(),
					DeathCount = p.GetDeathCount().ToString(),
				};
				if (selfTeam == p.GetTeam())
				{
					data.Allies.Add(m);
					data.TotalKillCountAlly += p.GetKillCount();
				}
				else
				{
					data.Enemies.Add(m);
					data.TotalKillCountEnemy += p.GetKillCount();
				}
			}

			while (data.Allies.Count < data.Enemies.Count)
				data.Allies.Add(new Mode10ResultPlayerElement.Model() { NickName = "", KillCount = "", DeathCount = "" });
			while (data.Enemies.Count < data.Allies.Count)
				data.Enemies.Add(new Mode10ResultPlayerElement.Model() { NickName = "", KillCount = "", DeathCount = "" });

			return data;
		}
	}
}