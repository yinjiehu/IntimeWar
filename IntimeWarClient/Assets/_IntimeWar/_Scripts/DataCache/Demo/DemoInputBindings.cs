using MechSquadShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MechSquad
{
	public class DemoInputBindings
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitAttachmentSettings()
		{
			CustomPref.Control.KeyboardAndMouseBinding = new KeyboardAndMouseBinding();

			CustomPref.Control.KeyboardAndMouseBinding.ZoomChange.BindKey(KeyCode.M, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AimingModeAuto.BindKey(KeyCode.F, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AimingModeManual.BindKey(KeyCode.Mouse1, 0);
			
			CustomPref.Control.KeyboardAndMouseBinding.MainFireControl.BindKey(KeyCode.Mouse0, 0);

			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[0].BindKey(KeyCode.Alpha1, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[1].BindKey(KeyCode.Alpha2, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[2].BindKey(KeyCode.Alpha3, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[3].BindKey(KeyCode.Alpha4, 0);

			CustomPref.Control.KeyboardAndMouseBinding.AllAttachmentConnection.BindKey(KeyCode.R, 0);


			CustomPref.Control.JoystickBinding = new JoystickInputBinding();

			CustomPref.Control.JoystickBinding.ZoomChange.BindKey(KeyCode.Joystick1Button8, 0);
			CustomPref.Control.JoystickBinding.AimingModeAuto.BindKey(KeyCode.Joystick1Button10, 0);
			CustomPref.Control.JoystickBinding.AimingModeManual.BindKey(KeyCode.Joystick1Button11, 0);

			CustomPref.Control.JoystickBinding.MainFireControl.BindKey(KeyCode.Joystick1Button8, 0);

			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[0].BindKey(KeyCode.Joystick1Button0, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[0].BindKey(KeyCode.Joystick1Button4, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[1].BindKey(KeyCode.Joystick1Button1, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[0].BindKey(KeyCode.Joystick1Button5, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[2].BindKey(KeyCode.Joystick1Button2, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[0].BindKey(KeyCode.Joystick1Button6, 0);
			CustomPref.Control.KeyboardAndMouseBinding.AttachmentButtons[3].BindKey(KeyCode.Joystick1Button3, 0);
			
			CustomPref.Control.KeyboardAndMouseBinding.AllAttachmentConnection.BindKey(KeyCode.Joystick1Button9, 0);
		}
	}
}
