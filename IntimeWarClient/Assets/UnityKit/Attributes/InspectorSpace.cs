using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Haruna.Inspector
{
	[Serializable]
	public class InspectorSpace
	{
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(InspectorSpace))]
	public class SpacingPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		}
	}
#endif
}