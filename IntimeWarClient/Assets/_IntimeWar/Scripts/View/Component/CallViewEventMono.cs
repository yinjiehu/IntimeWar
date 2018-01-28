using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class CallViewEventMono : MonoBehaviour
	{
		[SerializeField]
		string _viewName;

		[SerializeField]
		string _eventName;

		public void Call()
		{
			var view = ViewManager.Instance.GetViewByName(_viewName);
			if(view == null)
			{
				Debug.LogErrorFormat(this, "Can not find view named {0}", _viewName);
			}
			else
			{
				view.CallEvent(_eventName);
			}
		}
	}
}