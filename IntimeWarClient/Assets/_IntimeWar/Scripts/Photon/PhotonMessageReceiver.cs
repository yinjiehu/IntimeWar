using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MechSquad
{
	public class PhotonMessageReceiver : MonoBehaviour
	{
		static PhotonMessageReceiver _instance;
		public static PhotonMessageReceiver Instance
		{
			get
			{
				if (_instance == null)
				{
					var go = new GameObject("PhotonMessageReceiver");
					_instance = go.AddComponent<PhotonMessageReceiver>();
				}
				return _instance;
			}
		}

		event Action _onConnectedToPhoton;
		public static event Action OnConnectedToPhoton { add { Instance._onConnectedToPhoton += value; } remove { if (_instance != null) _instance._onConnectedToPhoton -= value; } }
		public void OnConnectedToPhotonMethod()
		{
			if (_onConnectedToPhoton != null)
				_onConnectedToPhoton();
		}


		event Action _onLeftRoom;
		public static event Action OnLeftRoom { add { Instance._onConnectedToPhoton += value; } remove { if (_instance != null) _instance._onConnectedToPhoton -= value; } }
		public void OnLeftRoomMethod()
		{
			if (_onLeftRoom != null)
				_onLeftRoom();
		}


		event Action<PhotonPlayer> _onMasterClientSwitched;
		public static event Action<PhotonPlayer> OnMasterClientSwitched { add { Instance._onMasterClientSwitched += value; } remove { if (_instance != null) _instance._onMasterClientSwitched -= value; } }
		public void OnMasterClientSwitchedMethod(PhotonPlayer newMasterClient)
		{
			if (_onMasterClientSwitched != null)
				_onMasterClientSwitched(newMasterClient);
		}


		event Action<object[]> _onPhotonCreateRoomFailed;
		public static event Action<object[]> OnPhotonCreateRoomFailed { add { Instance._onPhotonCreateRoomFailed += value; } remove { if (_instance != null) _instance._onPhotonCreateRoomFailed -= value; } }
		public void OnPhotonCreateRoomFailedMethod(object[] codeAndMsg)
		{
			if (_onPhotonCreateRoomFailed != null)
				_onPhotonCreateRoomFailed(codeAndMsg);
		}

		event Action<object[]> _onPhotonJoinRoomFailed;
		public static event Action<object[]> OnPhotonJoinRoomFailed { add { Instance._onPhotonJoinRoomFailed += value; } remove { if (_instance != null) _instance._onPhotonJoinRoomFailed -= value; } }
		public void OnPhotonJoinRoomFailedMethod(object[] codeAndMsg)
		{
			if (_onPhotonJoinRoomFailed != null)
				_onPhotonJoinRoomFailed(codeAndMsg);
		}

		event Action _onCreatedRoom;
		public static event Action OnCreatedRoom { add { Instance._onCreatedRoom += value; } remove { if (_instance != null) _instance._onCreatedRoom -= value; } }
		public void OnCreatedRoomMethod()
		{
			if (_onCreatedRoom != null)
				_onCreatedRoom();
		}

		event Action _onJoinedLobby;
		public static event Action OnJoinedLobby { add { Instance._onJoinedLobby += value; } remove { if (_instance != null) _instance._onJoinedLobby -= value; } }
		public void OnJoinedLobbyMethod()
		{
			if (_onJoinedLobby != null)
				_onJoinedLobby();
		}

		event Action _onLeftLobby;
		public static event Action OnLeftLobby { add { Instance._onLeftLobby += value; } remove { if (_instance != null) _instance._onLeftLobby -= value; } }
		public void OnLeftLobbyMethod()
		{
			if (_onLeftLobby != null)
				_onLeftLobby();
		}

		event Action<DisconnectCause> _onFailedToConnectToPhoton;
		public static event Action<DisconnectCause> OnFailedToConnectToPhoton { add { Instance._onFailedToConnectToPhoton += value; } remove { if (_instance != null) _instance._onFailedToConnectToPhoton -= value; } }
		public void OnFailedToConnectToPhotonMethod(DisconnectCause cause)
		{
			if (_onFailedToConnectToPhoton != null)
				_onFailedToConnectToPhoton(cause);
		}

		event Action _onDisconnectedFromPhoton;
		public static event Action OnDisconnectedFromPhoton { add { Instance._onDisconnectedFromPhoton += value; } remove { if (_instance != null) _instance._onDisconnectedFromPhoton -= value; } }
		public void OnDisconnectedFromPhotonMethod()
		{
			if (_onDisconnectedFromPhoton != null)
				_onDisconnectedFromPhoton();
		}

		event Action<DisconnectCause> _onConnectionFail;
		public static event Action<DisconnectCause> OnConnectionFail { add { Instance._onConnectionFail += value; } remove { if (_instance != null) _instance._onConnectionFail -= value; } }
		public void OnConnectionFailMethod(DisconnectCause cause)
		{
			if (_onConnectionFail != null)
				_onConnectionFail(cause);
		}


		event Action<PhotonMessageInfo> _onPhotonInstantiate;
		public static event Action<PhotonMessageInfo> OnPhotonInstantiate { add { Instance._onPhotonInstantiate += value; } remove { if (_instance != null) _instance._onPhotonInstantiate -= value; } }
		public void OnPhotonInstantiateMethod(PhotonMessageInfo info)
		{
			if (_onPhotonInstantiate != null)
				_onPhotonInstantiate(info);
		}

		event Action _onReceivedRoomListUpdate;
		public static event Action OnReceivedRoomListUpdate { add { Instance._onReceivedRoomListUpdate += value; } remove { if (_instance != null) _instance._onReceivedRoomListUpdate -= value; } }
		public void OnReceivedRoomListUpdateMethod()
		{
			if (_onReceivedRoomListUpdate != null)
				_onReceivedRoomListUpdate();
		}
		
		event Action _onJoinedRoom;
		public static event Action OnJoinedRoom { add { Instance._onJoinedRoom += value; } remove { if (_instance != null) _instance._onJoinedRoom -= value; } }
		public void OnJoinedRoomMethod()
		{
			if (_onJoinedRoom != null)
				_onJoinedRoom();
		}


		event Action<PhotonPlayer> _onPhotonPlayerConnected;
		public static event Action<PhotonPlayer> OnPhotonPlayer { add { Instance._onPhotonPlayerConnected += value; } remove { if (_instance != null) _instance._onPhotonPlayerConnected -= value; } }
		public void OnPhotonPlayerConnectedMethod(PhotonPlayer newPlayer)
		{
			if (_onPhotonPlayerConnected != null)
				_onPhotonPlayerConnected(newPlayer);
		}


		event Action<PhotonPlayer> _onPhotonPlayerDisconnected;
		public static event Action<PhotonPlayer> OnPhotonPlayerDisconnected { add { Instance._onPhotonPlayerDisconnected += value; } remove { if (_instance != null) _instance._onPhotonPlayerDisconnected -= value; } }
		public void OnPhotonPlayerDisconnectedMethod(PhotonPlayer otherPlayer)
		{
			if (_onPhotonPlayerDisconnected != null)
				_onPhotonPlayerDisconnected(otherPlayer);
		}
		
		event Action<object[]> _onPhotonRandomJoinFailed;
		public static event Action<object[]> OnPhotonRandomJoinFailed { add { Instance._onPhotonRandomJoinFailed += value; } remove { if (_instance != null) _instance._onPhotonRandomJoinFailed -= value; } }
		public void OnPhotonRandomJoinFailedMethod(object[] codeAndMsg)
		{
			if (_onPhotonRandomJoinFailed != null)
				_onPhotonRandomJoinFailed(codeAndMsg);
		}

		event Action _onConnectedToMaster;
		public static event Action OnConnectedToMaster { add { Instance._onConnectedToMaster += value; } remove { if (_instance != null) _instance._onConnectedToMaster -= value; } }
		public void OnConnectedToMasterMethod()
		{
			if (_onConnectedToMaster != null)
				_onConnectedToMaster();
		}
		
		event Action _onPhotonMaxCccuReached;
		public static event Action OnPhotonMaxCccuReached { add { Instance._onPhotonMaxCccuReached += value; } remove { if (_instance != null) _instance._onPhotonMaxCccuReached -= value; } }
		public void OnPhotonMaxCccuReachedMethod()
		{
			if (_onPhotonMaxCccuReached != null)
				_onPhotonMaxCccuReached();
		}

		event Action<Hashtable> _onPhotonCustomRoomPropertiesChanged;
		public static event Action<Hashtable> OnPhotonCustomRoomPropertiesChanged { add { Instance._onPhotonCustomRoomPropertiesChanged += value; } remove { if (_instance != null) _instance._onPhotonCustomRoomPropertiesChanged -= value; } }
		public void OnPhotonCustomRoomPropertiesChangedMethod(Hashtable propertiesThatChanged)
		{
			if (_onPhotonCustomRoomPropertiesChanged != null)
				_onPhotonCustomRoomPropertiesChanged(propertiesThatChanged);
		}

		event Action<object[]> _onPhotonPlayerPropertiesChanged;
		public static event Action<object[]> OnPhotonPlayerPropertiesChanged { add { Instance._onPhotonPlayerPropertiesChanged += value; } remove { if (_instance != null) _instance._onPhotonPlayerPropertiesChanged -= value; } }
		public void OnPhotonPlayerPropertiesChangedMethod(object[] playerAndUpdatedProps)
		{
			if (_onPhotonPlayerPropertiesChanged != null)
				_onPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
		}

		event Action _onUpdatedFriendList;
		public static event Action OnUpdatedFriendList { add { Instance._onUpdatedFriendList += value; } remove { if (_instance != null) _instance._onUpdatedFriendList -= value; } }
		public void OnUpdatedFriendListMethod()
		{
			if (_onUpdatedFriendList != null)
				_onUpdatedFriendList();
		}

		event Action<string> _onCustomAuthenticationFailed;
		public static event Action<string> OnCustomAuthenticationFailed { add { Instance._onCustomAuthenticationFailed += value; } remove { if (_instance != null) _instance._onCustomAuthenticationFailed -= value; } }
		public void OnCustomAuthenticationFailedMethod(string debugMessage)
		{
			if (_onCustomAuthenticationFailed != null)
				_onCustomAuthenticationFailed(debugMessage);
		}

		event Action<Dictionary<string, object>> _onCustomAuthenticationResponse;
		public static event Action<Dictionary<string, object>> OnCustomAuthenticationResponse { add { Instance._onCustomAuthenticationResponse += value; } remove { if (_instance != null) _instance._onCustomAuthenticationResponse -= value; } }
		public void OnCustomAuthenticationResponseMethod(Dictionary<string, object> data)
		{
			if (_onCustomAuthenticationResponse != null)
				_onCustomAuthenticationResponse(data);
		}

		event Action<OperationResponse> _onWebRpcResponse;
		public static event Action<OperationResponse> OnWebRpcResponse { add { Instance._onWebRpcResponse += value; } remove { if (_instance != null) _instance._onWebRpcResponse -= value; } }
		public void OnWebRpcResponseMethod(OperationResponse response)
		{
			if (_onWebRpcResponse != null)
				_onWebRpcResponse(response);
		}

		event Action<object[]> _onOwnershipRequest;
		public static event Action<object[]> OnOwnershipRequest { add { Instance._onOwnershipRequest += value; } remove { if (_instance != null) _instance._onOwnershipRequest -= value; } }
		public void OnOwnershipRequestMethod(object[] viewAndPlayer)
		{
			if (_onOwnershipRequest != null)
				_onOwnershipRequest(viewAndPlayer);
		}

		event Action _onLobbyStatisticsUpdate;
		public static event Action OnLobbyStatisticsUpdate { add { Instance._onLobbyStatisticsUpdate += value; } remove { if (_instance != null) _instance._onLobbyStatisticsUpdate -= value; } }
		public void OnLobbyStatisticsUpdateMethod()
		{
			if (_onLobbyStatisticsUpdate != null)
				_onLobbyStatisticsUpdate();
		}

		event Action<PhotonPlayer> _onPhotonPlayerActivityChanged;
		public static event Action<PhotonPlayer> OnPhotonPlayerActivityChanged { add { Instance._onPhotonPlayerActivityChanged += value; } remove { if (_instance != null) _instance._onPhotonPlayerActivityChanged -= value; } }
		public void OnPhotonPlayerActivityChangedMethod(PhotonPlayer otherPlayer)
		{
			if (_onPhotonPlayerActivityChanged != null)
				_onPhotonPlayerActivityChanged(otherPlayer);
		}

		event Action<object[]> _onOwnershipTransfered;
		public static event Action<object[]> OnOwnershipTransfered { add { Instance._onOwnershipTransfered += value; } remove { if (_instance != null) _instance._onOwnershipTransfered -= value; } }
		public void OnOwnershipTransferedMethod(object[] viewAndPlayers)
		{
			if (_onOwnershipTransfered != null)
				_onOwnershipTransfered(viewAndPlayers);
		}

	}
}