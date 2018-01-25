using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using YJH.Unit;

namespace IntimeWar.View
{
    public class AttachmentBtn : MonoBehaviour
    {
        [SerializeField]
        Image _reloadImage;

        [SerializeField]
        int _inputNo;

        AbstractSkill _attachment;

        public void InitAttachment()
        {
            var unit = UnitManager.ThisClientPlayerUnit;
            if (unit == null)
                return;

            var attManager = unit.GetAbility<UnitAttachmentManager>();
            _attachment = attManager.GetAttachmentByInputNo(_inputNo) as AbstractSkill;
            UpdateAttachmentState();
        }

        private void Update()
        {
            if (_attachment != null)
            {
                UpdateAttachmentState();
            }
            else
            {
                InitAttachment();
            }
        }

        public void UpdateAttachmentState()
        {
            if (_attachment.IsReloading)
            {
                _reloadImage.fillAmount = 1 - _attachment.ReloadingCompleteRate;
            } else
            {
                _reloadImage.fillAmount = 0;
            }
            
        }
    }
}
