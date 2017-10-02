using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View.RoomCreate
{
	public class RoomMemberElement : MonoBehaviour
	{
		[SerializeField]
		Text _nickName;
		[SerializeField]
		Text _mechSimpleName;
		[SerializeField]
		Haruna.UI.ListMemberUpdater _activeSlots;
		[SerializeField]
		Haruna.UI.ListMemberUpdater _passiveSlots;
		[SerializeField]
		GameObject _switchTeamArrow;
		[SerializeField]
		Text _categoryText;
		[SerializeField]
		Text _spText;

		public class Model
		{
			public bool Self;
			public string PlayerNickName;
			public string MechSimpleName;
			public List<Sprite> ActiveSlotAttImages;
			public string[] PassiveSlotAttNames;
			public string CategoryText;
			public string SPText;
		}

		public void BindData(Model data)
		{
			_nickName.text = data.PlayerNickName;
			_mechSimpleName.text = data.MechSimpleName;
			_categoryText.text = data.CategoryText;
			_spText.text = data.SPText;
			_activeSlots.OnListUpdate(data.ActiveSlotAttImages, (s, go, Index) =>
			{
				go.GetComponent<Image>().sprite = s;
			});
			_passiveSlots.OnListUpdate(data.PassiveSlotAttNames, (s, go, Index) =>
			{
				go.GetComponentInChildren<Text>().text = s;
			});
			_switchTeamArrow.SetActive(data.Self);
		}

		public void OnClickSwitch()
		{
			RoomCreatePresenter.SwitchTeam();
		}
	}
}