using MechSquad.View;
using System.Collections.Generic;
using System.Text;

namespace MechSquad
{
	public static class AssemblyUtil
	{
		public static List<AssemblyActiveAttachmentParameterElement.Model> GetActiveAttachmentParameterDisplayInfo(Dictionary<string, float> param)
		{
			var ret = new List<AssemblyActiveAttachmentParameterElement.Model>();

			ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
			{
				Label = Texts.Get(TextForAttachment.Parameter_PowerToUnit),
				Number = param[ConstParameter.PowerToUnit].ToString(),
			});
			ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
			{
				Label = Texts.Get(TextForAttachment.Parameter_PowerToEnv),
				Number = param[ConstParameter.PowerToEnv].ToString(),
			});
			ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
			{
				Label = Texts.Get(TextForAttachment.Parameter_MaxRange),
				Number = param[ConstParameter.MaxRange].ToString(),
			});

			if (param.ContainsKey(ConstParameter.MinRange))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_MinRange),
					Number = param[ConstParameter.MinRange].ToString(),
				});

			if (param.ContainsKey(ConstParameter.ShootRate))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_ShootRate),
					Number = param[ConstParameter.ShootRate].ToString(),
				});

			if (param.ContainsKey(ConstParameter.CartridgeCapacity))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_CartridgeCapacity),
					Number = param[ConstParameter.CartridgeCapacity].ToString(),
				});

			if (param.ContainsKey(ConstParameter.ReloadSeconds))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_ReloadSeconds),
					Number = param[ConstParameter.ReloadSeconds].ToString(),
				});

			if (param.ContainsKey(ConstParameter.WarmingUp))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_WarmingUp),
					Number = param[ConstParameter.WarmingUp].ToString(),
				});

			if (param.ContainsKey(ConstParameter.ExplosiveRadius))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_ExplosiveRadius),
					Number = param[ConstParameter.ExplosiveRadius].ToString(),
				});

			if (param.ContainsKey(ConstParameter.ArmorPenetration))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_ArmorPenetration),
					Number = param[ConstParameter.ArmorPenetration].ToString(),
				});

			if (param.ContainsKey(ConstParameter.Accuracy))
				ret.Add(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = Texts.Get(TextForAttachment.Parameter_Accuracy),
					Number = param[ConstParameter.Accuracy].ToString(),
				});

			return ret;
		}

		public static List<MechSquadShared.PatAmmo> GetAvaliableAmmosByCategory(List<string> avaliableAmmoCategory)
		{
			var ret = new List<MechSquadShared.PatAmmo>();
			var passiveAttachments = GlobalCache.GetPassiveAttachmentSettingsCollection().Settings;
			for (var i = 0; i < passiveAttachments.Count; i++)
			{
				var pa = passiveAttachments[i];
				if (pa.Category == PassiveAttachmentTypeEnum.Ammo.ToString()
					&& avaliableAmmoCategory.Contains(pa.AttachmentID))
				{
					ret.Add((MechSquadShared.PatAmmo)pa);
				}
			}
			return ret;
		}

		public static List<AssemblyMechParameterElement.Model> GetMechParametersDisplayInfo(
			MechSquadShared.VehicleStatus vehicleStatus, MechSquadShared.VehicleSettings vehicleSettings)
		{
			var ret = new List<AssemblyMechParameterElement.Model>();

			{
				var model = new AssemblyMechParameterElement.Model();
				model.Label = Texts.Get(TextForVehicle.Parameter_BodyHP);
				model.Content = vehicleSettings.BasicParameters[ConstParameter.BodyHP].ToString();

				ret.Add(model);
			}
			{
				var model = new AssemblyMechParameterElement.Model()
				{
					Label = Texts.Get(TextForVehicle.Parameter_ArmorHP),
					Content = GetDisplayContentFromModify(
								vehicleSettings.BasicParameters[ConstParameter.ArmorHP],
								GetArmorModifyList(vehicleStatus, vehicleSettings)),
				};
				ret.Add(model);
			}
			{
				var model = new AssemblyMechParameterElement.Model()
				{
					Label = Texts.Get(TextForVehicle.Parameter_MaxSpeed),
					Content = GetDisplayContentFromModify(
								vehicleSettings.BasicParameters[ConstParameter.MaxSpeed],
								GetMaxSpeedModify(vehicleStatus, vehicleSettings)),
				};
				ret.Add(model);
			}
			{
				var model = new AssemblyMechParameterElement.Model()
				{
					Label = Texts.Get(TextForVehicle.Parameter_TurnSpeed),
					Content = GetDisplayContentFromModify(
								vehicleSettings.BasicParameters[ConstParameter.TurnSpeed],
								GetTurnSpeedModify(vehicleStatus, vehicleSettings)),
				};
				ret.Add(model);
			}
			{
				var model = new AssemblyMechParameterElement.Model()
				{
					Label = Texts.Get(TextForVehicle.Parameter_Sight),
					Content = GetDisplayContentFromModify(
								vehicleSettings.BasicParameters[ConstParameter.Sight],
								GetSightModify(vehicleStatus, vehicleSettings)),
				};
				ret.Add(model);
			}

			return ret;
		}

		public static int GetSlotTotalCountWithSize(MechSquadShared.VehicleSettings settings)
		{
			var count = settings.PassiveSlotsCount;
			for (var i = 0; i < settings.ActiveSlotSettings.Count; i++)
			{
				count += settings.ActiveSlotSettings[i].MaxSize;
			}
			return count;
		}

		public static int GetUsedSlotTotalCountWithSzie(MechSquadShared.VehicleStatus status)
		{
			var count = 0;
			for (var i = 0; i < status.ActiveSlots.Count; i++)
			{
				var slot = status.ActiveSlots[i];
				if (slot.Attched)
				{
					var att = GlobalCache.GetActiveAttachmentSettingsCollection().Get(slot.AttachmentID);
					count += att.Size;
				}
			}
			for (var i = 0; i < status.PassiveSlots.Count; i++)
			{
				if (status.PassiveSlots[i].Attched)
					count++;
			}
			return count;
		}

		public static int GetCountOfSpecifiedPassiveAttachment(this MechSquadShared.VehicleStatus status, string passiveAttachmentID)
		{
			int count = 0;
			for (var i = 0; i < status.PassiveSlots.Count; i++)
			{
				var slot = status.PassiveSlots[i];
				if (slot.AttachmentID == passiveAttachmentID)
				{
					count++;
				}
			}
			return count;
		}

		public static float GetCartrigCapacity(MechSquadShared.ActiveAttachmentSettings activeAttachment)
		{
			return activeAttachment.ControlType == AttachmentControlTypeEnum.Rapid.ToString() ?
										activeAttachment.ExtraParameters[ConstParameter.CartridgeCapacity] : 1;
		}


		public static float GetTotalAmmoCount(MechSquadShared.VehicleStatus status, string attachmentID, string ammoID)
		{
			var attachment = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID);
			var ammoSettings = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(ammoID) as MechSquadShared.PatAmmo;

			return GetTotalAmmoCount(status, attachment, ammoSettings);
		}
		public static float GetTotalAmmoCount(MechSquadShared.VehicleStatus status,
										MechSquadShared.ActiveAttachmentSettings activeAttachment,
										MechSquadShared.PatAmmo ammoSettings)
		{
			var cartridgeCapacity = activeAttachment.ControlType == AttachmentControlTypeEnum.Rapid.ToString() ?
										   activeAttachment.ExtraParameters[ConstParameter.CartridgeCapacity] : 1;
			var totalAmmo = cartridgeCapacity * activeAttachment.ExtraParameters[ConstParameter.DefaultCarriedCartridgeCount];

			totalAmmo += ammoSettings.CountPerSlot *
				GetCountOfSpecifiedPassiveAttachment(status, ammoSettings.AttachmentID);
			return totalAmmo;
		}

		public static HashSet<string> GetPassiveSlotAvaliableAmmo(MechSquadShared.VehicleStatus status)
		{
			var avaliableAmmo = new HashSet<string>();
			foreach (var activeSlot in status.ActiveSlots)
			{
				if (activeSlot.Attched)
					avaliableAmmo.Add(activeSlot.AttachedAmmoID);
			}
			return avaliableAmmo;
		}

		public static AssemblyPassiveAttachmentElement.Model GetPassiveAttachmentDisplayInfoInSlot(string attachmentID)
		{
			if (attachmentID.StartsWith("Ammo"))
			{
				var ammoSettings = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(attachmentID) as MechSquadShared.PatAmmo;
				return new AssemblyPassiveAttachmentElement.Model()
				{
					AttachmentID = attachmentID,
					DisplayName = string.Format("{0} X {1}", TextForAttachment.GetNameFull(attachmentID), ammoSettings.CountPerSlot),
					IntroduceSimple = TextForAttachment.GetIntroduceSimple(attachmentID),
				};
			}

			return new AssemblyPassiveAttachmentElement.Model()
			{
				AttachmentID = attachmentID,
				DisplayName = TextForAttachment.GetNameFull(attachmentID),
				IntroduceSimple = TextForAttachment.GetIntroduceSimple(attachmentID),
			};
		}


		public struct ParameterModify
		{
			public enum ModifyTypeEnum
			{
				Absolute,
				Percent,
			}
			public ModifyTypeEnum ModifyType;


			public enum ModifySourceEnum
			{
				PassiveAttachment,
				UnloadedSlot,
			}
			public ModifySourceEnum ModifySource;
			public string SourceAttachmentID;

			public int ModifySourceCount;
			public float ModifyValue;

		}
		public static List<ParameterModify> GetArmorModifyList(MechSquadShared.VehicleStatus status, MechSquadShared.VehicleSettings settings)
		{
			var ret = new List<ParameterModify>();
			AddModifyFromAttachment(ref ret, status, PassiveAttachmentID.Up_Armor, ParameterModify.ModifyTypeEnum.Percent);
			return ret;
		}
		public static List<ParameterModify> GetMaxSpeedModify(MechSquadShared.VehicleStatus status, MechSquadShared.VehicleSettings settings)
		{
			var ret = new List<ParameterModify>();
			AddModifyFromAttachment(ref ret, status, PassiveAttachmentID.Up_MaxSpeed, ParameterModify.ModifyTypeEnum.Percent);
			AddModifyFromEmptySlot(ref ret, status, settings, 0.2f);
			return ret;
		}
		public static List<ParameterModify> GetTurnSpeedModify(MechSquadShared.VehicleStatus status, MechSquadShared.VehicleSettings settings)
		{
			var ret = new List<ParameterModify>();
			AddModifyFromAttachment(ref ret, status, PassiveAttachmentID.Up_TurnSpeed, ParameterModify.ModifyTypeEnum.Percent);
			AddModifyFromEmptySlot(ref ret, status, settings, 0.3f);
			return ret;
		}
		public static List<ParameterModify> GetSightModify(MechSquadShared.VehicleStatus status, MechSquadShared.VehicleSettings settings)
		{
			var ret = new List<ParameterModify>();
			AddModifyFromAttachment(ref ret, status, PassiveAttachmentID.Up_Sight, ParameterModify.ModifyTypeEnum.Absolute);
			return ret;
		}

		static void AddModifyFromAttachment(ref List<ParameterModify> list,
					MechSquadShared.VehicleStatus status, string attachmentID, ParameterModify.ModifyTypeEnum modifyType)
		{
			var count = GetCountOfSpecifiedPassiveAttachment(status, attachmentID);
			if(count > 0)
			{
				var upAttachment = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(attachmentID) as MechSquadShared.PatParameterUp;
				list.Add(new ParameterModify()
				{
					ModifyType = modifyType,
					ModifySource = ParameterModify.ModifySourceEnum.PassiveAttachment,
					SourceAttachmentID = upAttachment.AttachmentID,
					ModifySourceCount = count,
					ModifyValue = upAttachment.UpValue * count,
				});
			}
		}
		static void AddModifyFromEmptySlot(ref List<ParameterModify> list, 
					MechSquadShared.VehicleStatus status, MechSquadShared.VehicleSettings settings, float upValueWhenAllEmpty)
		{
			var totalCount = GetSlotTotalCountWithSize(settings);
			var usedCount = GetUsedSlotTotalCountWithSzie(status);
			var emptyCount = totalCount - usedCount;
			if(emptyCount > 0)
			{
				list.Add(new ParameterModify()
				{
					ModifyType = ParameterModify.ModifyTypeEnum.Percent,
					ModifySource = ParameterModify.ModifySourceEnum.UnloadedSlot,
					ModifySourceCount = emptyCount,
					ModifyValue = upValueWhenAllEmpty * emptyCount / totalCount,
				});
			}
		}

		public static float GetFinalValueFromModifies(float basicValue, List<ParameterModify> modifyList)
		{
			var totalPlus = 0f;
			var totalPercent = 0f;
			for (var i = 0; i < modifyList.Count; i++)
			{
				var modify = modifyList[i];
				if (modify.ModifyType == ParameterModify.ModifyTypeEnum.Absolute)
					totalPlus += modify.ModifyValue;
				else
					totalPercent += modify.ModifyValue;
			}
			var finalValue = basicValue * (1 + totalPercent) + totalPlus;

			return finalValue;
		}

		public static string GetDisplayContentFromModify(float basicValue, List<ParameterModify> modifyList)
		{
			var content = new StringBuilder();

			for (var i = 0; i < modifyList.Count; i++)
			{
				var modify = modifyList[i];
				var sourceStr = modify.ModifySource == ParameterModify.ModifySourceEnum.PassiveAttachment ?
					TextForAttachment.GetNameFull(modify.SourceAttachmentID) : Texts.Get(TextForAssemblyView.NotLoadedSlot);

				if(modify.ModifyType == ParameterModify.ModifyTypeEnum.Absolute)
					content.Append(string.Format("\n    {0} X {1} +{2}", sourceStr, modify.ModifySourceCount, modify.ModifyValue));
				else
					content.Append(string.Format("\n    {0} X {1} +{2:G3}%", sourceStr, modify.ModifySourceCount, modify.ModifyValue * 100));

			}

			var finalValue = GetFinalValueFromModifies(basicValue, modifyList);

			if (modifyList.Count == 0)
				content.Insert(0, basicValue);
			else
				content.Insert(0, string.Format("{0}   ({1} {2})", finalValue, Texts.Get(TextForAssemblyView.BasicValue), basicValue));

			return content.ToString();
		}

		public static int GetOrderIndexFromSlotID(string slotID)
		{
			switch (slotID)
			{
				case SlotID.LHand:
					return 1;
				case SlotID.RHand:
					return 2;
				case SlotID.LShoulder:
					return 3;
				case SlotID.RShoulder:
					return 4;
			}
			return 0;
		}
	}
}