using HutongGames.PlayMaker;
using MechSquad.View;
using UnityEngine;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class TransitionControl : FsmStateAction
	{
		public enum ControlTypeEnum
		{
			StartTransitionIn,
			StartTransitionOut
		}
		[SerializeField]
		public ControlTypeEnum _control;
		
		public override void OnEnter()
		{
			base.OnEnter();
			if(_control == ControlTypeEnum.StartTransitionIn)
			{
				ViewManager.Instance.GetViewByName("Transition").Show();
			}
			else
			{
				ViewManager.Instance.GetViewByName("Transition").Hide();
			}

			Finish();
		}
	}
}