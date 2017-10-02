using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View.RoomCreate
{
	public class RoomMemberView : BaseView
	{
		[SerializeField]
		Haruna.UI.ListMemberUpdater _leftMemberList;
		[SerializeField]
		Haruna.UI.ListMemberUpdater _rightMemberList;

		[SerializeField]
		Button _startButton;

		float _elapsedTime;
		float _duration = 0.5f;

		public override void Show()
		{
			base.Show();
			RoomCreatePresenter.SetSimpleVehicleStatusToPhotonPlayer();
			RoomCreatePresenter.UpdateRoomMemberAction();
		}

		void Update()
		{
			_elapsedTime += Time.deltaTime;
			if (_elapsedTime > _duration)
			{
				_elapsedTime = 0;
				RoomCreatePresenter.UpdateRoomMemberAction();
			}
		}

		public struct Model
		{
			public string RoomName;
			public List<RoomMemberElement.Model> LeftMemberList;
			public List<RoomMemberElement.Model> RightMemberList;
			public bool ButtonInteractable;
		}

		public void BindToView(Model data)
		{
			_leftMemberList.OnListUpdate(data.LeftMemberList, (d, go, index) =>
			{
				var el = go.GetComponent<RoomMemberElement>();
				el.BindData(d);
			});
			_rightMemberList.OnListUpdate(data.RightMemberList, (d, go, index) =>
			{
				var el = go.GetComponent<RoomMemberElement>();
				el.BindData(d);
			});
			_startButton.interactable = data.ButtonInteractable;
		}
	}
}