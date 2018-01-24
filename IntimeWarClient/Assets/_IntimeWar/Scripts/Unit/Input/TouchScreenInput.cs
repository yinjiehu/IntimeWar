using UnityEngine;
using System.Linq;
using Haruna.UI;
using View;
using IntimeWar.View;

namespace YJH.Unit
{
    public class TouchScreenInput : Ability, IUnitMobilityInput, IUnitAttachmentInput
    {
        [SerializeField]
        float _autoToSemiAutoDragDistance = 0.5f;

        public bool Enabled { get { return enabled; } }

        HarunaJoyStick _leftJoystick;
        HarunaJoyStick _rightJoystick;
        HarunaButton[] _activeAttachmentBtnInputs;

        #region mobility implement
        Vector3? _moveDirection;
        public Vector3? NormalizedMoveDirection { get { return _moveDirection; } }
        #endregion

        #region aiming implement
        Vector3? _aimingDirection;
        public Vector3? AimingDirection { get { return _aimingDirection; } }

        #endregion

        #region attachment implement


        bool _attachmentGroupHolding;
        public bool AttachmentGroupHolding { get { return _attachmentGroupHolding; } }

        bool[] _attachmentPress = new bool[4];
        public bool[] AttachmentPress { get { return _attachmentPress; } }
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
        #endregion


        public override void Init()
        {
            if (!_unit.IsPlayerForThisClient)
                return;

            var joyStickView = ViewManager.Instance.GetView<JoystickView>();
            if (joyStickView != null)
            {
                _leftJoystick = joyStickView.Left;
                _rightJoystick = joyStickView.Right;
            }

            var attachmentView = ViewManager.Instance.GetView<AttachmentView>();
            _activeAttachmentBtnInputs = attachmentView.GetButtons().Select(go => go.GetComponent<HarunaButton>()).ToArray();


        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!_unit.IsPlayerForThisClient)
                return;

            UpdateMoveDirection();
            UpdateMainFireControl();
            UpdateAttachmentButtons();
        }

        void UpdateMoveDirection()
        {
            if (_leftJoystick != null)
            {
                if (_leftJoystick.CurrentDown && _leftJoystick.DirectionToActivePadInMainCamera != Vector3.zero)
                    _moveDirection = _leftJoystick.DirectionToActivePadInMainCamera;
                else
                    _moveDirection = null;
            }
        }


        void UpdateAttachmentButtons()
        {
            for (var i = 0; i < _activeAttachmentBtnInputs.Length; i++)
            {
                var input = _activeAttachmentBtnInputs[i];
                _attachmentPress[i] = !input.PreviousDown && input.CurrentDown;
                _attachmentsHolding[i] = input.Holding;
                _attachmentRelease[i] = input.PreviousDown && !input.CurrentDown;

                _attachmentsClick[i] = input.Clicked;
            }
        }

        void UpdateMainFireControl()
        {
            if (_rightJoystick != null)
            {
                _mainFireControlPress = !_rightJoystick.PreviousDown && _rightJoystick.CurrentDown;
                _mainFireControlRelease = _rightJoystick.PreviousDown && !_rightJoystick.CurrentDown;
                _mainFireControlHolding = _rightJoystick.Holding;
            }
        }

    }
}