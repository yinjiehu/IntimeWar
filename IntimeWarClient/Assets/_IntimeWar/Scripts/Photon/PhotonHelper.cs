using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MechSquad
{
    public enum PhotonClientEvCodeEnum : byte
    {
        CustomEvent = 100,

        InstantiateUnit = 130,
        DestroyUnit = 135,
        ResendInstantiateEvent = 140,
        ResetBattle = 150,
    }

    public enum PhotonServerEvCodeEnum : byte
    {
        CustomEvent = 100,

        NewPlayerStartBattle = 125,
        InstantiateUnit = 130,
        DestroyUnit = 135,

        PlayerDisconnected = 101,
    }

    public enum InstantiateUnitEventKey
    {
        CreaterViewID = 0,
        CreaterMethodName = 3,
        Parameters = 4,
        EventTimeStamp = 8,
        CurrentSyncStatus = 10,
    }

    public static partial class PhotonHelper
	{
        public static float GetBattlePrepairElapsedTime()
        {
            object obj;
            if (PhotonNetwork.room.CustomProperties.TryGetValue(RoomPropertyKey.BattlePrepairCountDownSeconds, out obj))
            {
                return (float)obj;
            }
            return 0;
        }
        public static PhotonView GetPhotonView(this UnityEngine.Component com)
		{
            PhotonView pv = com.GetComponent<PhotonView>();
            if (pv != null)
                return pv;
            else
                return null;
		}

        public static bool IsControlByThisClient(this PhotonView view)
        {
            if (PhotonNetwork.offlineMode)
                return true;

            if (view.ownerId == PhotonNetwork.player.ID)
                return true;

            if (view.isSceneView && PhotonNetwork.isMasterClient)
                return true;

            if (PhotonNetwork.isMasterClient)
            {
                var viewPlayer = GetPlayer(view.OwnerActorNr);
				if(viewPlayer == null)
				{
					Debug.LogErrorFormat("Can not find player by actor id {0}", view.OwnerActorNr);
					return true;
				}

                if (viewPlayer.IsInactive || !viewPlayer.IsInSynchronization())
                    return true;
            }

            return false;
        }

		public static PhotonPlayer GetPlayer(int actorID)
		{
            return PhotonNetwork.networkingPeer.GetPlayerWithId(actorID);
		}

		public static PhotonPlayer GetPlayerByUserID(string userID)
		{
            if(PhotonNetwork.offlineMode)
            {
                if (userID == PhotonNetwork.player.UserId)
                    return PhotonNetwork.player;
                else
                    throw new Exception("Can not get player {0} in offline mode");
            }
			var plist = PhotonNetwork.playerList;
			for(var i = 0; i < plist.Length; i++)
			{
				if (plist[i].UserId == userID)
					return plist[i];
			}
			return null;
		}

		//public static PhotonPlayer GetPlayerByUserIDIncludeRobot(string userID)
		//{
		//	var p = GetPlayerByUserID(userID);
		//	if (p != null)
		//		return p;
		//	var robots = GetRobotPlayers();
		//	if (robots != null && robots.ContainsKey(userID))
		//		return robots[userID];

		//	return null;
		//}

		//public static List<PhotonPlayer> GetAllPlayerIncludeRobots()
		//{
		//	var ret = new List<PhotonPlayer>();
		//	ret.AddRange(PhotonNetwork.playerList);
		//	var robots = GetRobotPlayers();
		//	if (robots != null)
		//		ret.AddRange(robots.Values);

		//	return ret;
		//}
		//public static Dictionary<string, PhotonPlayer> GetRobotPlayers()
		//{
		//	object robots;
		//	if (GlobalCache.TryGet(GlobalCacheKey.ROBOT_PLAYER, out robots))
		//	{
		//		return (Dictionary<string, PhotonPlayer>)robots;
		//	}
		//	return null;
		//}
		
        public static bool IsInSynchronization(this PhotonPlayer photonPlayer)
        {
            object value;
            if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.InSynchronization, out value))
                return (bool)value;
            return false;
        }
        public static void SetUnitTeam(byte team)
		{
			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.Team, team }
			});
		}
		public static byte GetUnitTeam(this PhotonPlayer photonPlayer)
		{
			object value;
			if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.Team, out value))
				return (byte)value;

			return 0;
		}
		public static byte GetPlayerTeam(PhotonPlayer p)
		{
			return p.GetUnitTeam();
		}

		public static void SetClassify(string classifyID)
		{
            PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.ClassifyID, classifyID },
			});
		}
		public static string GetClassify(this PhotonPlayer photonPlayer)
		{
			object value;
			if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.ClassifyID, out value))
				return (string)value;
            return "";
		}
		public static void SetSeqInTeam(byte seq)
		{
			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.SeqInTeam, seq},
			});
		}
		public static byte GetSeqInTeam(this PhotonPlayer photonPlayer)
		{
			object value;
			if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.SeqInTeam, out value))
				return (byte)value;

			return 255;
		}

		public static void ResetKillCount()
		{
			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.KillCount, 0},
			});
		}
		public static void AddKillCount()
		{
			var current = PhotonNetwork.player.GetKillCount();

			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.KillCount, (short)(current + 1)},
			});
		}
		public static short GetKillCount(this PhotonPlayer photonPlayer)
		{
			object value;
			if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.KillCount, out value))
				return (short)value;

			return 0;
		}
		public static void AddDeathCount()
		{
			var current = PhotonNetwork.player.GetDeathCount();

			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.DeathCount, (short)(current + 1)},
			});
		}
		public static int GetDeathCount(this PhotonPlayer photonPlayer)
		{
			object value;
			if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.DeathCount, out value))
				return (short)value;

			return 0;
		}

		//public static void SetPlayerBattleState(PlayerPropertyBattleState state)
		//{
		//	PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
		//	{
		//		{ PlayerPropertyKey.BattleState, state }
		//	});
		//}
		//public static PlayerPropertyBattleState GetPlayerBattleState(this PhotonPlayer photonPlayer)
		//{
		//	object value;
		//	if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.BattleState, out value))
		//		return (PlayerPropertyBattleState)(byte)value;

		//	return PlayerPropertyBattleState.BattlePrepair;
		//}

		public static void SetInitialParameter(UnitInitialParameter param)
		{
			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ PlayerPropertyKey.InitialParam, param.ToPhotonHashtable() }
			});
		}
		public static UnitInitialParameter GetInitialParameter(this PhotonPlayer photonPlayer)
		{
			//if (PhotonNetwork.offlineMode)
			//	return UnitInitialParameter.Create(GlobalCache.GetPlayerStatus());

			object value;
			if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.InitialParam, out value))
			{
				return UnitInitialParameter.FromPhotonHashtable((ExitGames.Client.Photon.Hashtable)value);
			}
			return null;
		}

		public static bool TryGetProerty(this PhotonPlayer photonPlayer, string key, out object obj)
		{
			return photonPlayer.CustomProperties.TryGetValue(key, out obj);
		}


        public static void CreateOfflinePhotonPlayerProperties()
		{

			PhotonNetwork.offlineMode = true;
			SetUnitTeam(1);
			SetSeqInTeam(0);
            SetClassify("Player");
			//SetInitialParameter(UnitInitialParameter.Create(GlobalCache.GetPlayerStatus()));
		}
		
		
#if UNITY_EDITOR
		[UnityEditor.MenuItem("Tools/Disconnect From Photon", priority = 80)]
		public static void ForceDisconnect()
		{
			Debug.Log("Try to disconnect");
			PhotonNetwork.Disconnect();
		}

		[MenuItem("Tools/Clear Photon View ID", priority = 81)]
		public static void ClearPhotonViewID()
		{
			var assetPaths = AssetDatabase.GetAllAssetPaths().Where(p => p.StartsWith("Assets/_Prefabs") && p.EndsWith(".prefab")).ToArray();
			for (var i = 0; i < assetPaths.Length; i++)
			{
				var path = assetPaths[i];
				EditorUtility.DisplayProgressBar("Checking game object photon view id...", path, (float)i / assetPaths.Length);

				var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				if (go != null)
				{
					var views = go.GetComponentsInChildren<PhotonView>();
					foreach (var v in views)
					{
						if (v.viewID != 0)
						{
							Debug.LogWarningFormat("Clear photon view id : {0} on {1}, {2}", v.viewID, v.name, path);
							v.viewID = 0;
						}
					}
				}
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.SaveAssets();
		}
#endif
	}
}