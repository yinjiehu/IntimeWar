using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Haruna.Inspector
{
	[System.Serializable]
	public class InspectorButton
	{
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(InspectorButton))]
	public class InspectorButtonPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if(GUI.Button(position, property.displayName))
			{
				var method = property.serializedObject.targetObject.GetType()
					.GetMethod(property.name.Substring(1), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
				if(method != null)
				{
					if(method.IsStatic)
						method.Invoke(null, null);
					else
						method.Invoke(property.serializedObject.targetObject, null);
				}
				else
				{
					Debug.LogError("can not find method " + property.name.Substring(1));
				}
			}
		}
	}
#endif
}