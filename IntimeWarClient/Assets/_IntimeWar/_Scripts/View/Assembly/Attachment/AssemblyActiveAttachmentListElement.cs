using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyActiveAttachmentListElement : MonoBehaviour
	{
		[SerializeField]
		Text _displayName;

		[Header("ControlType")]
		[SerializeField]
		GameObject _controlTypeR;
		[SerializeField]
		GameObject _controlTypeS;
		[SerializeField]
		GameObject _controlTypeG;
		[SerializeField]
		GameObject _controlTypeA;


		[Header("Common")]
		[SerializeField]
		Image _attachmentIcon;
		[SerializeField]
		Image _purposeIcon;
		[SerializeField]
		Image _purposeBg;
		[SerializeField]
		Text _category;
		[SerializeField]
		GameObject _size;


		[Header("Parameters")]
		[SerializeField]
		Haruna.UI.ListMemberUpdater _parameters;

		[SerializeField]
		Button _onClickButton;

		public struct Model
		{
			public string AttachmentID;
			public string SlotID;

			public string ControlType;

			public Sprite AttachmentIcon;
			public Sprite PurposeIcon;
			public string AttachmentDisplayName;
			public string Category;
			public int SizeNum;
			public bool Purpose;
			public bool Size;

			public List<AssemblyActiveAttachmentParameterElement.Model> ParameterList;

			public bool Attachable;
		}

		string _attachmentID;
		
		public void BindData(Model data)
		{
			_attachmentID = data.AttachmentID;

			_displayName.text = data.AttachmentDisplayName;

			//_controlTypeR.SetActive(data.ControlType == AttachmentControlTypeEnum.Rapid.ToString());
			//_controlTypeS.SetActive(data.ControlType == AttachmentControlTypeEnum.Single.ToString());
			//_controlTypeG.SetActive(data.ControlType == AttachmentControlTypeEnum.Grenade.ToString());
			//_controlTypeA.SetActive(data.ControlType == AttachmentControlTypeEnum.Artillery.ToString());

			for (int i = 0; i < _size.transform.childCount; i++)
			{
				if (i < data.SizeNum)
					_size.transform.GetChild(i).gameObject.SetActive(true);
				else
					_size.transform.GetChild(i).gameObject.SetActive(false);
			}

			if (data.Purpose)
				_purposeBg.color = new Color(0.7f, 0.77f, 0.77f, 0.8f);//gray
			else
				_purposeBg.color = new Color(0.82f, 0.275f, 0.082f);//orange

			if(data.Size)
			{
				for (int i = 0; i < data.SizeNum; i++)
				{
					_size.transform.GetChild(i).GetComponent<Image>().color = new Color(0.7f, 0.77f, 0.77f, 0.8f);//gray
				}
			}
			else
			{
				for (int i = 0; i < data.SizeNum; i++)
				{
					_size.transform.GetChild(i).GetComponent<Image>().color = new Color(0.82f, 0.275f, 0.082f);//orange
				}
			}

			_category.text = data.Category;
			_attachmentIcon.sprite = data.AttachmentIcon;
			_purposeIcon.sprite = data.PurposeIcon;
			_parameters.OnListUpdate(data.ParameterList, (d, go, index) =>
			{
				var el = go.GetComponent<AssemblyActiveAttachmentParameterElement>();
				el.BindData(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = d.Label,
					Number = d.Number
				});
			});


			_onClickButton.interactable = data.Attachable;
		}

		public void OnClickAttach()
		{
			AssemblyPresenter.SelectedActiveAttachmentID = _attachmentID;
			AssemblyPresenter.ShowAmmoListAction();
		}
	}
}