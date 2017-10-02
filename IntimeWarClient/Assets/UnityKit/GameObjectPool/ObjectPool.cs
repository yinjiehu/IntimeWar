using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Haruna.Pool
{
	public class ObjectPool
	{
		PoolElement _prefab;
		public PoolElement Prefab { get { return _prefab; } }

		Queue<PoolElement> _avaliableInstance;
		public Queue<PoolElement> AvaliableInstance { get { return _avaliableInstance; } }

		public ObjectPool(PoolElement prefab)
		{
			_prefab = prefab;
			_avaliableInstance = new Queue<PoolElement>();
		}

		public PoolElement GetInstance()
		{
			if (_avaliableInstance.Count == 0)
			{
				var ins = UnityEngine.Object.Instantiate(_prefab);
				ins.Init(this, _prefab.GetInstanceID());
				return ins;
			}

			return _avaliableInstance.Dequeue();
		}

		public void RecycleInstance(PoolElement el)
		{
			_avaliableInstance.Enqueue(el);
		}
	}
}