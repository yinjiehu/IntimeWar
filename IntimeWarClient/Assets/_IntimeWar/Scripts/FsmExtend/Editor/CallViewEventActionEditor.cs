using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace IntimeWar.Fsm
{
	[CustomActionEditor(typeof(CallViewEvent))]
	public class CallViewEventActionEditor : CustomActionEditor
	{
		public override bool OnGUI()
		{
			FsmEditor.ActionEditor.EditField(target, "_viewName");
			FsmEditor.ActionEditor.EditField(target, "_eventType");

			var action = target as CallViewEvent;
			if(action._eventType == CallViewEvent.ViewEventTypeEnum.Custom)
			{
				FsmEditor.ActionEditor.EditField(target, "_eventName");
			}

			return GUI.changed;
		}
	}
}