using MechSquadShared;
using System.Collections.Generic;

namespace MechSquad
{
	public class DemoActiveAttachmentSettings
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitAttachmentSettings()
		{
			var att = new ActiveAttachmentSettingsCollection()
			{
				Settings = new List<ActiveAttachmentSettings>()
				{
					//new ActiveAttachmentSettings()
					//{
					//	AttachmentID = ActiveAttachmentID.AT_AC01_R2_ACR09,
					//	GaragePrefabName = ActiveAttachmentID.AT_AC01_R2_ACR09,
					//	BattlePrefabName = ActiveAttachmentID.AT_AC01_R2_ACR09,

					//	ControlType = AttachmentControlTypeEnum.Rapid.ToString(),
					//	PurposeType = AttachmentPurposeTypeEnum.Shell.ToString(),
					//	Category = AttachmentCategoryTypeEnum.AC.ToString(),
					//	Size = 2,

					//	AvaliableAmmoCategories = new List<string>() { AmmoCategory.Shell_S },
						
					//	ExtraParameters = new Dictionary<string, float>()
					//	{
					//		{ConstParameter.PowerToUnit, 20 },
					//		{ConstParameter.PowerToEnv, 0 },
					//		{ConstParameter.ShootRate, 9f },
					//		{ConstParameter.CartridgeCapacity, 30 },
					//		{ConstParameter.DefaultCarriedCartridgeCount, 5 },
					//		{ConstParameter.ReloadSeconds, 1.2f },
					//		{ConstParameter.MaxRange, 80 },
					//		{ConstParameter.Accuracy, 99 },
					//	}
					//},

					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_AC02_R2_M270,
						GaragePrefabName = ActiveAttachmentID.AT_AC02_R2_M270,
						BattlePrefabName = ActiveAttachmentID.AT_AC02_R2_M270,

						ControlType = AttachmentControlTypeEnum.Rapid.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.AutoCanon.ToString(),

						Size = 2,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Shell_45 },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 40 },
							{ConstParameter.PowerToEnv, 4 },
							{ConstParameter.ShootRate, 6f },
							{ConstParameter.CartridgeCapacity, 30 },
							{ConstParameter.DefaultCarriedCartridgeCount, 5 },
							{ConstParameter.ReloadSeconds, 2f },
							{ConstParameter.MaxRange, 90 },
							{ConstParameter.WarmingUp, 0.5f },
							{ConstParameter.Accuracy, 80 },
						}
					},

					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_SG01_S2_SPAS28,
						GaragePrefabName = ActiveAttachmentID.AT_SG01_S2_SPAS28,
						BattlePrefabName = ActiveAttachmentID.AT_SG01_S2_SPAS28,

						ControlType = AttachmentControlTypeEnum.Rapid.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Shotgun.ToString(),
						Size = 2,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Slug_28 },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 15 },
							{ConstParameter.PowerToEnv, 2 },
							{ConstParameter.ShootRate, 4f },
							{ConstParameter.CartridgeCapacity, 4 },
							{ConstParameter.DefaultCarriedCartridgeCount, 6 },
							{ConstParameter.ReloadSeconds, 2.5f },
							{ConstParameter.MaxRange, 50 },
							{ConstParameter.ScatterCount, 12 },
							{ConstParameter.Accuracy, 25 },
						}
					},

					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_MG01_R3_RK97,
						GaragePrefabName = ActiveAttachmentID.AT_MG01_R3_RK97,
						BattlePrefabName = ActiveAttachmentID.AT_MG01_R3_RK97,

						ControlType = AttachmentControlTypeEnum.Rapid.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.MachineGun.ToString(),
						Size = 3,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Shell_20 },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 22 },
							{ConstParameter.PowerToEnv, 2 },
							{ConstParameter.ShootRate, 15 },
							{ConstParameter.CartridgeCapacity, 150 },
							{ConstParameter.DefaultCarriedCartridgeCount, 1 },
							{ConstParameter.ReloadSeconds, 6f },
							{ConstParameter.MaxRange, 100 },
							{ConstParameter.WarmingUp, 0.55f },
							{ConstParameter.Accuracy, 50 },
						}
					},
					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_CN01_S3_CP105,
						GaragePrefabName = ActiveAttachmentID.AT_CN01_S3_CP105,
						BattlePrefabName = ActiveAttachmentID.AT_CN01_S3_CP105,

						ControlType = AttachmentControlTypeEnum.Single.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Canon.ToString(),
						Size = 3,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Shell_105AP },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 900 },
							{ConstParameter.PowerToEnv, 450 },
							{ConstParameter.DefaultCarriedCartridgeCount, 12 },
							{ConstParameter.ReloadSeconds, 4f },
							{ConstParameter.MaxRange, 110 },
							{ConstParameter.Accuracy, 90 },
						}
					},
					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_CN02_S3_CH105,
						GaragePrefabName = ActiveAttachmentID.AT_CN01_S3_CP105,
						BattlePrefabName = ActiveAttachmentID.AT_CN01_S3_CP105,

						ControlType = AttachmentControlTypeEnum.Single.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Canon.ToString(),
						Size = 3,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Shell_105HEAT },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 1100 },
							{ConstParameter.PowerToEnv, 100 },
							{ConstParameter.DefaultCarriedCartridgeCount, 12 },
							{ConstParameter.ReloadSeconds, 4f },
							{ConstParameter.MaxRange, 100 },
							{ConstParameter.Accuracy, 90 },
						}
					},
					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_CN30_S4_CP185,
						GaragePrefabName = ActiveAttachmentID.AT_CN01_S3_CP105,
						BattlePrefabName = ActiveAttachmentID.AT_CN01_S3_CP105,

						ControlType = AttachmentControlTypeEnum.Single.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Canon.ToString(),
						Size = 4,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Shell_185AP },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 1800 },
							{ConstParameter.PowerToEnv, 700 },
							{ConstParameter.DefaultCarriedCartridgeCount, 10 },
							{ConstParameter.ReloadSeconds, 6f },
							{ConstParameter.MaxRange, 150 },
							{ConstParameter.Accuracy, 90 },
						}
					},

					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_GRL01_G2_SS39,
						GaragePrefabName = ActiveAttachmentID.AT_GRL01_G2_SS39,
						BattlePrefabName = ActiveAttachmentID.AT_GRL01_G2_SS39,

						ControlType = AttachmentControlTypeEnum.Grenade.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Mortar.ToString(),
						Size = 2,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Grenade_67 },

						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 380 },
							{ConstParameter.PowerToEnv, 300 },
							{ConstParameter.DefaultCarriedCartridgeCount, 10 },
							{ConstParameter.ReloadSeconds, 4f },
							{ConstParameter.MaxRange, 80 },
							{ConstParameter.ExplosiveRadius, 10f },
							{ConstParameter.MinRange, 35f },
						}
					},

					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_GRL02_G3_SS51,
						GaragePrefabName = ActiveAttachmentID.AT_GRL01_G2_SS39,
						BattlePrefabName = ActiveAttachmentID.AT_GRL01_G2_SS39,

						ControlType = AttachmentControlTypeEnum.Grenade.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Mortar.ToString(),
						Size = 3,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Grenade_100 },
						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 650 },
							{ConstParameter.PowerToEnv, 650 },
							{ConstParameter.DefaultCarriedCartridgeCount, 14 },
							{ConstParameter.ReloadSeconds, 4f },
							{ConstParameter.MaxRange, 135 },
							{ConstParameter.ExplosiveRadius, 20f },
							{ConstParameter.MinRange, 50f },
						}
					},

					new ActiveAttachmentSettings()
					{
						AttachmentID = ActiveAttachmentID.AT_AC10_R2_LAS19,
						GaragePrefabName = ActiveAttachmentID.AT_AC10_R2_LAS19,
						BattlePrefabName = ActiveAttachmentID.AT_AC10_R2_LAS19,

						ControlType = AttachmentControlTypeEnum.Rapid.ToString(),
						PurposeType = AttachmentPurposeTypeEnum.Ballistic.ToString(),
						Category = AttachmentCategoryTypeEnum.Laser.ToString(),
						Size = 2,

						AvaliableAmmos = new List<string>() { PassiveAttachmentID.Ammo_Shell_45 },
						ExtraParameters = new Dictionary<string, float>()
						{
							{ConstParameter.PowerToUnit, 40 },
							{ConstParameter.PowerToEnv, 4 },
							{ConstParameter.ShootRate, 6f },
							{ConstParameter.CartridgeCapacity, 30 },
							{ConstParameter.DefaultCarriedCartridgeCount, 5 },
							{ConstParameter.ReloadSeconds, 2f },
							{ConstParameter.MaxRange, 90 },
							{ConstParameter.WarmingUp, 0.5f },
							{ConstParameter.Accuracy, 80 },
						}
					},
				}
			};

			GlobalCache.Set(GlobalCacheKey.ActiveAttachmentSettingsCollection, att);
		}
	}
}
