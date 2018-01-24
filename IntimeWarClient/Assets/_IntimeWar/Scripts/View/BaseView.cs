using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

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

		public virtual bool CallEvent(string evName)
		{
			if (evName == "Show")
			{
				Show();
				return true;
			}
			if (evName == "Hide")
			{
				Hide();
				return true;
			}

			for (var i = 0; i < _customEvents.Count; i++)
			{
				if (_customEvents[i].EvName == evName)
				{
					_customEvents[i].Event.Invoke();
					return true;
				}
			}
			return false;
		}
	}
}