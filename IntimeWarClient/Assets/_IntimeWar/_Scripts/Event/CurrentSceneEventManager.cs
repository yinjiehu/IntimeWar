using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MechSquad.Event
{
	public class CurrentSceneEventManager : AbstractEventManager<EventBindingInfo, CurrentSceneEvent>
	{
		static CurrentSceneEventManager _instance;

		public static CurrentSceneEventManager Get()
		{
			if (_instance != null)
				return _instance;

			var go = new GameObject(typeof(CurrentSceneEventManager).Name);
			_instance = go.AddComponent<CurrentSceneEventManager>();

			return _instance;
		}
		public static CurrentSceneEventManager Instance { get { return Get(); } }

		public EventBindingInfo ReigstEvent<R>(Action<R, EventControl> callback, float priority = 0) where R : CurrentSceneEvent
		{
			var eventBindingInfo = new EventBindingInfo();
			eventBindingInfo.Priority = priority;
			eventBindingInfo.BindType = typeof(R);
			eventBindingInfo.Callback = callback;

			RegistEvent(eventBindingInfo);

			return eventBindingInfo;
		}

		protected override void CallDelegate(EventBindingInfo bindingInfo, CurrentSceneEvent eventInfo, EventControl eventControl)
		{
			bindingInfo.Callback.DynamicInvoke(eventInfo, eventControl);
		}
	}
	
	public class CurrentSceneEvent
	{

	}
}