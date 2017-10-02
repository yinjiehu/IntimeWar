using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyMechParameterElement : MonoBehaviour
	{
		[SerializeField]
		Text _label;

		[SerializeField]
		Text _content;
		
		public struct Model
		{
			public string Label;
			public string Content;
		}

		public void BindData(Model data)
		{
			_label.text = data.Label;
			_content.text = data.Content;
		}
	}
}