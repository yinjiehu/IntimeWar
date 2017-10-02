using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace Haruna.Inspector
{
	[CustomPropertyDrawer(typeof(ListByNameDefineAttribute))]
	public class ListByNameDefinePropertyDrawer : PropertyDrawer
	{
		private const float _splitWithLabel = .70f;
		private const float _splitWithOutLabel = .55f;
		private const int _margin = 1;
		
		public override void OnGUI(Rect region, SerializedProperty property, GUIContent label)
		{
			//return base.Edit(region, label, element, attribute, metadata);

			//EditorGUI.PrefixLabel(region, label);
			var value = DrawStringPopField(region, property.stringValue, label.text, ((ListByNameDefineAttribute)attribute).Filter);
			if (!string.IsNullOrEmpty(value))
				property.stringValue = value;

			//float splitWidth = region.width * (label.text == "" ? SplitWithOutLabel : SplitWithLabel);
			//Rect leftRect = new Rect(region.xMin, region.yMin, splitWidth, region.height);
			//Rect rightRect = new Rect(region.xMin + splitWidth + Margin, region.yMin,
			//	region.width - splitWidth - Margin, region.height);
			
			//EditorGUI.PropertyField(leftRect, property);

			//var lines = System.IO.File.ReadAllLines(Application.dataPath + "/_Prefabs/Data/NameDefine/NameDefine.txt");
			
   //         var filter = ((ListByNameDefineAttribute)attribute).Filter;
			//List<string> values = GetNameDefines(filter);
			
			//values.Insert(0, "ManualInput");

			//var actualValueList = values.Select(v => v.Split('/').Last()).ToList();
			//var index = actualValueList.FindIndex(v => v == property.stringValue);
			//if (index < 0) index = 0;

			//var menus = values.Select(v => new GUIContent(v.StartsWith("/") ? v.Substring(1) : v)).ToArray();
			//index = EditorGUI.Popup(rightRect, index, menus);
			//if (index != 0)
			//	property.stringValue = actualValueList[index];
		}

		public static string DrawStringPopField(Rect region, string value, string label, List<string> filter)
		{
			float splitWidth = region.width * (label == "" ? _splitWithOutLabel : _splitWithLabel);
			Rect leftRect = new Rect(region.xMin, region.yMin, splitWidth + 15, region.height);
			Rect rightRect = new Rect(region.xMin + splitWidth + _margin, region.yMin,
				region.width - splitWidth - _margin, region.height);

			value = EditorGUI.TextField(leftRect, label, value);
						
			List<string> values = GetNameDefines(filter);
			values.Insert(0, "ManualInput");

			var actualValueList = values.Select(v => v.Split('/').Last()).ToList();
			var index = actualValueList.FindIndex(v => v == value);
			if (index < 0) index = 0;

			var menus = values.Select(v => new GUIContent(v.StartsWith("/") ? v.Substring(1) : v)).ToArray();
			index = EditorGUI.Popup(rightRect, index, menus);
			if (index != 0)
				value = actualValueList[index];

			return value;
		}

		static List<string> GetNameDefines(List<string> filters)
		{
			var values = new List<string>();
			HashSet<string> lines = new HashSet<string>();
			lines.UnionWith(System.IO.File.ReadAllLines(Application.dataPath + "/_Prefabs/Data/NameDefine/NameDefine.txt"));
			lines.UnionWith(System.IO.File.ReadAllLines(Application.dataPath + "/_Prefabs/Data/NameDefine/NameDefineAuto.txt"));

			if (filters.Count == 1)
			{
				var temp = lines.Where(v => !string.IsNullOrEmpty(v) && v.StartsWith(filters[0])).Select(v => v.Substring(filters[0].Length));
				values.AddRange(temp);
			}
			else if(filters.Count > 1)
			{
				var temp = lines.Where(v => !string.IsNullOrEmpty(v) && filters.Any(f => v.StartsWith(f)));
				values.AddRange(temp);
			}
			else
			{
				var temp = lines.Where(v => !string.IsNullOrEmpty(v));
				values.AddRange(temp);
			}
			return values;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}