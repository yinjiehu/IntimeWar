using MechSquadShared;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad
{
	public class DemoIconCollection
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod]
		public static void InitIcons()
		{
			//var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Prefabs/Data/AttachmentBattlePrefabs.prefab");
			var prefab = Resources.Load<GameObject>("IconCollection");
			var instance = Object.Instantiate(prefab).GetComponent<IconSpriteSettingCollection>();
			instance.name = prefab.name;
			Object.DontDestroyOnLoad(instance);
			GlobalCache.Set("IconCollection", instance);
		}
	}

	public partial class GlobalCache
	{
		public static IconSpriteSettingCollection GetIcon()
		{
			object obj;
			if (GlobalCache.TryGet("IconCollection", out obj))
			{
				return (IconSpriteSettingCollection)obj;
			}
			return null;
		}
	}
}
