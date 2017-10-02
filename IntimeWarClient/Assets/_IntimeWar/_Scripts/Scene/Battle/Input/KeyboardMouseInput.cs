using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	public class KeyboardMouseInput : Ability, IUnitMobilityInput, IUnitAimingInput, IUnitAttachmentInput
	{
		public bool Enabled { get { return enabled; } }

		Vector3? _moveDirection;
		public Vector3? NormalizedMoveDirection { get { return _moveDirection; } }

		Vector3? _aimingDirection;
		public Vector3? AimingDirection { get { return _aimingDirection; } }

		bool _switchAimingMode;
		public bool SwitchAimingMode { get { return _switchAimingMode; } }
		AimingModeEnum _toSwitchAimingMode;
		public AimingModeEnum ToSwitchAimingMode { get { return _toSwitchAimingMode; } }

		bool _switchTargetOnAutoMode;
		public bool SwitchTargetOnAutoMode { get { return _switchTargetOnAutoMode; } }

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

		IMainFireControlSystem _mainFireControl;
		UnitAttachmentManager _attachmentManager;
		KeyboardAndMouseBinding _inputBinding;
		CameraZoomControl _zoomControl;

		public override void Init()
		{
			base.Init();
			_mainFireControl = _unit.GetAbility<IMainFireControlSystem>();
			_attachmentManager = _unit.GetAbility<UnitAttachmentManager>();
			_zoomControl = FindObjectOfType<CameraZoomControl>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			_inputBinding = CustomPref.Control.KeyboardAndMouseBinding;
			_inputBinding.OnUpdate(Time.deltaTime);

			UpdateKeyboardMoving();
			UpdateKeyboardAiming();
			UpdateAimingMode();
			UpdateMainFireControl();
			UpdateAttachments();
			UpdateAllAttachmentConnection();
			UpdateManualAiming();
			UpdateMapZoom();
		}

		void UpdateKeyboardMoving()
		{
			var direction = _inputBinding.MovingDirection.NormalizedDirection;
			if (direction == Vector2.zero)
			{
				_moveDirection = null;
			}
			else
			{
				var worldDirection = Camera.main.transform.TransformDirection(direction);
				_moveDirection = worldDirection.ChangeY().normalized;
			}
		}
		void UpdateKeyboardAiming()
		{
			Vector3 direction = Vector3.zero;
			direction.y += Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
			direction.y += Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
			direction.x += Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
			direction.x += Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;

			if (direction == Vector3.zero)
			{
				_aimingDirection = null;
			}
			else
			{
				var worldDirection = Camera.main.transform.TransformDirection(direction);
				_aimingDirection = worldDirection.ChangeY().normalized; ;
			}
		}

		void UpdateAimingMode()
		{
			if (_inputBinding.AimingModeAuto.Press)
			{
				_switchAimingMode = true;
				_toSwitchAimingMode = AimingModeEnum.Auto;
			}
			else if (_inputBinding.AimingModeManual.Press)
			{
				_switchAimingMode = true;
				if (CustomPref.Control.AutoCorrectionForManualAiming)
					_toSwitchAimingMode = AimingModeEnum.SemiAuto;
				else
					_toSwitchAimingMode = AimingModeEnum.Manual;
			}
			else
			{
				_switchAimingMode = false;
			}
		}

		void UpdateMainFireControl()
		{
			_mainFireControlPress = _inputBinding.MainFireControl.Press;
			_mainFireControlHolding = _inputBinding.MainFireControl.CurrentDown;
			_mainFireControlRelease = _inputBinding.MainFireControl.Release;
		}

		void UpdateAllAttachmentConnection()
		{
			if (_inputBinding.AllAttachmentConnection.Press)
			{
				var atts = new UnitAttachment[4];
				using(var itr = _attachmentManager.GetAllActiveAttachment().GetEnumerator())
				{
					while (itr.MoveNext())
					{
						atts[itr.Current.Key] = itr.Current.Value;
					}
				}
				var allDisconnected = true;
				for(var i = 0; i < atts.Length; i++)
				{
					var a = atts[i];
					if(a != null && a is IWeaponTypeR && ((IWeaponTypeR)a).IsConnectedToMainFireControl)
					{
						allDisconnected = false;
						break;
					}
				}
				for (var i = 0; i < atts.Length; i++)
				{
					var a = atts[i];
					if (a != null && a is IWeaponTypeR)
					{
						if ((allDisconnected && !((IWeaponTypeR)a).IsConnectedToMainFireControl)
							|| (!allDisconnected && ((IWeaponTypeR)a).IsConnectedToMainFireControl))
							_attachmentClicked[i] = true;
					}
				}
			}
		}

		void UpdateAttachments()
		{
			for (var i = 0; i < 4; i++)
			{
				var ks = _inputBinding.AttachmentButtons[i];
				_attachmentPress[i] = ks.Press;
				_attachmentHolding[i] = ks.Holding;
				_attachmentRelease[i] = ks.Release;
				_attachmentClicked[i] = ks.Clicked;
			}
		}

		void UpdateManualAiming()
		{
			if (_mainFireControl.CurrentAimingMode != AimingModeEnum.Auto && Input.mousePresent)
			{
				//var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).ChangeY(Camera.main.transform.position.y);
				//var height = _mainFireControl.TurretRoot.position.y;

				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 2000, LayerMask.GetMask("Ground")))
				{
					_aimingDirection = (hit.point - _unit.Model.position).ChangeY();
				}
			}
			else
			{
				_aimingDirection = null;
			}
		}

		void UpdateMapZoom()
		{
			if (_inputBinding.ZoomChange.Press)
			{
				_zoomControl.SwitchCameraHeightPreset();
			}
		}
	}
}