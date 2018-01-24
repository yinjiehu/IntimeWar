using System.Collections;
using UnityEngine;
using System;
using Shared.Models;

namespace IntimeWar
{
	public static partial class GlobalCacheKey
	{
		public const string PlayerStatus = "PlayerStatus";
        public const string CustomPref = "CustomPref";
        public const string Control = "Control";
        public const string Graphic = "Graphic";
        public const string Keys = "Keys";
        public const string VehicleSettingsCollection = "VehicleSettingsCollection";

		public const string ActiveAttachmentSettingsCollection = "ActiveAttachmentSettingsCollection";
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
    }
}
