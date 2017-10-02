using UnityEngine;

namespace MechSquad
{
	public enum GameModeEnum
	{
		None = 0,
		/// <summary>
		/// TeamDeathMatch
		/// </summary>
		Mode10 = 10,
	}
	
	public enum Mode10RoomStateEnum
	{
		Garage,
		Loading,
		InBattle,
		Resulting,
	}
	public enum Mode10PlayerStateEnum
	{
		Alive,
		Dead,
	}

	public static class Mode10RommPropertyKey
	{
		public const string SpawnPositionReverse = "SpawnPositionReverse";
	}

	public partial class PhotonHelper
	{
		public const string GameModeKkey = "GameMode";

		/// <summary>
		/// Define at
		///     MechSquad.GameMode
		/// </summary>
		/// <param name="mode"></param>
		public static void SetGameMode(GameModeEnum mode)
		{
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ GameModeKkey, (byte)mode }
			});
		}
		public static GameModeEnum GetGameMode(this Room room)
		{
			object value;
			if (room.CustomProperties.TryGetValue(GameModeKkey, out value))
				return (GameModeEnum)(byte)value;

			return GameModeEnum.None;
		}
		public static bool IsModeInTeamDeathMatach(this Room room)
		{
			return room.GetGameMode() == GameModeEnum.Mode10;
		}

		#region TeamDeathMatch

		public static void SetMode10RoomBattleState(Mode10RoomStateEnum state)
		{
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ MechSquadShared.RoomPropertyKey.BattleState, (byte)state },
			});
		}
		public static Mode10RoomStateEnum GetMode10RoomBattleState(this RoomInfo room)
		{
			object value;
			if (room.CustomProperties.TryGetValue(MechSquadShared.RoomPropertyKey.BattleState, out value))
				return (Mode10RoomStateEnum)(byte)value;

			return Mode10RoomStateEnum.Garage;
		}

		public static void SetMode10SpawnPositionReverse(bool reverse)
		{
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ Mode10RommPropertyKey.SpawnPositionReverse, reverse },
			});
		}
		public static bool GetMode10SpawnPositionReverse(this RoomInfo room)
		{
			object value;
			if (room.CustomProperties.TryGetValue(Mode10RommPropertyKey.SpawnPositionReverse, out value))
				return (bool)value;

			return false;
		}


		public static void SetMode10PlayerBattleState(Mode10PlayerStateEnum state)
		{
			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ MechSquadShared.PlayerPropertyKey.BattleState, (byte)state },
			});
		}
		public static Mode10PlayerStateEnum GetMode10PlayerBattleState(this PhotonPlayer player)
		{
			object value;
			if (player.CustomProperties.TryGetValue(MechSquadShared.PlayerPropertyKey.BattleState, out value))
				return (Mode10PlayerStateEnum)(byte)value;

			return Mode10PlayerStateEnum.Alive;
		}
		//public static void SetMode0PlayerBattleState(Mode0PlayerStateEnum state)
		//{
		//	PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
		//	{
		//		{ MechSquadShared.PlayerPropertyKey.BattleState, (byte)state },
		//	});
		//}
		//public static Mode0PlayerStateEnum GetMode0PlayerBattleState(this RoomInfo room)
		//{
		//	object value;
		//	if (room.CustomProperties.TryGetValue(MechSquadShared.PlayerPropertyKey.BattleState, out value))
		//		return (Mode0PlayerStateEnum)(byte)value;

		//	return Mode0PlayerStateEnum.Garage;
		//}

		#endregion

	}
}