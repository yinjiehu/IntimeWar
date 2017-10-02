using UnityEngine;
using System.Collections.Generic;
using System;
using ExitGames.Client.Photon;
using System.Collections;
using MechSquadShared;
using System.Linq;

namespace MechSquad
{
	public class PhotonCustomEventReceiver : MonoBehaviour
	{
		HashSet<string> _processedEventIds = new HashSet<string>();
		Dictionary<string, Delegate> _registedDelegate = new Dictionary<string, Delegate>();
		Dictionary<string, Action<object>> _registedInnerDelegate = new Dictionary<string, Action<object>>();

		static PhotonCustomEventReceiver _instance;
        public static PhotonCustomEventReceiver Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				_instance = FindObjectOfType<PhotonCustomEventReceiver>();
				if (_instance != null)
					return _instance;

				//var go = new GameObject("PhotonCustomEventReceiver");
				//_instance = go.AddComponent<PhotonCustomEventReceiver>();

				return _instance;
			}
		}

        void Awake()
		{
			PhotonNetwork.OnEventCall += OnPhotonCustomEvent;
		}

		void OnDestroy()
		{
			_instance = null;
			PhotonNetwork.OnEventCall -= OnPhotonCustomEvent;
		}

		public static void RegistCustomEvent(string typeName, Action<object> onEvent)
		{
			if (Instance._registedInnerDelegate.ContainsKey(typeName))
				Debug.LogErrorFormat(Instance, "Photon custom event {0} has already been registed.", typeName);
			else
				Instance._registedInnerDelegate.Add(typeName, onEvent);
		}

		public static void RegistCustomEvent<T>(Action<T> onEvent) where T : PhotonCustomEvent
		{
			var typeName = typeof(T).FullName;
			if (Instance._registedDelegate.ContainsKey(typeName))
				Debug.LogErrorFormat(Instance, "Photon custom event {0} has already been registed.", typeof(T).Name);
			else
				Instance._registedDelegate.Add(typeName, onEvent);
		}
		public static void UnregistCustomEvent(string typeName)
		{
			if (_instance != null)
				_instance._registedDelegate.Remove(typeName);
		}
		public static void UnregistCustomEvent<T>() where T : PhotonCustomEvent
		{
			if(_instance != null)
				_instance._registedDelegate.Remove(typeof(T).FullName);
		}

		void OnPhotonCustomEvent(byte eventCode, object content, int senderId)
		{
			if (gameObject.activeInHierarchy)
			{
				switch (eventCode)
				{
					//case (byte)PhotonServerEvCodeEnum.NewPlayerStartBattle:
					//	{
					//		OnNewPlayerJoinInAndStartBattle((int)content);
					//	}
					//	break;
					case (byte)PhotonServerEvCodeEnum.InstantiateUnit:
						{
							var data = (ExitGames.Client.Photon.Hashtable)content;
							OnInstantiateUnit(data);
						}
						break;
					case (byte)PhotonServerEvCodeEnum.DestroyUnit:
						{
							OnDestroyUnit((int)content, senderId);
						}
						break;

					case (byte)PhotonServerEvCodeEnum.PlayerDisconnected:

						break;
					case (byte)PhotonServerEvCodeEnum.CustomEvent:
						{
							var data = (object[])content;
							var evName = (string)data[0];
							Delegate d1;
							Action<object> d2;
							if (_registedDelegate.TryGetValue(evName, out d1))
							{
								var type = Type.GetType(evName);
								//d1.DynamicInvoke(JsonUtil.Deserialize((string)data[1], type));
							}
							else if(_registedInnerDelegate.TryGetValue(evName, out d2))
							{
								d2(data[1]);
							}
							else
							{
                                //if(evName != "IronFury.Models.Game.S2CActorsLeftRoom" && evName != "IronFury.Models.Game.C2SAreaCaptureUpdateInAreaPlayers")
								if(MechSquadPreference.LogLevel >= LogLevelEnum.Trace)
								    Debug.LogWarningFormat("Unexpected custom event. evcode : {0}. content : {1}", evName, content);
							}
						}
						break;
				}
			}
		}

//		public static void OnNewPlayerJoinInAndStartBattle(int actorID)
//		{
//#if !NATIVEEDITOR_BUILD
//			var photonPlayer = PhotonNetwork.networkingPeer.GetPlayerWithId(actorID);
//			if(photonPlayer == null)
//			{
//				Debug.LogErrorFormat("On new player join in but can not find the actor id {0}", actorID);
//				return;
//			}
//			var vehicleID = photonPlayer.GetVehicleID();
//			var paintID = photonPlayer.GetPaintID();
//			var battlePrefabSettings = Config.VehiclePrefabSettings.Get(vehicleID);
//			battlePrefabSettings.LoadBattlePrefabAsync(unit =>
//			{
//				Debug.LogFormat("On new player {0} join. load prefab success. {1}", photonPlayer.UserId, vehicleID);
//			});
//			var materialSettings = battlePrefabSettings.GetPaint(paintID);
//			materialSettings.LoadMaterialAsync(m =>
//			{
//				Debug.LogFormat("On new player {0} join. load material success. {1}", photonPlayer.UserId, vehicleID);
//			});
//#endif
//		}

		public void OnInstantiateUnit(ExitGames.Client.Photon.Hashtable data)
		{
			var rpcParam = (object[])data[(byte)InstantiateUnitEventKey.Parameters];
			var allViewIds = (int[])rpcParam[0];
			var rootViewID = allViewIds[0];

			var timeStamp = (long)data[(byte)InstantiateUnitEventKey.EventTimeStamp];
			var eventID = rootViewID.ToString() + timeStamp.ToString();
			if (_processedEventIds.Contains(eventID))
			{
				if(MechSquadPreference.LogLevel > LogLevelEnum.Debug)
					Debug.LogFormat("Receive instantiate event is already processed. RootViewID:{0}, TimeStamp:{1}", rootViewID, timeStamp);
				return;
			}
			
			//if (MechSquadPreference.LogLevel > LogLevelEnum.Debug)
			//	Debug.LogFormat("Receive instantiate event. RootViewID:{0}, TimeStamp:{1}. Parameters : {2}",
					//rootViewID, timeStamp, JsonUtil.SerializeArgs(rpcParam));

			_processedEventIds.Add(eventID);
			//if (MechSquadPreference.LogLevel > LogLevelEnum.Debug)
			//	Debug.LogFormat("ProcessedEventIds : {0}", JsonUtil.SerializeArgs(_processedEventIds.ToArray()));

			var rootViewActorID = rootViewID / PhotonNetwork.MAX_VIEW_IDS;
			var senderID = rootViewActorID == 0 ? PhotonNetwork.masterClient.ID : rootViewActorID;

			if (rootViewActorID != 0)
			{
				var photonPlayer = PhotonHelper.GetPlayer(rootViewActorID);
				if (photonPlayer == null)
				{
					Debug.LogErrorFormat("Receive instantiate event but the player {0} is not exist!", senderID);
					return;
				}
			}
			//Debug.LogFormat("Receive instantiate event. RootViewID:{0}. TimeStamp:{1}", rootViewID, timeStamp);

			//var createrMethodName = (string)data[(byte)InstantiateUnitEventKey.CreaterMethodName];
			//var otherParameters = (object[])data[(byte)InstantiateUnitEventKey.OtherParameters];
			var ev = new EventData();
			ev.Code = PunEvent.RPC;
			ev.Parameters = new Dictionary<byte, object>();
			ev.Parameters.Add(ParameterCode.ActorNr, senderID);
			ev.Parameters.Add(ParameterCode.Data, data);

			PhotonNetwork.networkingPeer.OnEvent(ev);
			
			if (data.ContainsKey((byte)InstantiateUnitEventKey.CurrentSyncStatus))
			{
				var allViewSyncData = (ExitGames.Client.Photon.Hashtable[])data[(byte)InstantiateUnitEventKey.CurrentSyncStatus];
				if (MechSquadPreference.LogLevel > LogLevelEnum.Debug)
					Debug.LogFormat("-----Receive instantiate event with sync data. root view {0}. total count {1}", rootViewID, allViewSyncData.Length);

				StartCoroutine(SynchronizeView(allViewSyncData, senderID,rootViewID));

				if (MechSquadPreference.LogLevel > LogLevelEnum.Debug)
					Debug.LogFormat("----- end instantiate event with sync data {0}", rootViewID);
				//allSyncEvent = allViewSyncData.Select(syncData =>
				//{
				//	var evData = new EventData();
				//	evData.Code = PunEvent.SendSerialize;
				//	evData.Parameters = new Dictionary<byte, object>();
				//	evData.Parameters.Add(ParameterCode.ActorNr, rootViewActorID);
				//	evData.Parameters.Add(ParameterCode.Data, syncData);
				//	return evData;
				//}).ToArray();

				//SynchronizeView(allViewSyncData, rootViewActorID);
			}
			//var rpcParamWithSynchronizeData = new object[rpcParam.Length + 1];
			//rpcParamWithSynchronizeData[rpcParam.Length] = allSyncEvent;
			//Array.Copy(rpcParam, rpcParamWithSynchronizeData, rpcParam.Length);
			//data[(byte)InstantiateUnitEventKey.Parameters] = rpcParamWithSynchronizeData;
		}

		
        IEnumerator SynchronizeView(ExitGames.Client.Photon.Hashtable[] allViewSyncData, int actorID,int rootViewID)
		{
			PhotonView view = null;
			int retryTimes = 0;
			while (view == null && retryTimes < 300)
			{
				retryTimes++;
				yield return null;
				PhotonNetwork.networkingPeer.photonViewList.TryGetValue(rootViewID, out view);
			}

			if (view != null)
			{
				for(var i = 0; i< allViewSyncData.Length; i++)
				{
					var value = allViewSyncData[i];
#if UNITY_EDITOR
					//Debug.LogFormat("Sync Data: {0}", JsonUtil.Serialize(value));
#endif

					var evData = new EventData();
					evData.Code = PunEvent.SendSerialize;
					evData.Parameters = new Dictionary<byte, object>();
					evData.Parameters.Add(ParameterCode.ActorNr, actorID);
					evData.Parameters.Add(ParameterCode.Data, value);

					PhotonNetwork.networkingPeer.OnEvent(evData);
				};
			}
        }

        void OnDestroyUnit(int viewID, int actorID)
		{
			var target = PhotonView.Find(viewID);
			if(target == null)
			{
				Debug.LogWarningFormat("Can not find view {0} to destroy. message from {1}", viewID, actorID);
			}
			else
			{
				Destroy(target.gameObject);
			}
		}
	}
}
