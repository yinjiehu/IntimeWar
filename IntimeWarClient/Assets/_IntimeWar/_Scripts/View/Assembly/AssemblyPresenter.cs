
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MechSquad.View
{
	public class AssemblyPresenter
	{
		public static string SelectedVehicleID { set; get; }

		public static string SelectedActiveSlotID { set; get; }
		public static string SelectedActiveAttachmentID { set; get; }
		public static string SelectedAmmoID { set; get; }

		public static byte SelectedPassiveSlotNo { set; get; }
		public static string SelectedPassiveAttachmentID { set; get; }

		static List<string> purposeTypeList = new List<string>() {
							AttachmentPurposeTypeEnum.Energy.ToString(), AttachmentPurposeTypeEnum.Ballistic.ToString(),
							AttachmentPurposeTypeEnum.Missile.ToString(), AttachmentPurposeTypeEnum.Medic.ToString(),
							AttachmentPurposeTypeEnum.Support.ToString(), AttachmentPurposeTypeEnum.Motion.ToString()};

		
		public static void ShowMechDesignAction()
		{
			var selectedVehicleID = SelectedVehicleID;

			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(selectedVehicleID);

			var player = GlobalCache.GetPlayerStatus();
			if (!player.IsVehicleExist(selectedVehicleID))
			{
				var vehilce = new MechSquadShared.VehicleStatus();
				vehilce.InitWithSettings(vehicleSettings);
				player.Garage.Add(vehilce);
			}
			var vehicleStatus = GlobalCache.GetPlayerStatus().GetVehicle(selectedVehicleID);

			var data = new AssemblyMechDesignSubView.Model()
			{
				VehicleID = selectedVehicleID,
				ActiveSlots = new List<AssemblyMechDesignActiveSlotElement.Model>(),
				PassiveSlot = new List<string>(),
				//PassiveSlotRight = new List<string>(),
			};

			foreach (var activeSlotSettings in vehicleSettings.ActiveSlotSettings)
			{
				var slotModel = new AssemblyMechDesignActiveSlotElement.Model();
				slotModel.SlotID = activeSlotSettings.SlotID;
				if (activeSlotSettings.MaxSize <= 0)
				{
					slotModel.Show = false;
					slotModel.Attachable = false;
					slotModel.Attached = false;
					//slotModel.EmptyDescription = Texts.Get(TextForAssemblyView.SlotDisabled);
					slotModel.ShowSizeAndAvaliablePurpose = false;
				}
				else
				{
					slotModel.Show = true;
					slotModel.Attachable = true;

					slotModel.SlotName = GetSlotNameById(activeSlotSettings.SlotID);
					slotModel.Size = activeSlotSettings.MaxSize;

					var activeSlotStatus = vehicleStatus.GetActiveSlot(activeSlotSettings.SlotID);
					if (activeSlotStatus == null || !activeSlotStatus.Attched)
					{
						slotModel.Attached = false;
						//slotModel.EmptyDescription = Texts.Get(TextForAssemblyView.ClickSlot);
						slotModel.ShowSizeAndAvaliablePurpose = true;
						slotModel.AvaliablePurposeList = new List<bool>();

						foreach (var p in purposeTypeList)
						{
							if(activeSlotSettings.AvaliableAttachmentPurpose.Contains(p))
								slotModel.AvaliablePurposeList.Add(true);
							else
								slotModel.AvaliablePurposeList.Add(false);
						}
					}
					else
					{
						slotModel.Attached = true;
						slotModel.AttachmentFullName = TextForAttachment.GetNameFull(activeSlotStatus.AttachmentID);
						slotModel.LoadedAmmoName = TextForAttachment.GetNameFull(activeSlotStatus.AttachedAmmoID);
						var attachment = GlobalCache.GetActiveAttachmentSettingsCollection().Get(activeSlotStatus.AttachmentID);
						var ammoSettings = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(activeSlotStatus.AttachedAmmoID) as MechSquadShared.PatAmmo;
						slotModel.AttachmentIcon = GlobalCache.GetIcon().GetAttCategoryIcon(attachment.Category);

						slotModel.LoadedAmmoTotalCount = string.Format("{0} / {1}",
									AssemblyUtil.GetCartrigCapacity(attachment),
									AssemblyUtil.GetTotalAmmoCount(vehicleStatus, attachment, ammoSettings));
						slotModel.AttachementSize = attachment.Size;
					}
				}

				data.ActiveSlots.Add(slotModel);
			}

			for (byte i = 0; i < vehicleSettings.PassiveSlotsCount; i++)
			{
				var status = vehicleStatus.GetPassiveSlot(i);
				var displayStr = "";
				if (status != null && status.Attched)
				{
					var passiveAttSettings = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(status.AttachmentID);
					if (passiveAttSettings.Category == PassiveAttachmentTypeEnum.Ammo.ToString())
					{
						displayStr = string.Format("{0} X {1}",
							TextForAttachment.GetNameFull(status.AttachmentID),
							((MechSquadShared.PatAmmo)passiveAttSettings).CountPerSlot);
					}
					else
					{
						displayStr = TextForAttachment.GetNameFull(status.AttachmentID); 
					}
				}

				data.PassiveSlot.Add(displayStr);
				data.vehicleImage = GlobalCache.GetIcon().GetVehicleImage(vehicleSettings.IconPath);

				//if (i % 2 == 0)
				//{
				//	data.PassiveSlotLeft.Add(displayStr);
				//}
				//else
				//{
				//	data.PassiveSlotRight.Add(displayStr);
				//}
			}

			ViewManager.Instance.GetView<AssemblyMechDesignSubView>().BindDataToView(data);
		}

		public static void ShowMechDetailAction()
		{
			var selectedVehicleID = SelectedVehicleID;

			var vehicleStatus = GlobalCache.GetPlayerStatus().GetVehicle(selectedVehicleID);
			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(selectedVehicleID);

			var model = new AssemblyMechDetailSubView.Model()
			{
				Parameters = AssemblyUtil.GetMechParametersDisplayInfo(vehicleStatus, vehicleSettings),
				Attachments = new List<AssemblyMechLoadedAttachmentElement.Model>(),

				FullName = TextForVehicle.GetNameFull(selectedVehicleID),

				Weight = "??T",
				Category = "To do",
				SP = "To do",

				Introduce = TextForVehicle.GetIntroduce(selectedVehicleID),

				ShowStartBtn = !PhotonNetwork.connected,
				ShowCompleteBtn = PhotonNetwork.connected,
			};

			foreach (var activeSlotSettings in vehicleSettings.ActiveSlotSettings)
			{
				var slotStatus = vehicleStatus.GetActiveSlot(activeSlotSettings.SlotID);
				if (slotStatus == null || !slotStatus.Attched)
				{
					continue;
				}

				var attachment = GlobalCache.GetActiveAttachmentSettingsCollection().Get(slotStatus.AttachmentID);
				var ammoSettings = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(slotStatus.AttachedAmmoID) as MechSquadShared.PatAmmo;

				var attModel = new AssemblyMechLoadedAttachmentElement.Model()
				{
					AttachmentDisplayName = TextForAttachment.GetNameFull(slotStatus.AttachmentID),
					LoadedAmmoDisplayName = TextForAttachment.GetNameFull(slotStatus.AttachedAmmoID),
					ControlType = attachment.ControlType,
					Category = TextForAttachment.GetCategoryDesciption(attachment.Category),
					LoadedAmmoTotalCount = string.Format("{0} / {1}",
									AssemblyUtil.GetCartrigCapacity(attachment),
									AssemblyUtil.GetTotalAmmoCount(vehicleStatus, attachment, ammoSettings)),
					ParameterList = AssemblyUtil.GetActiveAttachmentParameterDisplayInfo(attachment.ExtraParameters),
				};

				model.Attachments.Add(attModel);
			}

			ViewManager.Instance.GetView<AssemblyMechDetailSubView>().BindToView(model);
		}

		public static void ShowActiveAttachmentListAction()
		{
			var slotID = SelectedActiveSlotID;
			List<AssemblyActiveAttachmentListElement.Model> attachmentList = new List<AssemblyActiveAttachmentListElement.Model>();

			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(GlobalCache.GetPlayerStatus().CurrentSelectedID);
			var slotSettings = vehicleSettings.GetActiveSlot(slotID);

			var activeAttachments = GlobalCache.GetActiveAttachmentSettingsCollection().Settings;

			foreach (var att in activeAttachments)
			{
				var elementModel = new AssemblyActiveAttachmentListElement.Model()
				{
					AttachmentID = att.AttachmentID,
					SlotID = slotID,
					AttachmentDisplayName = TextForAttachment.GetNameFull(att.AttachmentID),
					ControlType = att.ControlType,
					Category = TextForAttachment.GetCategoryDesciption(att.Category),
					ParameterList = AssemblyUtil.GetActiveAttachmentParameterDisplayInfo(att.ExtraParameters),
					AttachmentIcon = GlobalCache.GetIcon().GetAttCategoryIcon(att.Category),
					PurposeIcon = GlobalCache.GetIcon().GetAttPurposeIcon(att.PurposeType),
					SizeNum = att.Size,
					Purpose = slotSettings.AvaliableAttachmentPurpose.Contains(att.PurposeType) ? true:false,
					Size= slotSettings.MaxSize >= att.Size ? true : false,
					
					Attachable = slotSettings.MaxSize >= att.Size && slotSettings.AvaliableAttachmentPurpose.Contains(att.PurposeType)
				};

				attachmentList.Add(elementModel);
			}

			attachmentList = attachmentList.OrderBy(d => d.Attachable ? 0 : 1).ToList();

			var model = new AssemblyActiveAttachmentListSubView.Model();

			var activeSlot = GlobalCache.GetPlayerStatus().GetVehicle(SelectedVehicleID).GetActiveSlot(slotID);
			if (activeSlot != null)
			{
				model.WeaponSize = GlobalCache.GetActiveAttachmentSettingsCollection().Get(activeSlot.AttachmentID).Size;
				model.WeaponName = TextForAttachment.GetNameFull(activeSlot.AttachmentID); 
			}
			else
			{
				model.WeaponName = "";
				model.WeaponSize = 0;
			}

			model.PurposeList = new List<bool>();
			foreach (var p in purposeTypeList)
			{
				if (slotSettings.AvaliableAttachmentPurpose.Contains(p))
					model.PurposeList.Add(true);
				else
					model.PurposeList.Add(false);
			}

			model.PositionName = GetSlotNameById(slotID);
			model.SlotSize = slotSettings.MaxSize;
			model.ElementList = attachmentList;

			ViewManager.Instance.GetView<AssemblyActiveAttachmentListSubView>().BindToView(model);
		}

		static string GetSlotNameById(string slotID)
		{
			return Texts.Get(TextForAssemblyView.SlotNamePrefix + slotID);
		}


		public static void ShowAmmoListAction()
		{
			var attSettings = GlobalCache.GetActiveAttachmentSettingsCollection().Get(SelectedActiveAttachmentID);
			var ammos = AssemblyUtil.GetAvaliableAmmosByCategory(attSettings.AvaliableAmmos);

			if (ammos.Count == 0)
			{
				Debug.LogErrorFormat("avaliable ammo is empty for attacment {0}", attSettings.AttachmentID);
				return;
			}

			if (ammos.Count == 1)
			{
				SelectedAmmoID = ammos[0].AttachmentID;
				LoadActiveAttachmentAction();
				ViewManager.Instance.GetView<AssemblyView>().GetComponent<PlayMakerFSM>().SendEvent("HideAttachmentList");
				return;
			}

			var data = ammos.Select(a => new AssemblyAmmoElement.Model()
			{
				AmmoID = a.AttachmentID,
				DisplayName = TextForAttachment.GetNameFull(a.AttachmentID),
				IntroduceSimple = TextForAttachment.GetIntroduceSimple(a.AttachmentID),
			}).ToList();

			ViewManager.Instance.GetView<AssemblyAmmoListSubView>().BindToView(data);
			ViewManager.Instance.GetView<AssemblyView>().GetComponent<PlayMakerFSM>().SendEvent("ShowAmmoSelect");
		}

		public static void LoadActiveAttachmentAction()
		{
			var player = GlobalCache.GetPlayerStatus();
			var vehicleStatus = player.GetVehicle(SelectedVehicleID);
			var slot = vehicleStatus.GetActiveSlot(SelectedActiveSlotID);
			if (slot == null)
			{
				slot = new MechSquadShared.ActiveSlotStatus()
				{
					VehicleID = SelectedVehicleID,
					SlotID = SelectedActiveSlotID,
				};
				vehicleStatus.ActiveSlots.Add(slot);
			}
			slot.AttachmentID = SelectedActiveAttachmentID;
			slot.AttachedAmmoID = SelectedAmmoID;

			RemoveUnusedAmmo();

			ShowMechDesignAction();
			ShowMechDetailAction();

			DemoPlayerStatus.SaveToPlayerPref();
		}

		static void RemoveUnusedAmmo()
		{
			var p = GlobalCache.GetPlayerStatus();

			foreach (var vehicle in p.Garage)
			{
				HashSet<string> avaliableAmmos = new HashSet<string>();
				foreach (var activeSlot in vehicle.ActiveSlots)
				{
					if (activeSlot.Attched)
						avaliableAmmos.Add(activeSlot.AttachedAmmoID);
				}

				foreach (var passiveSlot in vehicle.PassiveSlots)
				{
					if (passiveSlot.Attched)
					{
						var attachmentID = passiveSlot.AttachmentID;
						var passiveAtt = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(attachmentID);
						if (passiveAtt.Category == PassiveAttachmentTypeEnum.Ammo.ToString() && !avaliableAmmos.Contains(attachmentID))
						{
							passiveSlot.AttachmentID = "";
						}
					}
				}
			}
		}
		
		public static void UnloadActiveAttachmentAction()
		{
			var player = GlobalCache.GetPlayerStatus();
			var vehicleStatus = player.GetVehicle(SelectedVehicleID);
			var slot = vehicleStatus.GetActiveSlot(SelectedActiveSlotID);
			if (slot != null)
			{
				vehicleStatus.ActiveSlots.Remove(slot);
			}

			RemoveUnusedAmmo();

			ShowMechDesignAction();
			ShowMechDetailAction();

			DemoPlayerStatus.SaveToPlayerPref();
		}

		public static void ShowPassiveAttachmentListAction()
		{
			var data = new List<AssemblyPassiveAttachmentElement.Model>();

			var vehicleStatus = GlobalCache.GetPlayerStatus().GetVehicle(SelectedVehicleID);
			data.Add(AssemblyUtil.GetPassiveAttachmentDisplayInfoInSlot(PassiveAttachmentID.Up_Armor));
			data.Add(AssemblyUtil.GetPassiveAttachmentDisplayInfoInSlot(PassiveAttachmentID.Up_MaxSpeed));
			data.Add(AssemblyUtil.GetPassiveAttachmentDisplayInfoInSlot(PassiveAttachmentID.Up_TurnSpeed));
			data.Add(AssemblyUtil.GetPassiveAttachmentDisplayInfoInSlot(PassiveAttachmentID.Up_Sight));

			var avaliableAmmos = AssemblyUtil.GetPassiveSlotAvaliableAmmo(vehicleStatus);
			foreach (var a in avaliableAmmos)
			{
				var ammoSettings = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(a) as MechSquadShared.PatAmmo;
				data.Add(AssemblyUtil.GetPassiveAttachmentDisplayInfoInSlot(a));
			}

			var view = ViewManager.Instance.Show<AssemblyPassiveAttachmentListSubView>();
			view.BindToView(data);
		}

		public static void LoadPassiveAttachmentAction()
		{
			var slotNo = SelectedPassiveSlotNo;

			var vehicleStatus = GlobalCache.GetPlayerStatus().GetVehicle(SelectedVehicleID);
			var slot = vehicleStatus.GetPassiveSlot(slotNo);
			if(slot == null)
			{
				vehicleStatus.PassiveSlots.Add(new MechSquadShared.PassiveSlostStatus()
				{
					VehicleID = SelectedVehicleID,
					SlotNo = slotNo,
					AttachmentID = SelectedPassiveAttachmentID,
				});
			}
			else
			{
				slot.AttachmentID = SelectedPassiveAttachmentID;
			}

			ViewManager.Instance.Hide<AssemblyPassiveAttachmentListSubView>();

			RemoveUnusedAmmo();

			ShowMechDesignAction();
			ShowMechDetailAction();

			DemoPlayerStatus.SaveToPlayerPref();
		}

		public static void UnloadPassiveSlot()
		{
			var slotNo = SelectedPassiveSlotNo;

			var vehicleStatus = GlobalCache.GetPlayerStatus().GetVehicle(SelectedVehicleID);
			var slot = vehicleStatus.GetPassiveSlot(slotNo);
			if (slot != null)
			{
				vehicleStatus.PassiveSlots.Remove(slot);
			}

			ViewManager.Instance.Hide<AssemblyPassiveAttachmentListSubView>();

			RemoveUnusedAmmo();

			ShowMechDesignAction();
			ShowMechDetailAction();

			DemoPlayerStatus.SaveToPlayerPref();
		}
	}
}