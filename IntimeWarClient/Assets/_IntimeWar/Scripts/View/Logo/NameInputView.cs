using UnityEngine;
using UnityEngine.UI;
using View;

namespace IntimeWar.View
{
	public class NameInputView : BaseView
	{
		[SerializeField]
		InputField _nameInput;

		public void SetName(string playerName)
		{
			_nameInput.text = playerName;
		}

		public string GetName()
		{
			return _nameInput.text;
		}
	}
}
