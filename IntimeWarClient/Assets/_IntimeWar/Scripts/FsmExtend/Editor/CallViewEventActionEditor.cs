using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace View.Fsm
{
	[CustomActionEditor(typeof(CallViewEvent))]
	public class CallViewEventActionEditor : CustomActionEditor
	{
		public override bool OnGUI()
		{
			FsmEditor.ActionEditor.EditField(target, "_viewName");
			FsmEditor.ActionEditor.EditField(target, "_onEnterEventType");

			var action = target as CallViewEvent;
			if(action._onEnterEventType == CallViewEvent.ViewEventTypeEnum.Custom)
			{
				FsmEditor.ActionEditor.EditField(target, "_onEnterEventCustomName");
			}

			FsmEditor.ActionEditor.EditField(target, "_onExitEventType");
			if (action._onExitEventType == CallViewEvent.ViewEventTypeEnum.Custom)
			{
				FsmEditor.ActionEditor.EditField(target, "_onExitEventCustomName");
			}
			FsmEditor.ActionEditor.EditField(target, "_finishAfterEnter");

			return GUI.changed;
		}
	}
}