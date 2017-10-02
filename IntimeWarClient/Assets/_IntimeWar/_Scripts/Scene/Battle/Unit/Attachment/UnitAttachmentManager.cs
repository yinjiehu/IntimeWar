using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class UnitAttachmentManager : Ability
	{
		Dictionary<int, UnitAttachment> _inputBtnToActiveAttachment = new Dictionary<int, UnitAttachment>();
		Dictionary<string, float> _extraAmmos = new Dictionary<string, float>();

		public override void Init()
		{
			base.Init();
			InitPassiveAttachment();
			InitActiveAttachment();
		}

		void InitActiveAttachment()
		{
			var slotAttachments = _unit.InitialParameter.GetActiveSlotAttachments();
			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(_unit.TypeID);

			var positionDefine = _unit.GetAbility<UnitPositionDefine>();

			foreach (var kv in slotAttachments)
			{
				var slotID = kv.Key;
				var attachmentID = kv.Value;

				var slotSettings = vehicleSettings.GetActiveSlot(slotID);
				var slotPosition = positionDefine.GetPosition(slotID);

				UnitAttachment attachment = null;

				if (!string.IsNullOrEmpty(attachmentID))
				{
					var positionTransform = slotPosition == null ? transform : slotPosition;

					//var attachmentPrefabName = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID).BattlePrefabName;
					var attSettings = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID);
					var attachmentPrefab = GlobalCache.GetAttachmentBattlePrefabs().Get(attSettings.BattlePrefabName);
					var attachmentGo = Instantiate(attachmentPrefab);
					attachmentGo.transform.SetParent(positionTransform);
					attachmentGo.transform.localPosition = Vector3.zero;
					attachmentGo.transform.localRotation = Quaternion.identity;
					attachmentGo.transform.localScale = attachmentPrefab.transform.localScale;

					attachment = attachmentGo.GetComponent<UnitAttachment>();
					attachment.AttachmentID = attachmentID;
					attachment.AttachToSlotID = slotID;

					_unit.AddAbilityToListOnInit(attachment);
				}

				if (attachment != null)
				{
					if (attachment is ActiveAttachment)
					{
						_inputBtnToActiveAttachment.Add(slotSettings.InputNo, attachment);
					}
					else
					{
						Debug.LogErrorFormat("Try attach to active slot [{0}] but [{1}] is not active attachment");
					}
				}
			}
		}

		public Dictionary<int, UnitAttachment> GetAllActiveAttachment()
		{
			return _inputBtnToActiveAttachment;
		}

		public UnitAttachment GetAttachmentByInputNo(int no)
		{
			UnitAttachment att;
			if (_inputBtnToActiveAttachment.TryGetValue(no, out att))
			{
				return att;
			}
			return null;
		}

		void InitPassiveAttachment()
		{
			var passiveAttachments = _unit.InitialParameter.GetPassiveSlotAttachments();
			foreach (var kv in passiveAttachments)
			{
				var attachmentID = kv.Value;
				var passiveAttachment = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(attachmentID);
				if (passiveAttachment.Category == PassiveAttachmentTypeEnum.Ammo.ToString())
				{
					var count = ((MechSquadShared.PatAmmo)passiveAttachment).CountPerSlot;
					if (_extraAmmos.ContainsKey(attachmentID))
						_extraAmmos[attachmentID] += count;
					else
						_extraAmmos.Add(attachmentID, count);
				}
			}

			if (EvOnAmmoStorageUpdate != null)
				foreach (var a in _extraAmmos)
				{
					EvOnAmmoStorageUpdate(a.Key, a.Value);
				}
		}

		public Action<string, float> EvOnAmmoStorageUpdate;

		public float GetStorageAmmoCount(string ammoAttachmentID)
		{
			float ret;
			if (_extraAmmos.TryGetValue(ammoAttachmentID, out ret))
				return ret;
			return 0;
		}

		public float RequestAmmo(string ammoAttachmentID, float count)
		{
			if (!_extraAmmos.ContainsKey(ammoAttachmentID))
				return 0;

			var currentCount = _extraAmmos[ammoAttachmentID];
			if (currentCount > count)
			{
				_extraAmmos[ammoAttachmentID] = currentCount - count;
				if (EvOnAmmoStorageUpdate != null)
					EvOnAmmoStorageUpdate(ammoAttachmentID, currentCount - count);
				return count;
			}
			else
			{
				_extraAmmos.Remove(ammoAttachmentID);
				if (EvOnAmmoStorageUpdate != null)
					EvOnAmmoStorageUpdate(ammoAttachmentID, 0);
				return currentCount;
			}
		}
		public void ReturnAmmo(string ammoAttachmentID, float count)
		{
			if (_extraAmmos.ContainsKey(ammoAttachmentID))
				_extraAmmos[ammoAttachmentID] += count;
			else
				_extraAmmos.Add(ammoAttachmentID, count);

			if (EvOnAmmoStorageUpdate != null)
				EvOnAmmoStorageUpdate(ammoAttachmentID, _extraAmmos[ammoAttachmentID]);
		}
	}
}