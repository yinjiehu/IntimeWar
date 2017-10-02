using UnityEngine;
using System.Linq;
using System;

namespace MechSquad.Battle
{
	public class AIInput : Ability, IUnitMobilityInput, IUnitAimingInput, IUnitAttachmentInput
	{
		public bool Enabled { get { return enabled; } }
		
		//mobility
		public Vector3? NormalizedMoveDirection { set; get; }
		
		//aiming
		public Vector3? AimingDirection { set; get; }
		
		bool _switchAimingMode;
		public bool SwitchAimingMode { get { return _switchAimingMode; } }
		AimingModeEnum _toSwitchAimingMode;
		public AimingModeEnum ToSwitchAimingMode { get { return _toSwitchAimingMode; } }

		bool _switchTargetOnAutoMode;
		public bool SwitchTargetOnAutoMode { get { return _switchTargetOnAutoMode; } }


		//attachment
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