using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyMechDesignActiveSlotElement : MonoBehaviour
	{

		[SerializeField]
		GameObject _sizeAll;
		[SerializeField]
		Text _slotName;

		[Header("Empty Group")]
		[SerializeField]
		GameObject _empty;

		//[SerializeField]
		//Text _emptyDescription;

		[SerializeField]
		GameObject _purposeAll;
		[SerializeField]
		List<Image> _purposeIconList;
		//[SerializeField]
		//Haruna.UI.ListMemberUpdater _avaliableAttPurposeList;

		[Header("Attached Group")]
		[SerializeField]
		GameObject _attached;

		[SerializeField]
		Text _attachmentName;
		[SerializeField]
		Text _loadedAmmoDisplayName;
		[SerializeField]
		Text _loadedAmmoTotalCount;
		[SerializeField]
		Button _selectButton;
		[SerializeField]
		Image _AttachementIcon;

		string _slotID;

		public struct Model
		{
			public string SlotID;
			public string SlotName;
			public bool Attachable;

			public bool Show;
			public bool Attached;
			//public string EmptyDescription;
			public bool ShowSizeAndAvaliablePurpose;
			public int Size;
			public int AttachementSize;
			public List<bool> AvaliablePurposeList;

			public string AttachmentID;
			public string AttachmentFullName;
			public string LoadedAmmoName;
			public string LoadedAmmoTotalCount;
			public string PositionName;
			public Sprite AttachmentIcon;
		}

		public void BindData(Model data)
		{
			_slotID = data.SlotID;
			_slotName.text = data.SlotName;
			gameObject.SetActive(data.Show);
			_selectButton.interactable = data.Attachable;

			_empty.SetActive(!data.Attached);

			for (int i = 0; i < _sizeAll.transform.childCount; i++)
			{
				if(i< data.Size)
					_sizeAll.transform.GetChild(i).gameObject.SetActive(true);
				else
					_sizeAll.transform.GetChild(i).gameObject.SetActive(false);
			}

			for (int i = 0; i < data.Size; i++)
			{
				if (i < data.AttachementSize)
					_sizeAll.transform.GetChild(data.Size-1-i).GetComponent<Image>().color = Color.white;
				else
					_sizeAll.transform.GetChild(data.Size-1-i).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
			}

			_attached.SetActive(data.Attached);
			if (data.Attached)
			{
				_attachmentName.text = data.AttachmentFullName;
				_loadedAmmoDisplayName.text = data.LoadedAmmoName;
				_loadedAmmoTotalCount.text = data.LoadedAmmoTotalCount;

				_AttachementIcon.sprite = data.AttachmentIcon;
			}
			else if(data.Show)
			{
				for (int i = 0; i < data.AvaliablePurposeList.Count; i++)
				{
					if (data.AvaliablePurposeList[i])
					{
						_purposeIconList[i].color = new Color(0.7f, 0.77f, 0.77f, 0.8f);//gray
					}
					else 
					{
						_purposeIconList[i].color = new Color(0.4f, 0.1f, 0, 0.8f);// dark red
					}
				}
			}
		}

		public void OnClickSlot()
		{
			AssemblyPresenter.SelectedActiveSlotID = _slotID;
			AssemblyPresenter.ShowActiveAttachmentListAction();
		}
	}
}