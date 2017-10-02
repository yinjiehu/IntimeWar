using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Haruna.Inspector
{
	public class HarunaInspectAttribute : PropertyAttribute
	{
		public bool HideInEditorMode { set; get; }
		public bool HideInRunningMode { set; get; }

		public bool Disabled { set; get; }

		public string ShowIf { set; get; }
		public float Indent { set; get; }
		public float BeforeSpace { set; get; }
		public float AfterSpace { set; get; }
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(HarunaInspectAttribute))]
	public class HarunaInspectPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var attr = attribute as HarunaInspectAttribute;
			var height = attr.BeforeSpace;
			height += attr.AfterSpace;

			if (attr.HideInEditorMode && !Application.isPlaying)
				return  height;
			if (attr.HideInRunningMode && Application.isPlaying)
				return  height;

			if (!string.IsNullOrEmpty(attr.ShowIf))
			{
				var obj = property.serializedObject.targetObjects;
				var directiHostObj = obj[obj.Length - 1];

				var showIfField = directiHostObj.GetType().GetField(attr.ShowIf, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (showIfField != null)
				{
					var value = showIfField.IsStatic ? showIfField.GetValue(null) : showIfField.GetValue(directiHostObj);
					if (!(bool)value)
						return height;
				}

				var showIfProperty = directiHostObj.GetType().GetProperty(attr.ShowIf, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (showIfProperty != null)
				{
					var value = showIfProperty.GetValue(directiHostObj, null);
					if (!(bool)value)
						return height;
				}

				var showIfMethod = directiHostObj.GetType().GetMethod(attr.ShowIf, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (showIfMethod != null)
				{
					var value = showIfMethod.Invoke(directiHostObj, null);
					if (!(bool)value)
						return height;
				}
			}

			height += EditorGUI.GetPropertyHeight(property, label, true);

			return  height;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var attr = attribute as HarunaInspectAttribute;
			position.y += attr.BeforeSpace;
			
			if (attr.HideInEditorMode && !Application.isPlaying)
				return;
			if (attr.HideInRunningMode && Application.isPlaying)
				return;

			if (!string.IsNullOrEmpty(attr.ShowIf))
			{
				var obj = property.serializedObject.targetObjects;
				var directiHostObj = obj[obj.Length - 1];

				var showIfField = directiHostObj.GetType().GetField(attr.ShowIf, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (showIfField != null)
				{
					var value = showIfField.IsStatic ? showIfField.GetValue(null) : showIfField.GetValue(directiHostObj);
					if (!(bool)value)
						return;
				}

				var showIfProperty = directiHostObj.GetType().GetProperty(attr.ShowIf, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (showIfProperty != null)
				{
					var value = showIfProperty.GetValue(directiHostObj, null);
					if (!(bool)value)
						return;
				}

				var showIfMethod = directiHostObj.GetType().GetMethod(attr.ShowIf, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (showIfMethod != null)
				{
					var value = showIfMethod.Invoke(directiHostObj, null);
					if (!(bool)value)
						return;
				}
			}

			position.x += attr.Indent;
			position.width -= attr.Indent;

			EditorGUI.BeginDisabledGroup(attr.Disabled);
			EditorGUI.PropertyField(position, property, label, true);
			EditorGUI.EndDisabledGroup();
			//EditorGUILayout.PropertyField(_onForwardToEndingProp);
		}
	}
#endif
}