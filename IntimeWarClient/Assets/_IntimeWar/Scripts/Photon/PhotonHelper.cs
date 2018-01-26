using System;
using UnityEngine;
using System.Linq;
using YJH.Unit;
using Shared.Models;
using Demo;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IntimeWar
{
    public static partial class PhotonHelper
    {
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
                if (viewPlayer == null)
                {
                    Debug.LogErrorFormat("Can not find player by actor id {0}", view.OwnerActorNr);
                    return true;
                }

                if (viewPlayer.IsInactive)
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
            if (PhotonNetwork.offlineMode)
            {
                if (userID == PhotonNetwork.player.UserId)
                    return PhotonNetwork.player;
                else
                    throw new Exception("Can not get player {0} in offline mode");
            }
            var plist = PhotonNetwork.playerList;
            for (var i = 0; i < plist.Length; i++)
            {
                if (plist[i].UserId == userID)
                    return plist[i];
            }
            return null;
        }

        //public static bool IsInSynchronization(this PhotonPlayer photonPlayer)
        //{
        //    object value;
        //    if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.InSynchronization, out value))
        //        return (bool)value;
        //    return false;
        //}
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

        public static void SetClassify(string classify)
        {
            PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                { PlayerPropertyKey.ClassifyID, classify },
            });
        }
        public static string GetClassify(this PhotonPlayer photonPlayer)
        {
            object value;
            if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.ClassifyID, out value))
                return (string)value;

            throw new Exception("Can not get vehicle id from photon player " + photonPlayer.ID);
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

        public static short GetRoomKillCountByTeam(this Room room, byte team)
        {
            if (PhotonNetwork.room == null)
                return 0;
            object value;
            if (PhotonNetwork.room.CustomProperties.TryGetValue(PlayerPropertyKey.KillCount + PlayerPropertyKey.Team + team, out value))
            {
                return short.Parse(value.ToString());
            }
            return 0;
        }

        public static short GetRoomDeadCountByTeam(this Room room, byte team)
        {
            if (PhotonNetwork.room == null)
                return 0;
            object value;
            if (PhotonNetwork.room.CustomProperties.TryGetValue(PlayerPropertyKey.DeathCount + PlayerPropertyKey.Team + team, out value))
            {
                return short.Parse(value.ToString());
            }
            return 0;
        }

        public static short GetRoomKillCountByID(this Room room, int id)
        {
            if (PhotonNetwork.room == null)
                return 0;
            object value;
            if (PhotonNetwork.room.CustomProperties.TryGetValue(PlayerPropertyKey.KillCount + id, out value))
            {
                return short.Parse(value.ToString());
            }
            return 0;
        }

        public static short GetRoomDeadCountByID(this Room room, int id)
        {
            if (PhotonNetwork.room == null)
                return 0;
            object value;
            if (PhotonNetwork.room.CustomProperties.TryGetValue(PlayerPropertyKey.DeathCount + id, out value))
            {
                return short.Parse(value.ToString());
            }
            return 0;
        }

        public static void ResetKillCount()
        {
            var team = PhotonNetwork.player.GetUnitTeam();
            Room room = PhotonNetwork.room;
            var current = room.GetRoomKillCountByTeam(team);

            room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                { PlayerPropertyKey.KillCount + PlayerPropertyKey.Team + team, 0 },
            });
        }

        public static void ResetKillCount(this Room room)
        {
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(PlayerPropertyKey.KillCount + PlayerPropertyKey.Team + Team.A, 0);
            properties.Add(PlayerPropertyKey.KillCount + PlayerPropertyKey.Team + Team.B, 0);
            room.SetCustomProperties(properties);
        }

        public static void ResetDeadCount(this Room room)
        {

            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(PlayerPropertyKey.DeathCount + PlayerPropertyKey.Team + Team.A, 0);
            properties.Add(PlayerPropertyKey.DeathCount + PlayerPropertyKey.Team + Team.B, 0);
            room.SetCustomProperties(properties);
        }

        public static void AddKillCount()
        {
            if (PhotonNetwork.offlineMode)
                return;
            var team = PhotonNetwork.player.GetUnitTeam();
            Room room = PhotonNetwork.room;
            var current = room.GetRoomKillCountByTeam(team);
            var playerKill = room.GetRoomKillCountByID(PhotonNetwork.player.ID);
            room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                { PlayerPropertyKey.KillCount + PlayerPropertyKey.Team + team, (short)(current + 1)},
                { PlayerPropertyKey.KillCount + PhotonNetwork.player.ID, (short)(playerKill + 1)},
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
            var team = PhotonNetwork.player.GetUnitTeam();
            Room room = PhotonNetwork.room;
            var current = room.GetRoomKillCountByTeam(team);
            var playerDead = room.GetRoomDeadCountByID(PhotonNetwork.player.ID);
            room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                { PlayerPropertyKey.DeathCount + PlayerPropertyKey.Team + team, (short)(current + 1)},
                { PlayerPropertyKey.DeathCount + PhotonNetwork.player.ID, (short)(playerDead + 1)},
            });
        }
        public static int GetDeathCount(this PhotonPlayer photonPlayer)
        {
            object value;
            if (photonPlayer.CustomProperties.TryGetValue(PlayerPropertyKey.DeathCount, out value))
                return (short)value;

            return 0;
        }

        public static void SetInitialParameter(UnitInitialParameter param)
        {
            PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                { PlayerPropertyKey.InitialParam, param.ToPhotonHashtable() }
            });
        }
        public static UnitInitialParameter GetInitialParameter(this PhotonPlayer photonPlayer)
        {
            if (PhotonNetwork.offlineMode)
                return UnitInitialParameter.Create(GlobalCache.GetPlayerStatus());

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
            DemoPlayerSettings.InitSettings();
            DemoSkillSettings.InitSettings();
            //PhotonNetwork.player.UserId = Save.Account.Uid;
            //PhotonNetwork.player.NickName = Save.Player.Basic.NickName;
            var player = new PlayerStatus();
            player.PlayerClassify = "Player_1_1";
            GlobalCache.SetPlayerStatus(player);
            SetUnitTeam(1);
            SetSeqInTeam(0);
            SetClassify("Player_1_1");
            SetInitialParameter(UnitInitialParameter.Create(GlobalCache.GetPlayerStatus()));
            //PhotonNetwork.player.SetVehicleAndPaint(currentTankStatus.TankTypeID, currentTankStatus.Paints.CurrentSelectedPaintID);
            //PhotonNetwork.player.SetInitialParameter(UnitInitialParam.Create(currentTankStatus.TankTypeID, Save.Player));
        }

    }
}