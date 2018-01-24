using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace YJH.Unit.Event
{
    public abstract class AbstractEventManager<TBindingInfo, TEvent> : MonoBehaviour where TBindingInfo : EventBindingInfo
    {
        List<TBindingInfo> _bingdingEvents = new List<TBindingInfo>();

        protected void RegistEvent(TBindingInfo eventBindingInfo)
        {
            _bingdingEvents.Add(eventBindingInfo);
            _bingdingEvents = _bingdingEvents.OrderByDescending(e => e.Priority).ToList();
        }

        public void RemoveEvent(TBindingInfo eventBindingInfo)
        {
            _bingdingEvents.Remove(eventBindingInfo);
        }

        //void Update()
        //{
        //	if (_toDeleteEvents.Count != 0)
        //	{
        //		_bingdingEvents.RemoveAll(e => _toDeleteEvents.Contains(e));
        //		_toDeleteEvents.Clear();
        //	}
        //	if (_toAddEvents.Count != 0)
        //	{
        //		_bingdingEvents.AddRange(_toAddEvents);
        //		_bingdingEvents.Sort((e1, e2) => (int)(e1.Priority - e2.Priority) * 1000000);
        //		_toAddEvents.Clear();
        //	}
        //}
        List<TBindingInfo> _ergodicList = new List<TBindingInfo>();
        public virtual void TriggerEvent(TEvent @event)
        {
            //if(_triggeringLevel == 0)

            //_triggeringLevel++;
            _ergodicList.Clear();
            _ergodicList.AddRange(_bingdingEvents);
            using (var itr = _ergodicList.GetEnumerator())
            {
                var fireType = @event.GetType();
                var control = new EventControl();
                while (itr.MoveNext())
                {
                    var bindType = itr.Current.BindType;
                    if (fireType == bindType || fireType.IsSubclassOf(bindType))
                    {
                        CallDelegate(itr.Current, @event, control);
                        if (control.DeleteThisRegist)
                            _bingdingEvents.Remove(itr.Current);
                        if (control.CancelNextFiring)
                            break;

                        control.DeleteThisRegist = false;
                    }
                }
            }
            //_triggeringLevel--;
        }

        protected abstract void CallDelegate(TBindingInfo bindingInfo, TEvent eventInfo, EventControl eventControl);
    }

    public class EventControl
    {
        public bool CancelNextFiring { set; get; }
        public bool DeleteThisRegist { set; get; }
    }

    public class EventBindingInfo
    {
        public float Priority;
        public Type BindType;
        public Delegate Callback;
    }
}