using UnityEngine;
using System.Linq;
using System;

namespace MechSquad.Battle
{
	public interface IUnitMobilityInput
	{
		bool Enabled { get; }
		Vector3? NormalizedMoveDirection { get; }
	}


	public interface IUnitAimingInput
	{
		bool Enabled { get; }
		Vector3? AimingDirection { get; }

		bool SwitchAimingMode { get; }
		AimingModeEnum ToSwitchAimingMode { get; }
		
		bool SwitchTargetOnAutoMode { get; }
	}

	public interface IUnitAttachmentInput
	{
		bool Enabled { get; }

		bool[] AttachmentPress { get; }
		bool[] AttachmentHolding { get; }
		bool[] AttachmentRelease { get; }
		bool[] AttachmentClicked { get; }

		bool MainFireControlPress { get; }
		bool MainFireControlHoding { get; }
		bool MainFireControlRelease { get; }
	}

	//public interface IAimingCursorInput
	//{
	//	Vector3? CursorTargetPosition { get; }
	//}

	//public interface IUnitWeaponControlInput
	//{
	//	Action<string> EvOnWeaponFire { set; get; }
	//}

	//public interface IUnitSkillControlInput
	//{
	//	Action<string> EvOnSkillActivate { set; get; }
	//}
}