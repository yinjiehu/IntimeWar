using HutongGames.PlayMaker;
using View;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace IntimeWar.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class CallViewEvent : FsmStateAction
	{
		public FsmString _viewName;
		public enum ViewEventTypeEnum
		{
			Show,
			Hide,
			Custom
		}
		public ViewEventTypeEnum _eventType;
		public FsmString _eventName;
		
		public override void OnEnter()
		{
			base.OnEnter();

			var view = ViewManager.Instance.GetViewByName(_viewName.Value);
			if (view == null)
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view named {0} at state {1}", _viewName.Value, State.Name);
				return;
			}

			string actualEvName;
			if (_eventType == ViewEventTypeEnum.Show)
				actualEvName = "Show";
			else if (_eventType == ViewEventTypeEnum.Hide)
				actualEvName = "Hide";
			else
				actualEvName = _eventName.Value;

			if (!view.CallEvent(actualEvName))
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view event named [{0}] in view [{1}], at state [{2}]",
					_eventName.Value, _viewName.Value, State.Name);
				return;
			}
			
			Finish();
		}
	}
}