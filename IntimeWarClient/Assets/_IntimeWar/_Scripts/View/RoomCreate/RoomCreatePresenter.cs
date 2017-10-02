using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MechSquad.View.RoomCreate
{
	public static class RoomCreatePresenter
	{
		public static void UpdateRoomListAction()
		{
			var rooms = PhotonNetwork.GetRoomList();
			ViewManager.Instance.GetView<LobbyView>().BindToView(
				rooms
				.Where(r => r.GetMode10RoomBattleState() == Mode10RoomStateEnum.Garage)
				.Select(r => r.Name).ToList());
		}

		public static string SeletedRoom;

		public static void UpdateRoomMemberAction()
		{
			if (!PhotonNetwork.inRoom)
				return;

			var playerlist = PhotonNetwork.playerList.OrderBy(p => p.ID).ToList();
			var model = new RoomMemberView.Model()
			{
				RoomName = PhotonNetwork.room.Name,
				LeftMemberList = new List<RoomMemberElement.Model>(),
				RightMemberList = new List<RoomMemberElement.Model>(),
				ButtonInteractable = PhotonNetwork.isMasterClient,
			};
			foreach (var p in playerlist)
			{
				var simpleStatus = p.GetSimpleVehicleStatusFromPhotonPlayer();
				if (simpleStatus == null)
					continue;

				simpleStatus.Self = p.ID == PhotonNetwork.player.ID;
				if (p.IsMasterClient)
					simpleStatus.PlayerNickName += " (Master)";
				if (simpleStatus != null)
				{
					if (p.GetUnitTeam() == 1)
						model.LeftMemberList.Add(simpleStatus);
					else if(p.GetUnitTeam() == 2)
						model.RightMemberList.Add(simpleStatus);
				}
			}
			ViewManager.Instance.GetView<RoomMemberView>().BindToView(model);
		}

		public static void SetSimpleVehicleStatusToPhotonPlayer()
		{
			var vehicleStatus = GlobalCache.GetPlayerStatus().GetCurrentSelected();
			var hash = new ExitGames.Client.Photon.Hashtable()
			{
				{ 0, vehicleStatus.VehicleID },
				{ 1, vehicleStatus.ActiveSlots.OrderBy(s => AssemblyUtil.GetOrderIndexFromSlotID(s.SlotID)).Select(s => s.AttachmentID).ToArray() },
				{ 2, vehicleStatus.PassiveSlots.OrderBy(s => s.SlotNo).Select(s => s.AttachmentID).ToArray() }
			};
			PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable()
			{
				{ "SimpleVehicleStatus", hash }
			});
		}
		public static RoomMemberElement.Model GetSimpleVehicleStatusFromPhotonPlayer(this PhotonPlayer photonPlayer)
		{
			object value;
			if (photonPlayer.CustomProperties.TryGetValue("SimpleVehicleStatus", out value))
			{
				var hash = (ExitGames.Client.Photon.Hashtable)value;
				var vehicleID = hash[0] as string;

				var activeAttachmentIcons = new List<Sprite>();
				foreach(var attachmentID in hash[1] as string[])
				{
					var attachment = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachmentID);
					activeAttachmentIcons.Add(GlobalCache.GetIcon().GetAttCategoryIcon(attachment.Category));
				}

				var vehicleSttings = GlobalCache.GetVehicleSettingsCollection().Get(vehicleID);
				var passiveAttachmentNames = new List<string>();
				passiveAttachmentNames.AddRange((hash[2] as string[]).Select(id => string.IsNullOrEmpty(id) ? "---" : TextForAttachment.GetNameSimple(id)));
				while (passiveAttachmentNames.Count < vehicleSttings.PassiveSlotsCount)
					passiveAttachmentNames.Add("---");

				return new RoomMemberElement.Model()
				{
					PlayerNickName = photonPlayer.NickName,
					MechSimpleName = TextForVehicle.GetNameSimple(vehicleID),
					ActiveSlotAttImages = activeAttachmentIcons,
					PassiveSlotAttNames = passiveAttachmentNames.ToArray(),
					CategoryText = "???",
					SPText = "???"
				};
			}
			return null;
		}

		public static void SwitchTeam()
		{
			var currentTeam = PhotonNetwork.player.GetUnitTeam();
			if (currentTeam == 1)
			{
				PhotonHelper.SetUnitTeam(2);
			}
			else
			{
				PhotonHelper.SetUnitTeam(1);
			}
			UpdateRoomMemberAction();
		}
	}
}