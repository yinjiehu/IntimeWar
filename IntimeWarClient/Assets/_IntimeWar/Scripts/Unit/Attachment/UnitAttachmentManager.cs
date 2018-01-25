using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using IntimeWar;
using IntimeWar.View;

namespace YJH.Unit
{
    public class UnitAttachmentManager : Ability
    {
        Dictionary<int, UnitAttachment> _inputBtnToActiveAttachment = new Dictionary<int, UnitAttachment>();

        public override void Init()
        {
            base.Init();
            InitActiveAttachment();
        }

        void InitActiveAttachment()
        {
            var skills = _unit.InitialParameter.GetSkills();

            foreach(var skill in skills)
            {
                var attachmentPrefab = SkillPrefabCollection.Get().Get(skill.Value);
                var attachmentGo = Instantiate(attachmentPrefab);
                attachmentGo.transform.SetParent(_unit.transform);
                attachmentGo.transform.localPosition = Vector3.zero;
                attachmentGo.transform.localRotation = Quaternion.identity;
                attachmentGo.transform.localScale = attachmentPrefab.transform.localScale;
                UnitAttachment attachment = attachmentGo.GetComponent<UnitAttachment>();
                attachment.SkillID = skill.Value;
                attachment.SlotID = skill.Key.ToString();

                _unit.AddAbilityToListOnInit(attachment);
                _inputBtnToActiveAttachment.Add(skill.Key, attachment);
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
    }
}