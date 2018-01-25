using System.Collections;
using UnityEngine;
using System;
using Shared.Models;
using IntimeWar.Data;

namespace IntimeWar
{
	public static partial class GlobalCacheKey
	{
		public const string PlayerStatus = "PlayerStatus";
        public const string CustomPref = "CustomPref";
        public const string Control = "Control";
        public const string Graphic = "Graphic";
        public const string Keys = "Keys";
        public const string PlayerSettingsCollection = "PlayerSettingsCollection";

		public const string SkillSettingsCollection = "SkillSettingsCollection";
		public const string PassiveAttachmentSettingsCollection = "PassiveAttachmentSettingsCollection";
	}

	public partial class GlobalCache
	{
        public static PlayerStatus GetPlayerStatus()
        {
            object ret;
            if (GlobalCache.TryGet(GlobalCacheKey.PlayerStatus, out ret))
                return ret as PlayerStatus;
            return null;
        }

        public static void SetPlayerStatus(PlayerStatus player)
        {
            GlobalCache.Set(GlobalCacheKey.PlayerStatus, player);
        }

        public static PlayerSettingsCollection GetPlayerSettings()
        {
            object ret;
            if (GlobalCache.TryGet(GlobalCacheKey.PlayerSettingsCollection, out ret))
                return ret as PlayerSettingsCollection;
            return null;
        }

        public static SkillSettingsCollection GetSkillSettings()
        {
            object ret;
            if (GlobalCache.TryGet(GlobalCacheKey.SkillSettingsCollection, out ret))
                return ret as SkillSettingsCollection;
            return null;
        }
    }
}
