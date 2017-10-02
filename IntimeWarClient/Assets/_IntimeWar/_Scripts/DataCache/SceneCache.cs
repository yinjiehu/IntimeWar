using System.Collections;
using UnityEngine;
using System;

namespace MechSquad
{
	public class SceneCache : MonoBehaviour
	{
		static SceneCache _instance;

        public static SceneCache Instance
        {
            get { return Get(); }
        }
        
		public static SceneCache Get()
		{
			if (_instance != null)
				return _instance;

			_instance = new GameObject("SceneCache").AddComponent<SceneCache>();
			DontDestroyOnLoad(_instance.gameObject);

			return _instance;
		}

		Hashtable _data = new Hashtable();

		public Hashtable Table { get { return _data; } }

		public static void Set(string key, object obj)
		{
            Instance._data[key] = obj;
		}

		public static bool TryGet(string key, out object obj)
		{
			if (Instance._data.ContainsKey(key))
			{
				obj = Instance._data[key];
				return true;
			}
			else
			{
				obj = null;
				return false;
			}
		}

		public static void Remove(string key)
		{
            Instance._data.Remove(key);
		}

		public static bool IsExsit(string key)
		{
			return Instance._data.ContainsKey(key);
		}
	}
}
