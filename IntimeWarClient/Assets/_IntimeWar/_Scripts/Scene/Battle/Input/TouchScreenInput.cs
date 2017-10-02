using UnityEngine;
using System.Linq;
using MechSquad.View;
using Haruna.UI;

namespace MechSquad.Battle
{
	public class TouchScreenInput : Ability, IUnitMobilityInput, IUnitAimingInput, IUnitAttachmentInput
	{
		[SerializeField]
		float _autoToSemiAutoDragDistance = 0.5f;

		public bool Enabled { get { return enabled; } }

		HarunaJoyStick _leftJoystick;
		HarunaJoyStick _rightJoystick;
		HarunaButton[] _activeAttachmentBtnInputs;
		SwitchAimingModeView _aimingModeSwitchView;

		#region mobility implement
		Vector3? _moveDirection;
		public Vector3? NormalizedMoveDirection { get { return _moveDirection; } }
		#endregion

		#region aiming implement
		Vector3? _aimingDirection;
		public Vector3? AimingDirection { get { return _aimingDirection; } }

		bool _switchAimingMode;
		public bool SwitchAimingMode { get { return _switchAimingMode; } }
		AimingModeEnum _toSwitchAimingMode;
		public AimingModeEnum ToSwitchAimingMode { get { return _toSwitchAimingMode; } }

		bool _switchTargetOnAutoMode;
		public bool SwitchTargetOnAutoMode { get { return _switchTargetOnAutoMode; } }
		#endregion

		#region attachment implement
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

		RaidarSystem _raidarSystem;
		IMainFireControlSystem _mainFireControl;

		public override void Init()
		{
			if (!_unit.IsPlayerForThisClient)
				return;

			var joyStickView = View.ViewManager.Instance.GetView<ScreenJoystickView>();
			_leftJoystick = joyStickView.Left;
			_rightJoystick = joyStickView.Right;

			var attachmentBtnView = ViewManager.Instance.GetView<ActiveAttachmentBtnView>();
			_activeAttachmentBtnInputs = attachmentBtnView.GetButtons().Select(go => go.GetComponent<HarunaButton>()).ToArray();

			_aimingModeSwitchView = ViewManager.Instance.GetView<SwitchAimingModeView>();

			_raidarSystem = _unit.GetAbility<RaidarSystem>();
			_mainFireControl = _unit.GetAbility<IMainFireControlSystem>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (CustomPref.Control.ControlDeviceType != CustomPref.ControlPref.ControlDeviceTypeEnum.Mobile || !_unit.IsPlayerForThisClient)
				return;

			UpdateMoveDirection();

			UpdateSwitchAimingMode();
			UpdateSwitchTargetOnAutoMode();

			UpdateMainFireControl();
			UpdateAttachmentButtons();
		}

		void UpdateMoveDirection()
		{
			if (_leftJoystick.CurrentDown && _leftJoystick.DirectionToActivePadInMainCamera != Vector3.zero)
				_moveDirection = _leftJoystick.DirectionToActivePadInMainCamera;
			else
				_moveDirection = null;
		}

		enum AimingStateEnum
		{
			None,
			Tracking,
			Shooting,
		}
		AimingStateEnum _aimingState;

		void UpdateMainFireControl()
		{
			if(_mainFireControl.CurrentAimingMode == AimingModeEnum.Auto && _mainFireControl.GetAimingTarget() != null)
			{
				_aimingDirection = _rightJoystick.DirectionToActivePadInMainCamera;

				_mainFireControlPress = !_rightJoystick.PreviousDown && _rightJoystick.CurrentDown;
				_mainFireControlRelease = _rightJoystick.PreviousDown && !_rightJoystick.CurrentDown;
				_mainFireControlHolding = _rightJoystick.Holding;
			}
			else
			{
				if (_rightJoystick.CurrentDown)
				{
					_aimingDirection = _rightJoystick.DirectionToActivePadInMainCamera;

					if (_aimingState == AimingStateEnum.None)
					{
						_aimingState = AimingStateEnum.Tracking;
					}
					else if (_aimingState == AimingStateEnum.Tracking)
					{
						var turretDirection = _unit.GetAbility<IMainFireControlSystem>().CurrentTurretDirection;
						if (_aimingDirection == Vector3.zero && _rightJoystick.Holding)
						{
							_mainFireControlPress = true;
							_aimingState = AimingStateEnum.Shooting;
						}
						else if (_aimingDirection != Vector3.zero && Vector3.Angle(turretDirection, _aimingDirection.Value) < 1)
						{
							_mainFireControlPress = true;
							_aimingState = AimingStateEnum.Shooting;
						}
					}
					else
					{
						_mainFireControlPress = false;
						_mainFireControlHolding = _rightJoystick.Holding;
					}
				}
				else
				{
					_mainFireControlPress = false;
					_mainFireControlHolding = false;
					_mainFireControlRelease = _aimingState == AimingStateEnum.Shooting;
					_aimingState = AimingStateEnum.None;
				}
			}
		}

		void UpdateSwitchAimingMode()
		{
			if (_aimingModeSwitchView.Clicked)
			{
				if (_mainFireControl.CurrentAimingMode == AimingModeEnum.Auto)
				{
					_switchAimingMode = true;
					_toSwitchAimingMode = AimingModeEnum.Manual;
				}
				else
				{
					_switchAimingMode = true;
					_toSwitchAimingMode = AimingModeEnum.Auto;
				}
			}
			else if (_rightJoystick.Holding && _mainFireControl.CurrentAimingMode == AimingModeEnum.Manual)
			{
				_switchAimingMode = true;
				_toSwitchAimingMode = AimingModeEnum.SemiAuto;
			}
			else if (_rightJoystick.Holding && _mainFireControl.CurrentAimingMode == AimingModeEnum.Auto && CustomPref.Control.DragToCancelAutoAimingMode)
			{
				_switchAimingMode = true;

				if (CustomPref.Control.AutoCorrectionForManualAiming)
					_toSwitchAimingMode = AimingModeEnum.SemiAuto;
				else
					_toSwitchAimingMode = AimingModeEnum.Manual;
			}
			else if (_rightJoystick.Clicked && _mainFireControl.CurrentAimingMode != AimingModeEnum.Auto && CustomPref.Control.DragToCancelAutoAimingMode)
			{
				_switchAimingMode = true;
				_toSwitchAimingMode = AimingModeEnum.Auto;
			}
			else if (!_rightJoystick.Holding && _mainFireControl.CurrentAimingMode == AimingModeEnum.SemiAuto)
			{
				_switchAimingMode = true;
				_toSwitchAimingMode = AimingModeEnum.Manual;
			}
			else
			{
				_switchAimingMode = false;
			}
		}

		void UpdateSwitchTargetOnAutoMode()
		{
			if (_mainFireControl.CurrentAimingMode == AimingModeEnum.Auto && _rightJoystick.Clicked)
				_switchTargetOnAutoMode = true;
			else
				_switchTargetOnAutoMode = false;
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

		//void UpdateAutoReload()
		//{
		//	var attachments = _attachmentManager.GetAllActiveAttachment();
		//	using(var itr = attachments.GetEnumerator())
		//	{
		//		while (itr.MoveNext())
		//		{
		//			if(itr.Current.Value is RapidWeapon)
		//			{
		//				var rapidWeapon = itr.Current.Value as RapidWeapon;

		//			}
		//		}
		//	}
		//}
	}
}