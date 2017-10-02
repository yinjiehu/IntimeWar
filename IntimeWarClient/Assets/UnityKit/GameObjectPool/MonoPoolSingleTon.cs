using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Haruna.Pool
{	
	public class MonoPoolSingleTon : MonoBehaviour
	{
		static MonoPoolSingleTon _instance;
		public static MonoPoolSingleTon Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				var go = new GameObject("MonoPoolSingleTon");
				_instance = go.AddComponent<MonoPoolSingleTon>();
				return _instance;
			}
		}

		Dictionary<int, ObjectPool> _poolMap = new Dictionary<int, ObjectPool>();

		public static PoolElement CreateFromPrefab(PoolElement prefab)
		{
			if (prefab == null)
				throw new Exception("Can not create pool element from null!");

			var ins = Instance.GetInstanceFromPool(prefab);
			Instance.SetInstanceToChild(ins, prefab.name);
			return ins;
		}

		public static void RecycleInstance(PoolElement ins)
		{
			if (_instance == null)
				return;

			ObjectPool pool;
			if (Instance._poolMap.TryGetValue(ins.PrefabInstanceID, out pool))
			{
				pool.AvaliableInstance.Enqueue(ins);
			}
		}

		PoolElement GetInstanceFromPool(PoolElement prefab)
		{
			var id = prefab.GetInstanceID();
			ObjectPool pool;
			if (!_poolMap.TryGetValue(id, out pool))
			{
				pool = new ObjectPool(prefab);
				_poolMap.Add(id, pool);
			}
			return pool.GetInstance();
		}
		
		public void SetInstanceToChild(PoolElement instance, string parentName)
		{
			if(instance.transform.parent == null || instance.transform.parent.name == parentName)
			{
				var parentTransform = transform.Find(parentName);
				if(parentTransform == null)
				{
					parentTransform = new GameObject(parentName).transform;
					parentTransform.SetParent(transform);
				}
				instance.transform.SetParent(parentTransform);
			}
		}
	}
}