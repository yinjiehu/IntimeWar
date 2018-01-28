using HutongGames.PlayMaker;
using View;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace View.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class ListenViewEvent : FsmStateAction
	{
		public FsmString _viewName;
		public FsmString _listenName;

		public FsmEvent _onListenerEvent;
				
		public override void OnEnter()
		{
			base.OnEnter();

			var view = ViewManager.Instance.GetViewByName(_viewName.Value);
			if (view == null)
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view named {0} in state {1}", _viewName.Value, State.Name);
				return;
			}

			view.AddListener(_listenName.Value, OnListenerTrigger);
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

				view.RemoveListener(_listenName.Value, OnListenerTrigger);
			}
		}

		void OnListenerTrigger(object p)
		{
			Fsm.Event(_onListenerEvent);
			Finish();
		}
	}
}