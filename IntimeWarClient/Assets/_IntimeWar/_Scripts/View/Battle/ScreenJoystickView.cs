using Haruna.UI;
using UnityEngine;

namespace MechSquad.View
{
	public class ScreenJoystickView : BaseView
	{
		[SerializeField]
		HarunaJoyStick _left;
		public HarunaJoyStick Left { get { return _left; } }

		[SerializeField]
		HarunaJoyStick _right;
		public HarunaJoyStick Right { get { return _right; } }

		//[SerializeField]
		//HarunaButton _rightStick;
	}
}
