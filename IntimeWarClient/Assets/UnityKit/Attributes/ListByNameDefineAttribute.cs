using System;
using System.Collections.Generic;
using UnityEngine;

namespace Haruna.Inspector
{
	public class ListByNameDefineAttribute : PropertyAttribute
	{
		public List<string> Filter { private set; get; }

		public ListByNameDefineAttribute()
		{
			Filter = new List<string>();
		}
		public ListByNameDefineAttribute(params string[] filter)
		{
			Filter = new List<string>();
			Filter.AddRange(filter);
		}
	}
	
    public class StringDisableAndSetByPropertyPath : PropertyAttribute
    {
        public string PropertyPath { private set; get; }
        public StringDisableAndSetByPropertyPath(string propertyPath)
        {
            PropertyPath = propertyPath;
        }
    }
}
