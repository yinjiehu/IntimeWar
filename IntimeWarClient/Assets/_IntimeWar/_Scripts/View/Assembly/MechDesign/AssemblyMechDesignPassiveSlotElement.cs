using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyMechDesignPassiveSlotElement : MonoBehaviour
	{
		[SerializeField]
		Text _displayName;

		byte _slotNo;

		public void BindData(byte slotNo, string data)
		{
			_slotNo = slotNo;
			_displayName.text = data;
		}

		public void OnClickPassiveSlot()
		{
			AssemblyPresenter.SelectedPassiveSlotNo = _slotNo;
			AssemblyPresenter.ShowPassiveAttachmentListAction();
		}
	}
}