using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace YJH.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class ReceiveAttachmentInput : FsmStateAbility
    {
        List<IUnitAttachmentInput> _inputs;

        UnitAttachmentManager _attachmentManager;

        public override void LateInit()
        {
            base.LateInit();
            _inputs = _unit.GetAllAbilities<IUnitAttachmentInput>().ToList();
            _attachmentManager = _unit.GetAbility<UnitAttachmentManager>();
            Finish();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();


            bool mainFireControlActivating = IsAnyMainFireControlHolding();
            bool mainFireControlPressDown = IsAnyMainFireControlPressDown();

            var allActiveAttachment = _attachmentManager.GetAllActiveAttachment();
            using (var itr = allActiveAttachment.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var inputNo = itr.Current.Key;
                    var attachment = itr.Current.Value;

                    var press = IsAnyInputPress(inputNo);
                    var release = IsAnyInputRelease(inputNo);
                    var holding = IsAnyInputHolding(inputNo);
                    var click = IsAnyInputClick(inputNo);


                    
                }
            }
        }

        bool IsAnyMainFireControlPressDown()
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                    ret = ret | _inputs[i].MainFireControlPress;
            }
            return ret;
        }
        bool IsAnyMainFireControlHolding()
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                    ret = ret | _inputs[i].MainFireControlHoding;
            }
            return ret;
        }
        bool IsAnyInputHolding(int no)
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                    ret = ret | _inputs[i].AttachmentHolding[no];
            }
            return ret;
        }
        bool IsAnyInputPress(int no)
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                    ret = ret | _inputs[i].AttachmentPress[no];
            }
            return ret;
        }

        bool IsAnyInputRelease(int no)
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                    ret = ret | _inputs[i].AttachmentRelease[no];
            }
            return ret;
        }

        bool IsAnyAttachmentPress()
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                {
                    for (int j = 0; j < _inputs[i].AttachmentPress.Length; j++)
                    {
                        ret = ret | _inputs[i].AttachmentPress[j];
                    }
                }
            }
            return ret;
        }

        bool IsAnyInputClick(int no)
        {
            bool ret = false;
            for (var i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Enabled)
                    ret = ret | (_inputs[i].AttachmentClicked[no]);
            }
            return ret;
        }

    }
}