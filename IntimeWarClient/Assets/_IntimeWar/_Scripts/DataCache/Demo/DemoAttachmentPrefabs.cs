using MechSquadShared;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad
{
	public class DemoAttachmentPrefabs
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod]
		public static void InitAttachmentPrefabs()
		{
			//var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Prefabs/Data/AttachmentBattlePrefabs.prefab");
			var prefab = Resources.Load<GameObject>("AttachmentBattlePrefabs");
			var instance = Object.Instantiate(prefab).GetComponent<AttachmentBattlePrefabs>();
			instance.name = prefab.name;
			Object.DontDestroyOnLoad(instance);
			GlobalCache.Set("AttachmentBattlePrefabs", instance);
		}
	}

	public partial class GlobalCache
	{
		public static AttachmentBattlePrefabs GetAttachmentBattlePrefabs()
		{
			object obj;
			if (GlobalCache.TryGet("AttachmentBattlePrefabs", out obj))
			{
				return (AttachmentBattlePrefabs)obj;
			}

			return null;
		}
	}
}
