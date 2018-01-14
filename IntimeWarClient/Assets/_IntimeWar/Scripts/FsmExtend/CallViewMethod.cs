using HutongGames.PlayMaker;
using MechSquad.View;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class CallViewMethod : FsmStateAction
	{
		public FsmString _viewName;

		public FsmString _methodName;

		public int _parametersLength;
		public FsmVar _parameter0;
		public FsmVar _parameter1;
		public FsmVar _parameter2;

		public bool _hasReturnValue;
		public FsmVar _storeReturnValue;

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
			
			var method = view.GetType().GetMethod(_methodName.Value, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if(method == null)
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Can not find view event or method named {0} in view {1}, at state {1}",
					_methodName.Value, _viewName.Value, State.Name);
				return;
			}

			try
			{
				object returnValue;
				if (_parametersLength == 0)
				{
					returnValue = method.Invoke(view, null);
				}
				else
				{
					var parameters = new object[_parametersLength];
					int i = 0;
					if (i < _parametersLength)
						parameters[i++] = _parameter0.NamedVar.RawValue;
					if (i < _parametersLength)
						parameters[i++] = _parameter1.NamedVar.RawValue;
					if (i < _parametersLength)
						parameters[i++] = _parameter2.NamedVar.RawValue;

					returnValue = method.Invoke(view, parameters);
				}
				
				if (_hasReturnValue)
					_storeReturnValue.SetValue(returnValue);

				Finish();
			}
			catch (Exception e)
			{
				Debug.LogErrorFormat(Fsm.FsmComponent, "Error occured when call method {0} in view {1}, at state {1}",
					_methodName.Value, _viewName.Value, State.Name);
				Debug.LogException(e);
			}
		}
	}
}