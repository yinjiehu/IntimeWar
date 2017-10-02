using MechSquadShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechSquad
{
	public class DemoPlayerStatus
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitPlayerStatus()
		{
			var player = new PlayerStatus()
			{
				CurrentSelectedID = VehicleID.Mech_02_Madcat,
				//	Garage = new List<VehicleStatus>()
				//	{
				//		new VehicleStatus()
				//		{
				//			VehicleID = VehicleID.Mech_02_Madcat,
				//			ActiveSlots = new List<ActiveSlotStatus>()
				//			{
				//				new ActiveSlotStatus()
				//				{
				//					VehicleID = VehicleID.Mech_02_Madcat,
				//					SlotID = SlotID.LHand,
				//					AttachmentID = ActiveAttachmentID.AT_SG01_S2_SPAS28,
				//					AttachedAmmoID = PassiveAttachmentID.Ammo_Slug_28,
				//				},
				//				new ActiveSlotStatus()
				//				{
				//					VehicleID = VehicleID.Mech_02_Madcat,
				//					SlotID = SlotID.RHand,
				//					AttachmentID = ActiveAttachmentID.AT_AC02_R2_M270,
				//					AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_45,
				//				},
				//				new ActiveSlotStatus()
				//				{
				//					VehicleID = VehicleID.Mech_02_Madcat,
				//					SlotID = SlotID.LShoulder,
				//					AttachmentID = ActiveAttachmentID.AT_CN01_S3_CP105,
				//					AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_105AP,
				//				},
				//				new ActiveSlotStatus()
				//				{
				//					VehicleID = VehicleID.Mech_02_Madcat,
				//					SlotID = SlotID.RShoulder,
				//					AttachmentID = ActiveAttachmentID.AT_MG01_R3_RK97,
				//					AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_20,
				//				},
				//			},
				//			PassiveSlots = new List<PassiveSlostStatus>()
				//			{
				//				new PassiveSlostStatus()
				//				{
				//					VehicleID = VehicleID.Mech_02_Madcat,
				//					SlotNo = 0,
				//					AttachmentID = PassiveAttachmentID.Ammo_Shell_45,
				//				},
				//				new PassiveSlostStatus()
				//				{
				//					VehicleID = VehicleID.Mech_02_Madcat,
				//					SlotNo = 1,
				//					AttachmentID = PassiveAttachmentID.Ammo_Shell_20,
				//				}
				//			}
				//		},
				//	}
				//};
				Garage = new List<VehicleStatus>()
				{
					new VehicleStatus()
					{
						VehicleID = VehicleID.Mech_02_Madcat,
						ActiveSlots = new List<ActiveSlotStatus>()
						{
							new ActiveSlotStatus()
							{
								VehicleID = VehicleID.Mech_02_Madcat,
								SlotID = SlotID.LHand,
								AttachmentID = ActiveAttachmentID.AT_AC10_R2_LAS19,
								AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_45,
							},
							new ActiveSlotStatus()
							{
								VehicleID = VehicleID.Mech_02_Madcat,
								SlotID = SlotID.RHand,
								AttachmentID = ActiveAttachmentID.AT_AC02_R2_M270,
								AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_45,
							},
							new ActiveSlotStatus()
							{
								VehicleID = VehicleID.Mech_02_Madcat,
								SlotID = SlotID.LShoulder,
								AttachmentID = ActiveAttachmentID.AT_AC02_R2_M270,
								AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_45,
							},
							new ActiveSlotStatus()
							{
								VehicleID = VehicleID.Mech_02_Madcat,
								SlotID = SlotID.RShoulder,
								AttachmentID = ActiveAttachmentID.AT_AC02_R2_M270,
								AttachedAmmoID = PassiveAttachmentID.Ammo_Shell_45,
							},
						},
						PassiveSlots = new List<PassiveSlostStatus>()
						{
							new PassiveSlostStatus()
							{
								VehicleID = VehicleID.Mech_02_Madcat,
								SlotNo = 0,
								AttachmentID = PassiveAttachmentID.Ammo_Shell_45,
							},
							new PassiveSlostStatus()
							{
								VehicleID = VehicleID.Mech_02_Madcat,
								SlotNo = 1,
								AttachmentID = PassiveAttachmentID.Ammo_Shell_20,
							}
						}
					},
				}
			};
			GlobalCache.Set(GlobalCacheKey.PlayerStatus, player);
		}
		
		public static void LoadPlayerPrefOrInit()
		{
			UnityEngine.Profiling.Profiler.BeginSample("Load Player");
			var str = UnityEngine.PlayerPrefs.GetString(GlobalCacheKey.PlayerStatus);
			if (string.IsNullOrEmpty(str))
			{
				var playerStatus = new PlayerStatus();
				playerStatus.Garage = new List<VehicleStatus>();
				foreach (var settings in GlobalCache.GetVehicleSettingsCollection().Settings)
				{
					var status = new VehicleStatus();
					status.InitWithSettings(settings);
					playerStatus.Garage.Add(status);
				}
				playerStatus.CurrentSelectedID = playerStatus.Garage[0].VehicleID;
				GlobalCache.Set(GlobalCacheKey.PlayerStatus, playerStatus);
			}
			else
			{
				//var playerStatus = JsonUtil.Deserialize<PlayerStatus>(str);
				//RemoveNotExistID(playerStatus);

				//GlobalCache.Set(GlobalCacheKey.PlayerStatus, playerStatus);
			}
			UnityEngine.Profiling.Profiler.EndSample();
		}

		static void RemoveNotExistID(PlayerStatus player)
		{
			var vehicleSettingsCollection = GlobalCache.GetVehicleSettingsCollection();
			var activeAttachmentSettingsCollection = GlobalCache.GetActiveAttachmentSettingsCollection();
			var passiveAttachmentSettingsCollection = GlobalCache.GetPassiveAttachmentSettingsCollection();

			player.Garage.RemoveAll(v => !vehicleSettingsCollection.IsExsit(v.VehicleID));

			foreach (var v in player.Garage)
			{
				var vehicleSettings = vehicleSettingsCollection.Get(v.VehicleID);
				if (v.ActiveSlots != null)
				{
					v.ActiveSlots.RemoveAll(slot => vehicleSettings.ActiveSlotSettings.All(slotSettings => slotSettings.SlotID != slot.SlotID));
					v.ActiveSlots.RemoveAll(slot => !activeAttachmentSettingsCollection.IsExsit(slot.AttachmentID));
				}
				if (v.PassiveSlots != null)
				{
					v.PassiveSlots.RemoveAll(slot => !passiveAttachmentSettingsCollection.IsExsit(slot.AttachmentID));
					while (v.PassiveSlots.Count > vehicleSettings.PassiveSlotsCount)
						v.PassiveSlots.RemoveAt(v.PassiveSlots.Count - 1);
				}
			}
		}

		public static void SaveToPlayerPref()
		{
			var p = GlobalCache.GetPlayerStatus();
			
			UnityEngine.Profiling.Profiler.BeginSample("Save Player");
			//UnityEngine.PlayerPrefs.SetString(GlobalCacheKey.PlayerStatus, JsonUtil.Serialize(p));
			UnityEngine.Profiling.Profiler.EndSample();
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("Tools/DeleteSave", priority = 65)]
		public static void ClearSave()
		{
			UnityEngine.PlayerPrefs.DeleteKey(GlobalCacheKey.PlayerStatus);
		}
#endif
	}
}
