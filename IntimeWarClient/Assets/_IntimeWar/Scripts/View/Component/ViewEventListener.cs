using System;
using System.Linq;
using UnityEngine;

namespace View
{
	public abstract class ViewEventListener : MonoBehaviour
	{
		public abstract void AddListener(Action<object> action);
		public abstract void RemoveListener(Action<object> action);
	}
}