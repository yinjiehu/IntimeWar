using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MechSquad
{
	public class CustomPref
	{
		static CustomPref _instance;
		public static CustomPref Instance
		{
			get
			{
				if (_instance == null)
					_instance = new CustomPref();
				return _instance;
			}
		}

		public class GraphicPref
		{
			public enum ParticleEffectLevelEnum
			{
				Low,
				Middle,
				High,
			}
			public ParticleEffectLevelEnum EffectLevel;

		}
		GraphicPref _graphic = new GraphicPref();
		public static GraphicPref Graphic { get { return Instance._graphic; } }

		public class ControlPref
		{
			public enum ControlDeviceTypeEnum
			{
				Mobile,
				PC,
			}
			public ControlDeviceTypeEnum ControlDeviceType
			{
				get
				{
#if UNITY_EDITOR
					return ControlDeviceTypeEnum.PC;
#elif UNITY_ANDROID || UNITY_IOS
					return ControlDeviceTypeEnum.Mobile;
#endif
				}
			}

			public bool AutoCorrectionForManualAiming = true;
			
			public bool DragToCancelAutoAimingMode = false;

			public KeyboardAndMouseBinding KeyboardAndMouseBinding;
			public JoystickInputBinding JoystickBinding;
		}

		ControlPref _control = new ControlPref();
		public static ControlPref Control { get { return Instance._control; } }

	}
}
