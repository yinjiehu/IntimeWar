using UnityEngine;
using System.Collections.Generic;
using System;

namespace YJH.Unit.Event
{
	public class UnitEventDispatcher : AbstractEventManager<EventBindingInfo, BattleUnitEvent>, IUnitAbility
    {
        public virtual string AbilityID { get { return name; } }

        [SerializeField]
		bool _sendSynchronization;
		public bool IsSyncAbility { get { return _sendSynchronization; } }

		protected BattleUnit _unit;
		public BattleUnit Unit { get { return _unit; } }
		
		HashSet<Type> _toReceiveEventTypes = new HashSet<Type>();

		public virtual void SetupInstance(BattleUnit unit)
		{
			_unit = unit;
		}

		public virtual void Init()
		{
		}

		public virtual void LateInit()
		{
		}

		public void AddReceiveEventType(Type t)
		{
			_toReceiveEventTypes.Add(t);
		}
		public void RemoveReceiveEventType(Type t)
		{
			_toReceiveEventTypes.Remove(t);
		}

		public override void TriggerEvent(BattleUnitEvent @event)
		{
			var type = @event.GetType();
			using(var itr = _toReceiveEventTypes.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					if(type == itr.Current || type.IsSubclassOf(itr.Current))
					{
						base.TriggerEvent(@event);
						break;
					}
				}
			}
		}

		public void ReigstEvent<R>(Action<R, EventControl> callback, float priority) where R : BattleUnitEvent
		{
			var eventInfo = new EventBindingInfo();
			eventInfo.Priority = priority;
			eventInfo.BindType = typeof(R);
			eventInfo.Callback = callback;

			RegistEvent(eventInfo);
		}

		protected override void CallDelegate(EventBindingInfo bindingInfo, BattleUnitEvent eventInfo, EventControl eventControl)
		{
			bindingInfo.Callback.DynamicInvoke(eventInfo, eventControl);
		}

        public void OnInitSynchronization(object data)
        {
        }
    }
}