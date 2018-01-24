using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YJH.Unit
{
	public class AIInput : Ability, IUnitMobilityInput, IUnitAttachmentInput
	{
        public bool Enabled { get { return enabled; } }

        //mobility
        public Vector3? NormalizedMoveDirection { set; get; }

        bool _attachmentGroupHolding;
        public bool AttachmentGroupHolding { get { return _attachmentGroupHolding; } }

        bool[] _attachmentPressDown = new bool[4];
        public bool[] AttachmentPress { get { return _attachmentPressDown; } }
        bool[] _attachmentRelease = new bool[4];
        public bool[] AttachmentRelease { get { return _attachmentRelease; } }
        bool[] _attachmentsHolding = new bool[4];
        public bool[] AttachmentHolding { get { return _attachmentsHolding; } }

        bool[] _attachmentsClick = new bool[4];
        public bool[] AttachmentClicked { get { return _attachmentsClick; } }

        bool _mainFireControlPress;
        public bool MainFireControlPress { get { return _mainFireControlPress; } }
        bool _mainFireControlHolding;
        public bool MainFireControlHoding { get { return _mainFireControlHolding; } }
        bool _mainFireControlRelease;
        public bool MainFireControlRelease { get { return _mainFireControlRelease; } }

        [SerializeField]
        bool _enableAtInit;

        public override void LateInit()
        {
            base.LateInit();
            if (_enableAtInit)
                EnableAI();
            else
                DisableAI();
        }

        public void EnableAI()
        {
            var coms = GetComponents<Ability>();
            for (var i = 0; i < coms.Length; i++)
            {
                coms[i].enabled = coms[i] == this;
            }
        }

        public void DisableAI()
        {
            var coms = GetComponents<Ability>();
            for (var i = 0; i < coms.Length; i++)
            {
                coms[i].enabled = coms[i] != this;
            }
        }
    }
}