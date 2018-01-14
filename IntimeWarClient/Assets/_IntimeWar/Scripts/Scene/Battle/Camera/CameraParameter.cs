using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad
{
	[CreateAssetMenu]
	public class CameraParameter : ScriptableObject
	{
		static CameraParameter _staticInstance;
		static CameraParameter Instance
		{
			get
			{
				if (_staticInstance == null)
					_staticInstance = Resources.Load<CameraParameter>("CameraParameter");
				return _staticInstance;
			}
		}
		
		[SerializeField]
		int _defaultPresetIndex = 1;
		public static int DefaultPresetIndex { get { return Instance._defaultPresetIndex; } }
		
		[SerializeField]
		AnimationCurve _cameraXToHeightCurve;
		public static float GetCameraX(float height)
		{
			return Instance._cameraXToHeightCurve.Evaluate(height);
		}

		[SerializeField]
		float[] _cameraHeightPreset;
		public static float[] CameraHeighPreset { get { return Instance._cameraHeightPreset; } }

		[Serializable]
		public struct CameraShakeParameter
		{
			public float StrengthMagnitude;
			public float Roughness;
			public float FadeinSeconds;
			public float FadeOutSeconds;
			public AnimationCurve DistanceDecay;
		}
		[SerializeField]
		CameraShakeParameter[] _cameraShakeStrengthLevel;
		public static CameraShakeParameter[] CameraShakeStrengthLevel { get { return Instance._cameraShakeStrengthLevel; } }
	}
}
