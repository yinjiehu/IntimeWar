using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	[RequireComponent(typeof(Button))]
	public class ButtonClickEventListener : NullParamEventListener
	{
		private void Start()
		{
			GetComponent<Button>().onClick.AddListener(SendEvent);
		}
	}
}