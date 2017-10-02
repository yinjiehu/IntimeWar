using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public abstract class ActiveAttachment : UnitAttachment
	{
		public abstract bool ShowDragingCursor { get; }

		public abstract bool CanConnectToMainFireControl { get; }
		public virtual bool IsConnectedToMainFireControl { get { return false; } }

		public virtual ReloadStateEnum ReloadingState { get { return ReloadStateEnum.None; } }
		public virtual float ReloadingCompleteRate { get { return 1; } }

		public abstract bool ShowCartridge { get; }
		public abstract bool ShowTotalAmmo { get; }

		public virtual float CartridgeCapacity { get { return 1; } }
		public virtual float CurrentAmmoCountInCartridge { get { return 0; } }
		public virtual float CurrentAmmoCountInTotal { get { return 0; } }

		public static Quaternion CalcCorrectionShootDirectionByMuzzle(float range, Transform _muzzleTransform, Vector3 unitPosition)
		{
			var angle = Mathf.Asin(_muzzleTransform.InverseTransformPoint(unitPosition).x / range);
			var muzzleRotationEuler = _muzzleTransform.rotation.eulerAngles;
			muzzleRotationEuler.y += angle;
			return Quaternion.Euler(muzzleRotationEuler);
		}
	}
}