using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntimeWar
{
	//[CreateAssetMenu]
	public class Texts
	{
		static Texts _staticInstance;
		public static Texts Instance
		{
			get
			{
				if (_staticInstance == null)
					_staticInstance = new Texts();
				return _staticInstance;
			}
		}

		Dictionary<string, string> _map = new Dictionary<string, string>();

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void DemoCreate()
		{
			Instance._map.Clear();
		}
		
		public static void Add(string name, string value)
		{
			if (Instance._map.ContainsKey(name))
			{
				Debug.LogErrorFormat("key {0} is already exist. \n{1} \n{2}", name, value, Instance._map[name]);
				return;
			}
			Instance._map.Add(name, value);
		}

		public static string Get(string name)
		{
			string ret;
			if (Instance._map.TryGetValue(name, out ret))
			{
				return ret;
			}
			return "404 " + name;
		}
	}
}
