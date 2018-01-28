using HutongGames.PlayMaker;
using View;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace View.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class CallViewEvent : FsmStateAction
	{
		public FsmString _viewName;
		public enum ViewEventTypeEnum
		{
			None,
			Show,
			Hide,
			Custom
		}
		public ViewEventTypeEnum _onEnterEventType;
		public FsmString _onEnterEventCustomName;


		public ViewEventTypeEnum _onExitEventType;
		public FsmString _onExitEventCustomName;

		public FsmBool _finishAfterEnter;

		public override void OnEnter()
		{
			base.OnEnter();

			if (_onEnterEventType != ViewEventTypeEnum.None)
				DoEvent(_onEnterEventType, _onEnterEventCustomName);

			if(_finishAfterEnter.Value)
				Finish();
		}

		public override void OnExit()
		{
			base.OnExit();

			if (_onExitEventType != ViewEventTypeEnum.None)
				DoEvent(_onExitEventType, _onExitEventCustomName);
		}

		void DoEvent(ViewEventTypeEnum evType, FsmString evName)
		{
			var instance = ViewManager.Instance;
			if (instance == null)
				return;

			var view = instance.GetViewByName(_viewName.Value);
			if (view == null)
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view named {0} at state {1}", _viewName.Value, State.Name);
				return;
			}

			string actualEvName;
			if (evType == ViewEventTypeEnum.Show)
				actualEvName = "Show";
			else if (evType == ViewEventTypeEnum.Hide)
				actualEvName = "Hide";
			else
				actualEvName = evName.Value;
			
			if (!view.CallEvent(actualEvName))
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view event named [{0}] in view [{1}], at state [{2}]",
					actualEvName, _viewName.Value, State.Name);
				return;
			}
		}
	}
}