using HutongGames.PlayMaker;
using View;
using UnityEngine;
using UnityEngine.Events;
using IntimeWar.View;

namespace View.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class ShowMessageBox : FsmStateAction
	{
		[SerializeField]
		public FsmString _message;

		[SerializeField]
		public FsmEvent _finishEvent;

		public override void OnEnter()
		{
			base.OnEnter();
			MessageBoxView.Show(_message.Value, OnMessageBoxHide);
		}

		void OnMessageBoxHide()
		{
			if (_finishEvent != null)
				Fsm.Event(_finishEvent);

			Finish();
		}
	}
}