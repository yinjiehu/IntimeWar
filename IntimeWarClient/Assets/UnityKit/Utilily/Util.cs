using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Haruna.Utility
{
	public static class Util
	{
		public static Camera GetUICamera()
		{
			return GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
		}

		public static Vector3 Vector3LerpBySpeed(Vector3 from, Vector3 to, float speed, float deltaTime)
		{
			if (from == to)
				return to;

			var distance = Vector3.Distance(from, to);
			var needTime = distance / speed;
			if (needTime <= deltaTime)
				return to;

			return Vector3.Lerp(from, to, deltaTime / needTime);
		}

		public static T RandomMember<T>(this IList<T> list)
		{
			var t = UnityEngine.Random.Range(0, list.Count);
			return list[t];
		}

		public static TCollection GetCollection<TKey, TCollection, TValue>
			(this Dictionary<TKey, TCollection> dictionary, TKey key) where TCollection : ICollection<TValue>, new()
		{
			TCollection t;
			if (!dictionary.TryGetValue(key, out t))
			{
				t = new TCollection();
				dictionary.Add(key, t);
			}
			return t;
		}

		public static TCollection AddElmentToCollection<TKey, TCollection, TValue>
			(this Dictionary<TKey, TCollection> dictionary, TKey key, TValue value) where TCollection : ICollection<TValue>, new()
		{
			TCollection t;
			if (!dictionary.TryGetValue(key, out t))
			{
				t = new TCollection();
				dictionary.Add(key, t);
			}
			t.Add(value);

			return t;
		}

		public static float GetHorizontalDistance(Vector3 p1, Vector3 p2)
		{
			p1.y = 0; p2.y = 0;
			return Vector3.Distance(p1, p2);
		}
		public static Vector3 GetHorizontalDirection(Vector3 from, Vector3 to)
		{
			var d = to - from;
			d.y = 0;
			return d;
		}
		public static Vector3 ChangeY(this Vector3 v, float y = 0)
		{
			v.y = y;
			return v;
		}

		public static bool RaycastFirst<T>(Vector3 startPosition, Vector3 direction, float range, LayerMask layer,
			out RaycastHit hit, out T receiver) where T : Component
		{
			var hits = Physics.RaycastAll(
				startPosition,
				direction,
				range,
				layer,
				QueryTriggerInteraction.Collide).OrderBy(h => Vector3.Distance(startPosition, h.point)).ToList();

			receiver = null;
			hit = new RaycastHit();
			for (var i = 0; i < hits.Count; i++)
			{
				receiver = hits[i].collider.GetComponent<T>();
				if (receiver != null)
				{
					hit = hits[i];
					break;
				}
			}

			return receiver != null;
		}

		public static Color ChangeColorAlpha(Color origin, float alpha)
		{
			origin.a = alpha;
			return origin;
		}
	}
}
