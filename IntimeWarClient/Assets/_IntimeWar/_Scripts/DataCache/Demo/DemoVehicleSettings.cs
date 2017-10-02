using MechSquadShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechSquad
{
	public class DemoVehicleSettings
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitAttachmentSettings()
		{
			var vehicleSettingCollection = new VehicleSettingsCollection();
			GlobalCache.Set(GlobalCacheKey.VehicleSettingsCollection, vehicleSettingCollection);

			vehicleSettingCollection.Settings = new List<VehicleSettings>()
			{
				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_01_Raven,
					GaragePrefabName = VehicleID.Mech_01_Raven,
					BattlePrefabName = VehicleID.Mech_01_Raven,
					IconPath = "Mech_01_Raven",

					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 800 },
						{ ConstParameter.ArmorHP, 1300 },
						{ ConstParameter.MaxSpeed, 23f },
						{ ConstParameter.TurnSpeed, 150 },
						{ ConstParameter.Sight, 95 },
					},

					PassiveSlotsCount = 4,

					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_01_Raven,
							SlotID = SlotID.LHand,
							MaxSize = 2,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_01_Raven,
							SlotID = SlotID.RHand,
							MaxSize = 3,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},

				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_01_Raven_M,
					GaragePrefabName = VehicleID.Mech_01_Raven,
					BattlePrefabName = VehicleID.Mech_01_Raven,
					IconPath = "Mech_01_Raven",

					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 800 },
						{ ConstParameter.ArmorHP, 1300 },
						{ ConstParameter.MaxSpeed, 26f },
						{ ConstParameter.TurnSpeed, 180 },
						{ ConstParameter.Sight, 115 },
					},

					PassiveSlotsCount = 4,

					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_01_Raven_M,
							SlotID = SlotID.LHand,
							MaxSize = 2,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_01_Raven_M,
							SlotID = SlotID.RHand,
							MaxSize = 2,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},


				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_02_Madcat.ToString(),
					GaragePrefabName = VehicleID.Mech_02_Madcat,
					BattlePrefabName = VehicleID.Mech_02_Madcat,
					IconPath = "Mech_02_Madcat",

					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 1000 },
						{ ConstParameter.ArmorHP, 2000 },
						{ ConstParameter.MaxSpeed, 19f },
						{ ConstParameter.TurnSpeed, 90 },
						{ ConstParameter.Sight, 75 },
					},

					PassiveSlotsCount = 6,

					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat,
							SlotID = SlotID.LHand,
							MaxSize = 2,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat,
							SlotID = SlotID.RHand,
							MaxSize = 3,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat,
							SlotID = SlotID.LShoulder,
							MaxSize = 3,
							InputNo = 2,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat,
							SlotID = SlotID.RShoulder,
							MaxSize = 0,
							InputNo = 3,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
					},
				},

				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_02_Madcat_M,
					GaragePrefabName = VehicleID.Mech_02_Madcat,
					BattlePrefabName = VehicleID.Mech_02_Madcat,
					IconPath = "Mech_02_Madcat",

					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 1000 },
						{ ConstParameter.ArmorHP, 1700 },
						{ ConstParameter.MaxSpeed, 21f },
						{ ConstParameter.TurnSpeed, 100 },
						{ ConstParameter.Sight, 75 },
					},

					PassiveSlotsCount = 6,

					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_M,
							SlotID = SlotID.LHand,
							MaxSize = 2,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_M,
							SlotID = SlotID.RHand,
							MaxSize = 2,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_M,
							SlotID = SlotID.LShoulder,
							MaxSize = 2,
							InputNo = 2,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_M,
							SlotID = SlotID.RShoulder,
							MaxSize = 2,
							InputNo = 3,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},


				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_02_Madcat_P,
					GaragePrefabName = VehicleID.Mech_02_Madcat,
					BattlePrefabName = VehicleID.Mech_02_Madcat,
					IconPath = "Mech_02_Madcat",
					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 1000 },
						{ ConstParameter.ArmorHP, 1600 },
						{ ConstParameter.MaxSpeed, 16f },
						{ ConstParameter.TurnSpeed, 80 },
						{ ConstParameter.Sight, 70 },
					},
					PassiveSlotsCount = 6,
					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_P,
							SlotID = SlotID.LHand,
							MaxSize = 2,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_P,
							SlotID = SlotID.RHand,
							MaxSize = 2,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_P,
							SlotID = SlotID.LShoulder,
							MaxSize = 3,
							InputNo = 2,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_02_Madcat_P,
							SlotID = SlotID.RShoulder,
							MaxSize = 3,
							InputNo = 3,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},


				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_03_Avatar,
					GaragePrefabName = VehicleID.Mech_03_Avatar,
					BattlePrefabName = VehicleID.Mech_03_Avatar,
					IconPath = "Mech_03_Avatar",
					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 1500 },
						{ ConstParameter.ArmorHP, 3500 },
						{ ConstParameter.MaxSpeed, 12f },
						{ ConstParameter.TurnSpeed, 70 },
						{ ConstParameter.Sight, 70 },
					},
					PassiveSlotsCount = 8,
					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar,
							SlotID = SlotID.LHand,
							MaxSize = 3,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar,
							SlotID = SlotID.RHand,
							MaxSize = 3,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar,
							SlotID = SlotID.RShoulder,
							MaxSize = 3,
							InputNo = 2,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},


				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_03_Avatar_P,
					GaragePrefabName = VehicleID.Mech_03_Avatar,
					BattlePrefabName = VehicleID.Mech_03_Avatar,
					IconPath = "Mech_03_Avatar",
					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 1500 },
						{ ConstParameter.ArmorHP, 3000 },
						{ ConstParameter.MaxSpeed, 11f },
						{ ConstParameter.TurnSpeed, 70 },
						{ ConstParameter.Sight, 70 },
					},
					PassiveSlotsCount = 8,
					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar_P,
							SlotID = SlotID.LHand,
							MaxSize = 3,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar_P,
							SlotID = SlotID.RHand,
							MaxSize = 3,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar_P,
							SlotID = SlotID.RShoulder,
							MaxSize = 4,
							InputNo = 2,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},


				new VehicleSettings()
				{
					VehicleID = VehicleID.Mech_03_Avatar_M,
					GaragePrefabName = VehicleID.Mech_03_Avatar,
					BattlePrefabName = VehicleID.Mech_03_Avatar,
					IconPath = "Mech_03_Avatar",
					BasicParameters = new Dictionary<string, float>()
					{
						{ ConstParameter.BodyHP, 1500 },
						{ ConstParameter.ArmorHP, 3000 },
						{ ConstParameter.MaxSpeed, 16f },
						{ ConstParameter.TurnSpeed, 100 },
						{ ConstParameter.Sight, 70 },
					},
					PassiveSlotsCount = 8,
					ActiveSlotSettings = new List<ActiveSlotSettings>()
					{
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar_M,
							SlotID = SlotID.LHand,
							MaxSize = 2,
							InputNo = 0,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar_M,
							SlotID = SlotID.RHand,
							MaxSize = 2,
							InputNo = 1,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString() },
						},
						new ActiveSlotSettings()
						{
							VehicleID = VehicleID.Mech_03_Avatar_M,
							SlotID = SlotID.RShoulder,
							MaxSize = 2,
							InputNo = 2,
							AvaliableAttachmentPurpose = new List<string>() { AttachmentPurposeTypeEnum.Ballistic.ToString(), AttachmentPurposeTypeEnum.Support.ToString() },
						},
					},
				},
			};
		}
	}
}
