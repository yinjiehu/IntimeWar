using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad
{
	public class IconSpriteSettingCollection : MonoBehaviour
	{
		static IconSpriteSettingCollection _instance;
		public static IconSpriteSettingCollection Get()
		{
			if (_instance != null)
				return _instance;

#if UNITY_EDITOR
			//var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Prefabs/Data/Resources/IconCollection");
			var prefab = Resources.Load<IconSpriteSettingCollection>("IconCollection");
#else
			var prefab = Resources.Load<IconSpriteSettingCollection>("IconCollection");
#endif
			_instance = Instantiate(prefab).GetComponent<IconSpriteSettingCollection>();
			_instance.name = prefab.name;
			DontDestroyOnLoad(_instance);
			return _instance;
		}

		[SerializeField]
		string _path;

		[SerializeField]
		List<Sprite> _prefabs;
		
		public Sprite Get(string IconName, bool throwExceptionWhenNotFound = true)
		{
			for (var i = 0; i < _prefabs.Count; i++)
			{
				if (_prefabs[i].name == IconName)
					return _prefabs[i];
			}

			if(throwExceptionWhenNotFound)
				throw new Exception(string.Format("Can not find icon {0}", IconName));

			return null;
		}

		public Sprite GetAttPurposeIcon(string purposeType, bool throwExceptionWhenNotFound = true)
		{
			var iconName = "Icon_Attachment_Purpose_" + purposeType;
			return Get(iconName, throwExceptionWhenNotFound);
		}
		public Sprite GetAttCategoryIcon(string category, bool throwExceptionWhenNotFound = true)
		{
			var iconName = "Icon_Attachment_Category_" + category;
			return Get(iconName, throwExceptionWhenNotFound);
		}
		public Sprite GetAttButtonIcon(string category, bool throwExceptionWhenNotFound = true)
		{
			var iconName = "Icon_Attachment_Btn_" + category;
			return Get(iconName, throwExceptionWhenNotFound);
		}
		public Sprite GetVehicleIcon(string vehicleIcon, bool throwExceptionWhenNotFound = true)
		{
			var iconName = "Icon_Vehicle_Icon_" + vehicleIcon;
			return Get(iconName, throwExceptionWhenNotFound);
		}
		public Sprite GetVehicleImage(string vehicleIcon, bool throwExceptionWhenNotFound = true)
		{
			var iconName = "Icon_Vehicle_Image_" + vehicleIcon;
			return Get(iconName, throwExceptionWhenNotFound);
		}


#if UNITY_EDITOR
		[SerializeField]
		Haruna.Inspector.InspectorButton _reload;
		public void Reload()
		{
			_prefabs.Clear();

			var allPaths = UnityEditor.AssetDatabase.GetAllAssetPaths().Where(p => p.Contains(_path));

			foreach (var p in allPaths)
			{
				var go = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(p);
				if(go == null)
				{
					Debug.LogWarningFormat("Ignore {0}", p);
				}
				else
				{
					_prefabs.Add(go);
				}
			}

			_prefabs.Sort((p1, p2) => string.Compare(p1.name, p2.name));
		}
#endif
	}
}
