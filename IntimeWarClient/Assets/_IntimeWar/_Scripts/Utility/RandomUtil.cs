using System;
using System.Collections;
using System.Collections.Generic;

namespace MechSquad
{
	public static class RandomUtil
	{
		static Random _random;

		public static T RandomGet<T>(this IList<T> list)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		//public static object RandomGet(this IList list)
		//{
		//	return list[UnityEngine.Random.Range(0, list.Count)];
		//}
	}
}
