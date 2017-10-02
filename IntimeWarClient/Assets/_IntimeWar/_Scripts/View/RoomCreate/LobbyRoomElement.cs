using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View.RoomCreate
{
	public class LobbyRoomElement : MonoBehaviour
	{
		[SerializeField]
		Text _roomName;

		public void BindData(string roomName)
		{
			_roomName.text = roomName;
		}

		public void OnClickRoom()
		{
			RoomCreatePresenter.SeletedRoom = _roomName.text;
		}
	}
}