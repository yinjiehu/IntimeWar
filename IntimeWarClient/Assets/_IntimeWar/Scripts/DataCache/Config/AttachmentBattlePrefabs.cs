using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntimeWar
{
	public class AttachmentBattlePrefabs : MonoBehaviour
	{
		static AttachmentBattlePrefabs _instance;
		public static AttachmentBattlePrefabs Get()
		{
			if (_instance != null)
				return _instance;

#if UNITY_EDITOR
			var prefab = Resources.Load<AttachmentBattlePrefabs>("AttachmentPrefabs");
			//var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Prefabs/Data/AttachmentPrefabs");
#else
			var prefab = Resources.Load<AttachmentBattlePrefabs>("AttachmentPrefabs");
#endif
			_instance = Instantiate(prefab).GetComponent<AttachmentBattlePrefabs>();
			_instance.name = prefab.name;
			DontDestroyOnLoad(_instance);

			return _instance;
		}

		[SerializeField]
		string _path;

		[SerializeField]
		List<GameObject> _prefabs;
		
		public GameObject Get(string prefabName, bool throwExceptionWhenNotFound = true)
		{
			for (var i = 0; i < _prefabs.Count; i++)
			{
				if (_prefabs[i].name == prefabName)
					return _prefabs[i];
			}

			if(throwExceptionWhenNotFound)
				throw new Exception(string.Format("Can not find attachment battle prefab {0}", prefabName));

			return null;
		}

#if UNITY_EDITOR
		[SerializeField]
		Haruna.Inspector.InspectorButton _reload;
		public void Reload()
		{
			_prefabs.Clear();

			var allPaths = UnityEditor.AssetDatabase.GetAllAssetPaths().Where(p => p.Contains(_path));

			foreach (var p in allPaths)
			{
				var go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(p);
				if(go == null)
				{
					Debug.LogWarningFormat("Ignore {0}", p);
				}
				else
				{
					_prefabs.Add(go);
				}
			}
		}
#endif
	}
}
