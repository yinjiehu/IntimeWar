using MechSquadShared;
using System.Collections.Generic;

namespace MechSquad
{
	public class DemoPassiveAttachmentSettings
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitAttachmentSettings()
		{
			var att = new PassiveAttachmentSettingsCollection()
			{
				Settings = new List<PassiveAttachmentSettings>()
				{
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Shell_20,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 150,
					},
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Shell_45,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 120,
					},
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Shell_105AP,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 12,
					},
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Shell_105HEAT,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 12,
					},
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Shell_185AP,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 8,
					},
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Slug_28,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 30,
					},
					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Grenade_67,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 8,
					},

					new PatAmmo()
					{
						AttachmentID = PassiveAttachmentID.Ammo_Grenade_100,
						Category = PassiveAttachmentTypeEnum.Ammo.ToString(),
						CountPerSlot = 10,
					},


					new PatParameterUp()
					{
						AttachmentID = PassiveAttachmentID.Up_Armor,
						Category = PassiveAttachmentTypeEnum.ParameterUp.ToString(),
						UpValue = 0.08f,
					},
					new PatParameterUp()
					{
						AttachmentID = PassiveAttachmentID.Up_MaxSpeed,
						Category = PassiveAttachmentTypeEnum.ParameterUp.ToString(),
						UpValue = 0.05f,
					},
					new PatParameterUp()
					{
						AttachmentID = PassiveAttachmentID.Up_TurnSpeed,
						Category = PassiveAttachmentTypeEnum.ParameterUp.ToString(),
						UpValue = 0.08f,
					},
					new PatParameterUp()
					{
						AttachmentID = PassiveAttachmentID.Up_Sight,
						Category = PassiveAttachmentTypeEnum.ParameterUp.ToString(),
						UpValue = 5,
					},
				}
			};

			GlobalCache.Set(GlobalCacheKey.PassiveAttachmentSettingsCollection, att);
		}
	}
}
