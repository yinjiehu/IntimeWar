using System.Collections.Generic;

namespace IntimeWar
{
	public static class RandomUtil
    {
        public static T RandomGet<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
