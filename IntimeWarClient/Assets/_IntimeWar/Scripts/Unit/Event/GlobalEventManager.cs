using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace YJH.Unit.Event
{
    public class GlobalEventManager : AbstractEventManager<EventBindingInfo, GlobalSceneEvent>
    {
        static GlobalEventManager _instance;

        public static GlobalEventManager Get()
        {
            if (_instance != null)
                return _instance;

            var go = new GameObject(typeof(GlobalEventManager).Name);
            _instance = go.AddComponent<GlobalEventManager>();

            return _instance;
        }
        public static GlobalEventManager Instance { get { return Get(); } }

        public EventBindingInfo ReigstEvent<R>(Action<R, EventControl> callback, float priority) where R : CurrentSceneEvent
        {
            var eventBindingInfo = new EventBindingInfo();
            eventBindingInfo.Priority = priority;
            eventBindingInfo.BindType = typeof(R);
            eventBindingInfo.Callback = callback;

            RegistEvent(eventBindingInfo);

            return eventBindingInfo;
        }

        protected override void CallDelegate(EventBindingInfo bindingInfo, GlobalSceneEvent eventInfo, EventControl eventControl)
        {
            bindingInfo.Callback.DynamicInvoke(eventInfo, eventControl);
        }
    }

    public class GlobalSceneEvent
    {

    }
}