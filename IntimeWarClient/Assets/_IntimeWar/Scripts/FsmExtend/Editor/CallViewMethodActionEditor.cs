using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor;

namespace MechSquad.Fsm
{
	[CustomActionEditor(typeof(CallViewMethod))]
	public class CallViewMethodActionEditor : CustomActionEditor
	{
		public override bool OnGUI()
		{
			FsmEditor.ActionEditor.EditField(target, "_viewName");
			FsmEditor.ActionEditor.EditField(target, "_methodName");
			EditorGUILayout.Space();
			
			FsmEditor.ActionEditor.EditField(target, "_parametersLength");
			var action = target as CallViewMethod;
			var argLength = action._parametersLength;

			EditorGUI.indentLevel++;
			for(var i = 0; i < argLength; i++)
			{
				FsmEditor.ActionEditor.EditField(target, "_parameter" + i);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			FsmEditor.ActionEditor.EditField(target, "_hasReturnValue");
			if (action._hasReturnValue)
			{
				EditorGUI.indentLevel++;
				FsmEditor.ActionEditor.EditField(target, "_storeReturnValue");
				EditorGUI.indentLevel--;
			}

			return GUI.changed;
		}
	}
}