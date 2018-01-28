using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View
{
    public class BaseView : MonoBehaviour
    {
        [SerializeField]
        UnityEvent _initializeEvent;
        public event UnityAction EvOnInitialize
        {
            add { _initializeEvent.AddListener(value); }
            remove { _initializeEvent.RemoveListener(value); }
        }
        [SerializeField]
        UnityEvent _showEvent;
        public event UnityAction EvOnShow
        {
            add { _showEvent.AddListener(value); }
            remove { _showEvent.RemoveListener(value); }
        }
        [SerializeField]
        UnityEvent _hideEvent;
        public event UnityAction EvOnHide
        {
            add { _hideEvent.AddListener(value); }
            remove { _hideEvent.RemoveListener(value); }
        }

        //[SerializeField]
        //PanelLayerEnum _sortingLayer = PanelLayerEnum.Default;
        //public PanelLayerEnum SortingLayer { get { return _sortingLayer; } }
        [SerializeField]
        bool _setActiveOnShow;
        [SerializeField]
        bool _setDeactiveOnHide;

        public enum ActionOnInitializeEnum
        {
            DoNothing,
            ShowOnInitializing,
            HideOnInitializing
        }
        [SerializeField]
        ActionOnInitializeEnum _actionOnInializing = ActionOnInitializeEnum.DoNothing;

        [Serializable]
        public class CustomEvent
        {
            public string EvName;
            public UnityEvent Event;
        }
        [SerializeField]
        List<CustomEvent> _customEvents;

        [Serializable]
        public class Listener
        {
            public string ListenerName;
            public ViewEventListener ListenerInstance;
        }
        [SerializeField]
        List<Listener> _eventListeners;

        public virtual void Init()
        {
            if (_initializeEvent != null)
                _initializeEvent.Invoke();

            if (_actionOnInializing == ActionOnInitializeEnum.ShowOnInitializing)
                Show();
            else if (_actionOnInializing == ActionOnInitializeEnum.HideOnInitializing)
                Hide();
        }

        public virtual void Show()
        {
            if (_setActiveOnShow)
                gameObject.SetActive(true);

            if (_showEvent != null)
                _showEvent.Invoke();
        }

        public virtual void Hide()
        {
            if (_setDeactiveOnHide)
                gameObject.SetActive(false);

            if (_hideEvent != null)
                _hideEvent.Invoke();
        }

        string _previousEvName;

        public virtual bool CallEvent(string evName)
        {
            if (evName == "Show" && _previousEvName != evName)
            {
                Show();
                return true;
            }
            if (evName == "Hide" && _previousEvName != evName)
            {
                Hide();
                return true;
            }

            if (string.IsNullOrEmpty(evName))
            {
                Debug.LogErrorFormat("event name is null!");
                return false;
            }

            for (var i = 0; i < _customEvents.Count; i++)
            {
                if (_customEvents[i].EvName == evName)
                {
                    _customEvents[i].Event.Invoke();
                    return true;
                }
            }

            _previousEvName = evName;
            return false;
        }

        public virtual bool AddListener(string listenerName, Action<object> callback)
        {
            for (var i = 0; i < _eventListeners.Count; i++)
            {
                if (_eventListeners[i].ListenerName == listenerName)
                {
                    Debug.LogFormat(this, "Add view event listener [{0}] to [{1}]", listenerName, name);
                    _eventListeners[i].ListenerInstance.AddListener(callback);
                    return true;
                }
            }

            Debug.LogWarningFormat(this, "Can not find button listener {0} in view {1}", listenerName, name);
            return false;
        }
        public virtual bool RemoveListener(string listenerName, Action<object> callback)
        {
            for (var i = 0; i < _eventListeners.Count; i++)
            {
                if (_eventListeners[i].ListenerName == listenerName)
                {
                    Debug.LogFormat(this, "Remove button listener [{0}] to [{1}]", listenerName, name);

                    _eventListeners[i].ListenerInstance.RemoveListener(callback);
                    return true;
                }
            }

            Debug.LogWarningFormat(this, "Can not find button listener {0} in view {1}", listenerName, name);
            return false;
        }
    }
}