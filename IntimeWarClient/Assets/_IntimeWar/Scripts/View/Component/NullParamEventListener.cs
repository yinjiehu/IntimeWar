using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class NullParamEventListener : ViewEventListener
	{
		event Action<object> _event;
		
		public override void AddListener(Action<object> action)
		{
			_event += action;
		}
		
		public override void RemoveListener(Action<object> action)
		{
			_event -= action;
		}
		
		public void SendEvent()
		{
			if (_event != null)
				_event(null);
		}
	}
}