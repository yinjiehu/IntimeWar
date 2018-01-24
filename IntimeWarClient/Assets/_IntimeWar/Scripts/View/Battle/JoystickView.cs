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

        public override void Init()
        {
            base.Init();
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}