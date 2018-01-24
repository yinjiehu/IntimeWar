

namespace IntimeWar
{
	public class ControlPref
	{
		public enum ControlDeviceTypeEnum
		{
			Mobile,
			PC,
		}

		ControlDeviceTypeEnum _controlDeviceType = ControlDeviceTypeEnum.PC;
		public ControlDeviceTypeEnum ControlDeviceType
		{
			set { _controlDeviceType = value; }
			get
			{
#if UNITY_EDITOR
				return _controlDeviceType;
#elif UNITY_ANDROID || UNITY_IOS
				return ControlDeviceTypeEnum.Mobile;
#else
				return ControlDeviceTypeEnum.PC;
#endif
			}
		}

        public enum TextLanguage
        {
            ZH_CN,
            EN_US
        }
        public TextLanguage Language = TextLanguage.ZH_CN;

        public bool AutoAiming = false;
		public bool AutoCorrectionForManualAiming = true;
         
		public bool DragToCancelAutoAimingMode = false;

	}
	
	public class KeysPref
	{
		public KeyboardMouseBinding KeyboardAndMouseBinding = new KeyboardMouseBinding();
		public JoystickInputBinding JoystickBinding = new JoystickInputBinding();
	}
}
