using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace MechSquad.View
{
	public class ViewManager : MonoBehaviour
	{
		static ViewManager _instance;
		public static ViewManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<ViewManager>();
					//DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}

		//List<BaseView> _viewList = new List<BaseView>();

		List<ViewContainer> _viewContainerList = new List<ViewContainer>();
		Dictionary<string, ViewContainer> _viewContainerMapping = new Dictionary<string, ViewContainer>();
		
		void Awake()
		{
			var list = GetComponentsInChildren<ViewContainer>();
			for (var i = 0; i < list.Length; i++)
			{
				if (_viewContainerMapping.ContainsKey(list[i].name))
				{
					Debug.LogErrorFormat(list[i], "View name dupicate in {0}", list[i].name);
				}
				else
				{
					_viewContainerMapping.Add(list[i].name, list[i]);
				}
			}
			_viewContainerList.AddRange(GetComponentsInChildren<ViewContainer>(true));
		}

		public void AddSubViewContainer(IList<ViewContainer> list)
		{
			for (var i = 0; i < list.Count; i++)
			{
				if (_viewContainerMapping.ContainsKey(list[i].name))
				{
					Debug.LogErrorFormat(list[i], "View name dupicate in {0}", list[i].name);
				}
				else
				{
					_viewContainerMapping.Add(list[i].name, list[i]);
				}
			}
			_viewContainerList.AddRange(list);
		}

		public BaseView GetViewByName(string viewName)
		{
			ViewContainer c;
			if (_viewContainerMapping.TryGetValue(viewName, out c))
			{
				return c.ViewInstance;
			}

			return null;
		}

		public BaseView GetView(Type type)
		{
			for (var i = 0; i < _viewContainerList.Count; i++)
			{
				var vtype = _viewContainerList[i].ViewPrefab.GetType();
				if (vtype == type || vtype.IsSubclassOf(type))
					return _viewContainerList[i].ViewInstance;
			}
			return null;
		}
		public T GetView<T>() where T : BaseView
		{
			for (var i = 0; i < _viewContainerList.Count; i++)
			{
				var p = _viewContainerList[i].ViewPrefab;
				if (p is T)
					return _viewContainerList[i].ViewInstance as T;
			}
			return null;
		}

		public T Show<T>() where T : BaseView
		{
			var v = GetView<T>();
			if (v == null)
				Debug.LogErrorFormat("Can not get view by type {0}", typeof(T).Name);
			v.Show();

			return v;
		}
		public T Hide<T>() where T : BaseView
		{
			var v = GetView<T>();
			if (v == null)
				Debug.LogErrorFormat("Can not get view by type {0}", typeof(T).Name);
			v.Hide();

			return v;
		}

		public BaseView ShowByName(string viewName)
		{
			var view = GetViewByName(viewName);
			if (view == null)
				Debug.LogErrorFormat("Can not get view by name {0}", viewName);
			else
				view.Show();

			return view;
		}
		public BaseView HideByName(string viewName)
		{
			var view = GetViewByName(viewName);
			if (view == null)
				Debug.LogErrorFormat("Can not get view by name {0}", viewName);
			else
				view.Hide();

			return view;
		}
	}
}