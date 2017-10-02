﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View.RoomCreate
{
	public class LobbyView : BaseView
	{
		[SerializeField]
		Haruna.UI.ListMemberUpdater _roomList;
		[SerializeField]
		InputField _input;
		[SerializeField]
		GameObject _emptyDisplay;

		float _elapsedTime;
		float _duration = 0.5f;

		public override void Show()
		{
			base.Show();
			RoomCreatePresenter.UpdateRoomListAction();
		}

		void Update()
		{
			_elapsedTime += Time.deltaTime;
			if (_elapsedTime > _duration)
			{
				_elapsedTime = 0;
				RoomCreatePresenter.UpdateRoomListAction();
			}
		}

		public void BindToView(List<string> rooms)
		{
			if (rooms.Count == 0)
			{
				_emptyDisplay.SetActive(true);
				_roomList.gameObject.SetActive(false);
			}
			else
			{
				_emptyDisplay.SetActive(false);
				_roomList.gameObject.SetActive(true);
				_roomList.OnListUpdate(rooms, (r, go, index) =>
				{
					go.GetComponent<LobbyRoomElement>().BindData(r);
				});
			}
		}

		public string GetInputRoomName()
		{
			return _input.text;
		}
	}
}