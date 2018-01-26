using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using IntimeWar;

namespace YJH.Unit
{
    public class KeyboardMouseInput : Ability, IUnitMobilityInput, IUnitAttachmentInput
    {
        public bool Enabled { get { return enabled; } }

        Vector3? _moveDirection;
        public Vector3? NormalizedMoveDirection { get { return _moveDirection; } }


        bool _switchTargetOnAutoMode;
        public bool SwitchTargetOnAutoMode { get { return _switchTargetOnAutoMode; } }

        bool _attachmentGroupHolding;
        public bool AttachmentGroupHolding { get { return _attachmentGroupHolding; } }

        bool[] _attachmentPress = new bool[4];
        public bool[] AttachmentPress { get { return _attachmentPress; } }

        bool[] _attachmentHolding = new bool[4];
        public bool[] AttachmentHolding { get { return _attachmentHolding; } }

        bool[] _attachmentRelease = new bool[4];
        public bool[] AttachmentRelease { get { return _attachmentRelease; } }

        bool[] _attachmentClicked = new bool[4];
        public bool[] AttachmentClicked { get { return _attachmentClicked; } }

        bool _mainFireControlPress;
        public bool MainFireControlPress { get { return _mainFireControlPress; } }

        bool _mainFireControlHolding;
        public bool MainFireControlHoding { get { return _mainFireControlHolding; } }

        bool _mainFireControlRelease;
        public bool MainFireControlRelease { get { return _mainFireControlRelease; } }

        UnitAttachmentManager _attachmentManager;

        KeyboardMouseBinding _inputBinding;


        public override void Init()
        {
            base.Init();
            _attachmentManager = _unit.GetAbility<UnitAttachmentManager>();

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_unit.IsPlayerForThisClient)
                return;
            
            _inputBinding = CustomPref.Keys.KeyboardAndMouseBinding;
            _inputBinding.OnUpdate(Time.deltaTime);

            UpdateKeyboardMoving();
            UpdateAttachments();
        }

        void UpdateKeyboardMoving()
        {
            var direction = _inputBinding.NormailizedMovingDirection;
            if (direction == Vector2.zero)
            {
                _moveDirection = Vector3.zero;
            }
            else
            {
                var worldDirection = Camera.main.transform.TransformDirection(direction);
                _moveDirection = worldDirection.normalized;
            }
        }


        void UpdateAttachments()
        {
            _attachmentGroupHolding = _inputBinding.AttachmentConnection.CurrentDown;
            for (var i = 0; i < 4; i++)
            {
                var ks = _inputBinding.AttachmentButtons[i];
                if (_inputBinding.AttachmentConnection.CurrentDown)
                {
                    _attachmentHolding[i] = false;
                    _attachmentPress[i] = false;
                    _attachmentClicked[i] = ks.Press;
                    _attachmentRelease[i] = false;
                }
                else
                {
                    _attachmentHolding[i] = ks.CurrentDown;
                    _attachmentPress[i] = ks.Press;
                    _attachmentClicked[i] = false;
                    _attachmentRelease[i] = ks.Release;
                }
            }
        }

    }
}