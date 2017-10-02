using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyActiveAttachmentParameterElement : MonoBehaviour
	{
		[SerializeField]
		Text _label;
		[SerializeField]
		Text _number;

		public struct Model
		{
			public string Label;
			public string Number;
		}

		public void BindData(Model data)
		{
			_label.text = data.Label;
			_number.text = data.Number;
		}
	}
}