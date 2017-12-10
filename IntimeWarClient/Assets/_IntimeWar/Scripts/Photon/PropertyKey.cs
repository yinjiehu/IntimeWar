using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MechSquad
{
	public static class PlayerPropertyKey
    {
        public const string InSynchronization = "Sync";
        public const string Initialized = "Initialized";
        public const string Team = "team";
        public const string SeqInTeam = "SeqInTeam";
        public const string Score = "score";
        public const string BattleState = "PlayerBattleState";
        public const string VehicleID = "VehicleID";
        public const string PaintID = "PaintID";
        public const string InitialParam = "InitialParam";
        public const string KillCount = "KillCount";
        public const string DeathCount = "DeathCount";
    }


    public static class RoomPropertyKey
    {
        public const string RobotPlayerInfo = "RobotPlayerInfo";
        public const string BattleState = "RoomBattleState";
        public const string TeamBlueScore = "TeamBlueScore";
        public const string TeamRedScore = "TeamRedScore";
        public const string SelectVehicleCountDownSeconds = "SelectVehicleCountDownSeconds";
        public const string BattlePrepairCountDownSeconds = "BattlePrepairCountDownSeconds";
        public const string InBattleCountDownSeconds = "InBattleCountDownSeconds";
    }

    public static class PhotonParameterCode
    {
        public const byte ActorNr = 254;
        public const byte Photon_CustomEventContent = 245;

    }
}