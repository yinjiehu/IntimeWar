using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Haruna.Pool
{
	public interface IRecyclable
	{
		void OnRecycle();
	}

	public class PoolElement : MonoBehaviour
	{
		int _prefabInstanceID;
		public int PrefabInstanceID { get { return _prefabInstanceID; } }

		ObjectPool _pool;
		public ObjectPool Pool { get { return _pool; } }

		IRecyclable[] _recyclableList;

		public void Init(ObjectPool pool, int prefabInstanceID)
		{
			_pool = pool;
			_prefabInstanceID = prefabInstanceID;
			_recyclableList = GetComponentsInChildren<IRecyclable>(true);
		}

		public void RecycleThis()
		{
			for (var i = 0; i < _recyclableList.Length; i++)
			{
				_recyclableList[i].OnRecycle();
			}
			_pool.RecycleInstance(this);
		}
	}
}