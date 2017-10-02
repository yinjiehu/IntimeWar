using UnityEngine;

namespace MechSquad.View
{
	public class ActiveAttachmentBtnView : BaseView
	{
		[SerializeField]
		GameObject[] _buttons;

		public GameObject[] GetButtons()
		{
			return _buttons;
		}

		//public void InitAllButton()
		//{
		//	for (var i = 0; i < _buttons.Length; i++)
		//	{
		//		_buttons[i].GetComponent<ActiveAttachmentBtnViewAction>().InitAttachment();
		//	}
		//}

	}
}
