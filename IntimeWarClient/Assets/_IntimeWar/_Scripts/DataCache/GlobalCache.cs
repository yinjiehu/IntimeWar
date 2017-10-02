using System.Collections;
using UnityEngine;
using System;

namespace MechSquad
{
	public partial class GlobalCache : MonoBehaviour
	{
		static GlobalCache _instance;

        public static GlobalCache Instance
        {
            get { return Get(); }
        }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static GlobalCache Get()
		{
			if (_instance != null)
				return _instance;

			_instance = new GameObject("GlobalCache").AddComponent<GlobalCache>();
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
