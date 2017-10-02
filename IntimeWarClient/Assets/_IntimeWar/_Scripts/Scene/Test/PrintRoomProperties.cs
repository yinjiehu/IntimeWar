using UnityEngine;

public class PrintRoomProperties : MonoBehaviour
{
	[SerializeField]
	Haruna.Inspector.InspectorButton _currentRoom;

	[SerializeField]
	Haruna.Inspector.InspectorButton _allRoom;

	public void CurrentRoom()
	{
		if (PhotonNetwork.room == null)
			Debug.LogWarning("not in room");
		else
		{
			//var str = MechSquad.JsonUtil.Serialize(PhotonNetwork.room.CustomProperties);
			//Debug.Log(str);
		}
	}
	
	public void AllRoom()
	{
		var rooms = PhotonNetwork.GetRoomList();
		if(rooms.Length == 0)
		{
			Debug.LogWarning("no room!");
			return;
		}
		foreach (var room in PhotonNetwork.GetRoomList())
		{
			Debug.LogFormat("------------{0}--------------", room.Name);
			//if(room.CustomProperties == null)
			//	Debug.LogWarning("null");
			//else
			//	Debug.Log(MechSquad.JsonUtil.Serialize(room.CustomProperties));
		}
	}
}
