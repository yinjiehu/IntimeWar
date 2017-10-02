#if UNITY_EDITOR
using System.Linq;
using UnityEngine;

public class CreateEnvironmentPrefab : MonoBehaviour
{
	[SerializeField]
	Haruna.Inspector.InspectorButton _create;

	private void Create()
	{
		var allAssets = UnityEditor.AssetDatabase.GetAllAssetPaths();

		var root = new GameObject("Environment");
		root.transform.localPosition = Vector3.zero;
		root.transform.rotation = Quaternion.identity;

		int i = 0;
		foreach (Transform srcParent in transform)
		{
			i++;
			var targetParent = new GameObject(srcParent.name);
			targetParent.transform.SetParent(root.transform);
			targetParent.transform.localPosition = Vector3.zero;
			targetParent.transform.rotation = Quaternion.identity;

			var j = 0;
			foreach (Transform srcObj in srcParent)
			{
				j++;
				if (UnityEditor.EditorUtility.DisplayCancelableProgressBar("doing",
					string.Format("{0} / {1}", i, transform.childCount),
					(float)j / srcParent.childCount))
				{
					return;
				}

				var prefabName = srcObj.name;
				var lastSpaceIndex = prefabName.LastIndexOf(' ');
				if (lastSpaceIndex > 0)
				{
					prefabName = prefabName.Substring(0, lastSpaceIndex);
				}

				var assetPath = allAssets.FirstOrDefault(a => a.EndsWith(prefabName + ".prefab"));
				if (string.IsNullOrEmpty(assetPath))
				{
					Debug.LogErrorFormat("can not find prefab [{0}]", prefabName);
					continue;
				}

				var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
				var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				instance.name = prefabName;
				instance.transform.SetParent(targetParent.transform);
				instance.transform.position = srcObj.position;
				instance.transform.rotation = srcObj.rotation;
			}
		}
		UnityEditor.EditorUtility.ClearProgressBar();
	}

}
#endif