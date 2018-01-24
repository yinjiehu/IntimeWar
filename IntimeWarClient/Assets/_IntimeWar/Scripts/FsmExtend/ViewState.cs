using HutongGames.PlayMaker;
using View;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IntimeWar.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class ViewState : FsmStateAction
	{
		public FsmString _viewName;
		
		public override void OnEnter()
		{
			base.OnEnter();
			//MessageBoxView.Show(_message.Value, OnMessageBoxHide);

			var view = ViewManager.Instance.GetViewByName(_viewName.Value);
			if (view == null)
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view named {0} in state {1}", _viewName.Value, State.Name);
				return;
			}

			view.Show();

			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();

			var ins = ViewManager.Instance;
			if (ins != null)
			{
				var view = ViewManager.Instance.GetViewByName(_viewName.Value);
				if (view == null)
				{
					Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view named {0} in state {1}", _viewName.Value, State.Name);
					return;
				}

				view.Hide();
			}
		}
	}
}