using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class HudView : BaseView
	{
		class HudPrefabPool
		{
			public HudInstance Prefab;
			public Queue<HudInstance> AvaliableInstance;
		}
		Dictionary<int, HudPrefabPool> _poolMap = new Dictionary<int, HudPrefabPool>();


		public HudInstance CreateFromPrefab(HudInstance prefab)
		{
			var ins = GetInstanceFromPool(prefab);
			ins.name = prefab.name;
			return ins;
		}

		public void RecycleInstance(HudInstance ins)
		{
			HudPrefabPool pool;
			if (_poolMap.TryGetValue(ins.PrefabInstanceID, out pool))
			{
				pool.AvaliableInstance.Enqueue(ins);
			}
		}

		HudInstance GetInstanceFromPool(HudInstance prefab)
		{
			var id = prefab.GetInstanceID();
			HudPrefabPool pool;
			if (!_poolMap.TryGetValue(id, out pool))
			{
				pool = new HudPrefabPool()
				{
					Prefab = prefab,
					AvaliableInstance = new Queue<HudInstance>()
				};
				_poolMap.Add(id, pool);
			}

			if (pool.AvaliableInstance.Count != 0)
			{
				return pool.AvaliableInstance.Dequeue();
			}

			var ins = Instantiate(prefab);
			ins.transform.SetParent(transform);
			ins.transform.localScale = prefab.transform.localScale;

			ins.View = this;
			ins.Prefab = prefab;
			return ins;
		}
	}
}