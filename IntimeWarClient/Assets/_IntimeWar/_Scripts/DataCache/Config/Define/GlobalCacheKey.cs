using System.Collections;
using UnityEngine;
using System;

namespace MechSquad
{
	public static partial class GlobalCacheKey
	{
		public const string PlayerStatus = "PlayerStatus";
		public const string VehicleSettingsCollection = "VehicleSettingsCollection";

		public const string ActiveAttachmentSettingsCollection = "ActiveAttachmentSettingsCollection";
		public const string PassiveAttachmentSettingsCollection = "PassiveAttachmentSettingsCollection";
	}

	public partial class GlobalCache
	{

		public static MechSquadShared.PlayerStatus GetPlayerStatus()
		{
			object ret;
			if (GlobalCache.TryGet(GlobalCacheKey.PlayerStatus, out ret))
				return ret as MechSquadShared.PlayerStatus;
			return null;
		}
		public static MechSquadShared.VehicleSettingsCollection GetVehicleSettingsCollection()
		{
			object ret;
			if (GlobalCache.TryGet(GlobalCacheKey.VehicleSettingsCollection, out ret))
				return ret as MechSquadShared.VehicleSettingsCollection;
			return null;
		}
		public static MechSquadShared.ActiveAttachmentSettingsCollection GetActiveAttachmentSettingsCollection()
		{
			object ret;
			if (GlobalCache.TryGet(GlobalCacheKey.ActiveAttachmentSettingsCollection, out ret))
				return ret as MechSquadShared.ActiveAttachmentSettingsCollection;
			return null;
		}
		public static MechSquadShared.PassiveAttachmentSettingsCollection GetPassiveAttachmentSettingsCollection()
		{
			object ret;
			if (GlobalCache.TryGet(GlobalCacheKey.PassiveAttachmentSettingsCollection, out ret))
				return ret as MechSquadShared.PassiveAttachmentSettingsCollection;
			return null;
		}
	}
}
