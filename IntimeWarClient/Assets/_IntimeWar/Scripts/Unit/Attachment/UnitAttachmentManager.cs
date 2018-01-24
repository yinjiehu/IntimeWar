using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YJH.Unit
{
    public class UnitAttachmentManager : Ability
    {
        Dictionary<int, UnitAttachment> _inputBtnToActiveAttachment = new Dictionary<int, UnitAttachment>();
        Dictionary<string, float> _extraAmmos = new Dictionary<string, float>();
        Dictionary<string, float> _slotAmmos = new Dictionary<string, float>();

        public override void Init()
        {
            base.Init();
            InitPassiveAttachment();
            InitActiveAttachment();
        }

        void InitActiveAttachment()
        {
            //var slotAttachments = _unit.InitialParameter.GetActiveSlotAttachments();
            //var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(_unit.TypeID);

            //var keys = slotAttachments.Keys.ToList();
            //keys.Sort();
            //for (var i = 0; i < keys.Count; i++)
            //{
            //    var slotID = keys[i];
            //    var attachmentID = slotAttachments[slotID];

            //    var slotSettings = vehicleSettings.GetActiveSlot(slotID);

            //    UnitAttachment attachment = null;

            //    if (!string.IsNullOrEmpty(attachmentID))
            //    {
            //        //var attachmentPrefabName = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID).BattlePrefabName;
            //        var attSettings = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID);
            //        var attachmentPrefab = GlobalCache.GetAttachmentBattlePrefabs().Get(attSettings.BattlePrefabName);
            //        var attachmentGo = Instantiate(attachmentPrefab);
            //        attachmentGo.transform.SetParent(_unit.transform);
            //        attachmentGo.transform.localPosition = Vector3.zero;
            //        attachmentGo.transform.localRotation = Quaternion.identity;
            //        attachmentGo.transform.localScale = attachmentPrefab.transform.localScale;

            //        attachment = attachmentGo.GetComponent<UnitAttachment>();
            //        attachment.AttachmentID = attachmentID;
            //        attachment.AttachToSlotID = slotID;

            //        _unit.AddAbilityToListOnInit(attachment);


            //        var ammoSlotIDs = _unit.InitialParameter.GetAmmoSlotIDs();
            //        var passiveSlotCount = ammoSlotIDs.FindAll(s => s == slotID).Count;
            //        if (_slotAmmos.ContainsKey(slotID))
            //            throw new Exception("Repeat the slotid :" + slotID);

            //        _slotAmmos.Add(slotID, passiveSlotCount * attSettings.AmmoCount);
            //    }

            //    if (attachment != null)
            //    {
            //        if (attachment is ActiveAttachment)
            //        {
            //            _inputBtnToActiveAttachment.Add(slotSettings.InputNo, attachment);
            //        }
            //        else
            //        {
            //            Debug.LogErrorFormat("Try attach to active slot [{0}] but [{1}] is not active attachment");
            //        }
            //    }
            //}


            //if (EvOnAmmoStorageUpdate != null)
            //    foreach (var a in _slotAmmos)
            //    {
            //        EvOnAmmoStorageUpdate(a.Key, a.Value);
            //    }
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
            //var passiveAttachments = _unit.InitialParameter.GetPassiveSlotAttachments();
            //foreach (var kv in passiveAttachments)
            //{
            //	var attachmentID = kv.Value;
            //	if (!string.IsNullOrEmpty(attachmentID))
            //	{
            //		var attSettings = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID);
            //		var passiveAttachment = GlobalCache.GetPassiveAttachmentSettingsCollection().Get(attachmentID);
            //		if (passiveAttachment.Category == PassiveAttachmentTypeEnum.Ammo.ToString())
            //		{
            //			var count = attSettings.AmmoCount;
            //			if (_extraAmmos.ContainsKey(attachmentID))
            //				_extraAmmos[attachmentID] += count;
            //			else
            //				_extraAmmos.Add(attachmentID, count);
            //		}
            //	}
            //}

            //if (EvOnAmmoStorageUpdate != null)
            //	foreach (var a in _extraAmmos)
            //	{
            //		EvOnAmmoStorageUpdate(a.Key, a.Value);
            //	}
        }

        public Action<string, float> EvOnAmmoStorageUpdate;

        public float GetStorageAmmoCount(string slotID)
        {
            float ret;
            if (_slotAmmos.TryGetValue(slotID, out ret))
                return ret;
            return 0;
        }

        public float RequestAmmo(string slotID, float count)
        {
            if (!_slotAmmos.ContainsKey(slotID))
                return 0;

            var currentCount = _slotAmmos[slotID];
            if (currentCount > count)
            {
                _slotAmmos[slotID] = currentCount - count;
                if (EvOnAmmoStorageUpdate != null)
                    EvOnAmmoStorageUpdate(slotID, currentCount - count);
                return count;
            }
            else
            {
                _slotAmmos.Remove(slotID);
                if (EvOnAmmoStorageUpdate != null)
                    EvOnAmmoStorageUpdate(slotID, 0);
                return currentCount;
            }
        }
        public void ReturnAmmo(string slotID, float count)
        {
            if (_slotAmmos.ContainsKey(slotID))
                _slotAmmos[slotID] += count;
            else
                _slotAmmos.Add(slotID, count);

            if (EvOnAmmoStorageUpdate != null)
                EvOnAmmoStorageUpdate(slotID, _slotAmmos[slotID]);
        }
    }
}