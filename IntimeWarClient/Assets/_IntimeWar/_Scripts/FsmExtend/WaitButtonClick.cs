
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class WaitButtonClick : FsmStateAction
	{
		public Button _button;

		public FsmEvent _onClick;

        public override void OnEnter()
        {
            base.OnEnter();
			_button.onClick.AddListener(OnClick);
        }

		void OnClick()
		{
			Fsm.Event(_onClick);
		}

		public override void OnExit()
		{
			base.OnExit();
			_button.onClick.RemoveListener(OnClick);
		}
	}
}