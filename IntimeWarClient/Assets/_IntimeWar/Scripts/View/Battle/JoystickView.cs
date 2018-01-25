using UnityEngine;
using View;
using Haruna.UI;

namespace IntimeWar.View
{
    public class JoystickView : BaseView
    {
        [SerializeField]
        HarunaJoyStick _left;
        public HarunaJoyStick Left { get { return _left; } }
    }
}