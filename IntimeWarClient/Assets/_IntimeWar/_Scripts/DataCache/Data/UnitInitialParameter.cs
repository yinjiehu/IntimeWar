using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MechSquad
{
	public class UnitInitialParameter
	{
		//public struct ActiveSlotInfo
		//{
		//	public string AttachmentID;
		//	public string AttachmentBattlePrefabName;
		//	public string AttachmentAmmoID;

		//	public string[] ToStringArray()
		//	{
		//		return new string[] { AttachmentID, AttachmentBattlePrefabName, AttachmentAmmoID };
		//	}
		//	public static ActiveSlotInfo FromStringArray(string[] array)
		//	{
		//		var t = new ActiveSlotInfo()
		//		{
		//			AttachmentID = array[0],
		//			AttachmentBattlePrefabName = array[1],
		//			AttachmentAmmoID = array[2],
		//		};
		//		return t;
		//	}
		//}

		Dictionary<string, float> _basicParameters = new Dictionary<string, float>();
		Dictionary<string, string> _activeSlotAttachments = new Dictionary<string, string>();
		Dictionary<string, string> _activeSlotAttachedAmmos = new Dictionary<string, string>();
		Dictionary<byte, string> _passiveSlotAttachments = new Dictionary<byte, string>();

		public float GetParameter(string type)
		{
			float v;
			if (_basicParameters.TryGetValue(type, out v))
			{
				return v;
			}
			return 0;
		}
		public void SetParameter(string type, float value)
		{
			if (_basicParameters.ContainsKey(type))
			{
				_basicParameters[type] = value;
			}
			else
			{
				_basicParameters.Add(type, value);
			}
		}

		public Dictionary<string, string> GetActiveSlotAttachments()
		{
			return _activeSlotAttachments;
		}
		public Dictionary<string, string> GetActiveSlotAttachedAmmo()
		{
			return _activeSlotAttachedAmmos;
		}
		public Dictionary<byte, string> GetPassiveSlotAttachments()
		{
			return _passiveSlotAttachments;
		}

		public static UnitInitialParameter Create(MechSquadShared.PlayerStatus player)
		{
			var ret = new UnitInitialParameter();

			var vehicleStatus = player.GetCurrentSelected();
			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(vehicleStatus.VehicleID);

			ret._basicParameters.Add(ConstParameter.BodyHP, vehicleSettings.BasicParameters[ConstParameter.BodyHP]);
			ret._basicParameters.Add(ConstParameter.ArmorHP, AssemblyUtil.GetFinalValueFromModifies(
				vehicleSettings.BasicParameters[ConstParameter.ArmorHP], AssemblyUtil.GetArmorModifyList(vehicleStatus, vehicleSettings)));
			ret._basicParameters.Add(ConstParameter.MaxSpeed, AssemblyUtil.GetFinalValueFromModifies(
				vehicleSettings.BasicParameters[ConstParameter.MaxSpeed], AssemblyUtil.GetMaxSpeedModify(vehicleStatus, vehicleSettings)));
			ret._basicParameters.Add(ConstParameter.TurnSpeed, AssemblyUtil.GetFinalValueFromModifies(
				vehicleSettings.BasicParameters[ConstParameter.TurnSpeed], AssemblyUtil.GetTurnSpeedModify(vehicleStatus, vehicleSettings)));
			ret._basicParameters.Add(ConstParameter.Sight, AssemblyUtil.GetFinalValueFromModifies(
				vehicleSettings.BasicParameters[ConstParameter.Sight], AssemblyUtil.GetSightModify(vehicleStatus, vehicleSettings)));

			if (vehicleStatus.ActiveSlots != null)
				foreach (var slot in player.GetCurrentSelected().ActiveSlots)
				{
					ret._activeSlotAttachments.Add(slot.SlotID, slot.AttachmentID);
					ret._activeSlotAttachedAmmos.Add(slot.SlotID, slot.AttachedAmmoID);
				}
			if (vehicleStatus.PassiveSlots != null)
				foreach (var slot in player.GetCurrentSelected().PassiveSlots)
				{
					ret._passiveSlotAttachments.Add(slot.SlotNo, slot.AttachmentID);
				}

			return ret;
		}

		public static UnitInitialParameter Create(string typeID)
		{
			var ret = new UnitInitialParameter();

			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(typeID);
			foreach (var slot in vehicleSettings.ActiveSlotSettings)
			{
				if (slot.FixedSlot)
					ret._activeSlotAttachments.Add(slot.SlotID, slot.FixedAttachmentID);
			}

			return ret;
		}

		public ExitGames.Client.Photon.Hashtable ToPhotonHashtable()
		{
			var ret = new ExitGames.Client.Photon.Hashtable();
			{
				var parameter = new ExitGames.Client.Photon.Hashtable();
				ret.Add(0, parameter);
				foreach (var kv in _basicParameters)
				{
					parameter.Add(kv.Key, kv.Value);
				}
			}
			{
				var activeAtt = new ExitGames.Client.Photon.Hashtable();
				ret.Add(1, activeAtt);
				foreach (var kv in _activeSlotAttachments)
				{
					activeAtt.Add(kv.Key, kv.Value);
				}
			}
			{
				var attAmmo = new ExitGames.Client.Photon.Hashtable();
				ret.Add(2, attAmmo);
				foreach (var kv in _activeSlotAttachedAmmos)
				{
					attAmmo.Add(kv.Key, kv.Value);
				}
			}
			{
				var passive = new ExitGames.Client.Photon.Hashtable();
				ret.Add(3, passive);
				foreach (var kv in _passiveSlotAttachments)
				{
					passive.Add(kv.Key, kv.Value);
				}
			}

			return ret;
		}

		public static UnitInitialParameter FromPhotonHashtable(ExitGames.Client.Photon.Hashtable table)
		{
			var ret = new UnitInitialParameter();
			{
				var parameter = table[0] as ExitGames.Client.Photon.Hashtable;
				foreach (var kv in parameter)
				{
					ret._basicParameters.Add((string)kv.Key, (float)kv.Value);
				}
			}
			{
				var activeAtt = table[1] as ExitGames.Client.Photon.Hashtable;
				foreach (var kv in activeAtt)
				{
					ret._activeSlotAttachments.Add((string)kv.Key, (string)kv.Value);
				}
			}
			{
				var ammo = table[2] as ExitGames.Client.Photon.Hashtable;
				foreach (var kv in ammo)
				{
					ret._activeSlotAttachedAmmos.Add((string)kv.Key, (string)kv.Value);
				}
			}
			{
				var pass = table[3] as ExitGames.Client.Photon.Hashtable;
				foreach (var kv in pass)
				{
					ret._passiveSlotAttachments.Add((byte)kv.Key, (string)kv.Value);
				}
			}
			return ret;
		}
	}
}
