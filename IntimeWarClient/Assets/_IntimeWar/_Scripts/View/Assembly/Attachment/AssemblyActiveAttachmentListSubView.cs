using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyActiveAttachmentListSubView : BaseView
	{
		[SerializeField]
		GameObject _emptyDisplay;
		[SerializeField]
		GameObject _scrollRoot;
		[SerializeField]
		Text _positionName;
		[SerializeField]
		Text _weaponName;
		[SerializeField]
		List<Image> _purposeList;
		[SerializeField]
		GameObject _size;

		[SerializeField]
		Haruna.UI.ListMemberUpdater _list;
		
		public struct Model
		{
			public string PositionName;
			public string WeaponName;
			public List<bool> PurposeList;
			public int SlotSize;
			public int WeaponSize;

			public List<AssemblyActiveAttachmentListElement.Model> ElementList;
		}

		public void BindToView(Model data)
		{
			_positionName.text = data.PositionName;
			_weaponName.text = data.WeaponName;

			for (int i = 0; i < data.PurposeList.Count; i++)
			{
				if (data.PurposeList[i])
				{
					_purposeList[i].color = new Color(0.7f, 0.77f, 0.77f, 0.8f);//gray
				}
				else
				{
					_purposeList[i].color = new Color(0.4f, 0.1f, 0, 0.8f);// dark red
				}
			}

			for (int i = 0; i < _size.transform.childCount; i++)
			{
				if (i < data.SlotSize)
					_size.transform.GetChild(i).gameObject.SetActive(true);
				else
					_size.transform.GetChild(i).gameObject.SetActive(false);
			}
			for (int i = 0; i < data.SlotSize; i++)
			{ 
				if (i < data.WeaponSize)
					_size.transform.GetChild(data.SlotSize - 1 - i).GetComponent<Image>().color = Color.white;
				else
					_size.transform.GetChild(data.SlotSize - 1 - i).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
			}
			

			if (data.ElementList == null || data.ElementList.Count == 0)
			{
				_emptyDisplay.SetActive(true);
				_scrollRoot.SetActive(false);
			}
			else
			{
				_emptyDisplay.SetActive(false);
				_scrollRoot.SetActive(true);

				_list.OnListUpdate(data.ElementList, (d, go, index) =>
				{
					var el = go.GetComponent<AssemblyActiveAttachmentListElement>();
					el.BindData(d);
				});
			}
		}

		public void OnClickUnload()
		{
			AssemblyPresenter.UnloadActiveAttachmentAction();
		}
	}
}