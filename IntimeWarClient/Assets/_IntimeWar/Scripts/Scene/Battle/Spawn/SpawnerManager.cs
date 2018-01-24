using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using System.Reflection;
using YJH.Unit;
using IntimeWar;

namespace IntimeWar.Battle
{
	public interface ISpawner
	{
		string Name { get; }

		void SpawnUnit();
	}

	public class SpawnerManager : MonoBehaviour
	{
		static SpawnerManager _instance;
		public static SpawnerManager Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				_instance = FindObjectOfType<SpawnerManager>();
				if (_instance == null)
					throw new UnityException("Can not find spawner manager!");

				return _instance;
			}
		}

		Dictionary<string, ISpawner> _map = new Dictionary<string, ISpawner>();

		IPlayerRevivePositionPolicy _playerSpawnPositionPolicy;
		public static IPlayerRevivePositionPolicy PlayerRevivePositionPolicy { get { return Instance._playerSpawnPositionPolicy; } }

		private void Awake()
		{
			_instance = this;

			var sps = GetComponentsInChildren<ISpawner>();
			for (var i = 0; i < sps.Length; i++)
			{
				_map.Add(sps[i].Name, sps[i]);
			}
			_playerSpawnPositionPolicy = GetComponentInChildren<IPlayerRevivePositionPolicy>();
		}

		public ISpawner Get(string name)
		{
			ISpawner ret;
			if (_map.TryGetValue(name, out ret))
			{
				return ret;
			}

			throw new UnityException("Can not spawner by name " + name);
		}

		private void Start()
		{
			PhotonMessageReceiver.OnPhotonCustomRoomPropertiesChanged += OnRoomPropertiesChanged;
		}
		private void OnDestroy()
		{
			PhotonMessageReceiver.OnPhotonCustomRoomPropertiesChanged -= OnRoomPropertiesChanged;
		}

		bool _instantiateEnabled;

		public void EnableSpawnSynchronization()
		{
			_instantiateEnabled = true;

			if (!PhotonNetwork.offlineMode)
				CheckInstantiateEvent(PhotonNetwork.room.CustomProperties);
		}

		private void OnRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable changedProperties)
		{
			if (_instantiateEnabled)
			{
				CheckInstantiateEvent(changedProperties);
			}
		}

		HashSet<int> _processedEventIDs = new HashSet<int>();

		void CheckInstantiateEvent(ExitGames.Client.Photon.Hashtable properties)
		{
			using (var itr = properties.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					var k = itr.Current.Key;
					var v = itr.Current.Value;

					if (k is string && ((string)k).StartsWith(RoomPropertyKey.UnitSpawnPrefix))
					{
						if (v is object[])
						{
							var data = (object[])v;

							var viewID = (int)data[0];
							if (viewID != 0 && !_processedEventIDs.Contains(viewID))
							{
								_processedEventIDs.Add(viewID);
								var spawnerName = data[1] as string;
								var methodName = data[2] as string;
								var parameters = data[3] as object[];

								CallSpawner(spawnerName, methodName, parameters);
							}
						}
					}
				}
			}
		}

		void CallSpawner(string spawnerName, string methodName, object[] parameters)
		{
			var viewID = parameters[0];
			var spawner = Get(spawnerName);

			if (spawner == null)
			{
				Debug.LogErrorFormat("Can not find spawne in instantiate event. spawner {0}, method {1}, view id {2}", spawnerName, methodName, viewID);
				return;
			}

			var method = spawner.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				Debug.LogErrorFormat("Can not find method in instantiate event. spawner {0}, method {1}, view id {2}", spawnerName, methodName, viewID);
				return;
			}

			try
			{
				method.Invoke(spawner, parameters);
			}
			catch (Exception e)
			{
				Debug.LogErrorFormat("Error when invoke method at instantiate event. spawner {0}, method {1}, id {2}", spawnerName, methodName, viewID);
				Debug.LogException(e);
			}
		}

		public void SendSpawnEvent(string spawnerName, string methodName, params object[] parameters)
		{
			if (!PhotonNetwork.offlineMode)
			{
				var viewID = (int)parameters[0];
				var data = new object[parameters.Length + 3];
				data[0] = viewID;
				data[1] = spawnerName;
				data[2] = methodName;
				data[3] = parameters;

				var spawnKey = RoomPropertyKey.UnitSpawnPrefix + viewID;
				PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
				{
					{ spawnKey, data }
				});
			}
			else
			{
				CallSpawner(spawnerName, methodName, parameters);
			} 
		}

		public void SendPermanentSyncDataForAbility(int viewID, string abilityID, object data)
		{
			if (!PhotonNetwork.offlineMode)
			{
				var syncKey = string.Concat(RoomPropertyKey.UnitSyncPrefix, viewID, "_", abilityID);
                if(PhotonNetwork.room != null)
                {
                    PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                    {
                        { syncKey, data }
                    });
                }
			}
		}

		public object GetPermanentSyncDataByAbility(int viewID, string abilityID)
		{
			if (!PhotonNetwork.offlineMode)
			{
				var syncKey = string.Concat(RoomPropertyKey.UnitSyncPrefix, viewID, "_", abilityID);
				var properties = PhotonNetwork.room.CustomProperties;
				using (var itr = properties.GetEnumerator())
				{
					while (itr.MoveNext())
					{
						var k = itr.Current.Key;
						var v = itr.Current.Value;

						if (k is string && (string)k == syncKey && v is ExitGames.Client.Photon.Hashtable)
						{
							return (ExitGames.Client.Photon.Hashtable)v;
						}
					}
				}
			}
			return null;
		}

		public Dictionary<string, object> GetPermanentSyncDataForAllAbility(int viewID)
		{
			if (!PhotonNetwork.offlineMode)
			{
				var ret = new Dictionary<string, object>();
				var syncKeyPrefix = string.Concat(RoomPropertyKey.UnitSyncPrefix, viewID, "_");
				var properties = PhotonNetwork.room.CustomProperties;
				using (var itr = properties.GetEnumerator())
				{
					while (itr.MoveNext())
					{
						var k = itr.Current.Key as string;
						if (k.StartsWith(syncKeyPrefix))
						{
							var abilityID = k.Substring(syncKeyPrefix.Length);
							ret.Add(abilityID, itr.Current.Value);
						}
					}
				}
				return ret;
			}
			return null;
		}


		public void RemoveUnitSpawnAndSyncData(int viewID)
		{
			if (!PhotonNetwork.offlineMode)
			{
				var data = new object[1];
				data[0] = viewID;

				var spawnKey = RoomPropertyKey.UnitSpawnPrefix + viewID;
				var syncKey = RoomPropertyKey.UnitSyncPrefix + viewID;
				PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
				{
					{ spawnKey, 0 },
					{ syncKey, 0 },
				});
			}
		}

		//public void ClearAllUnitData()
		//{
		//	if (!PhotonNetwork.offlineMode && PhotonNetwork.isMasterClient)
		//	{
		//		var table = new ExitGames.Client.Photon.Hashtable();

		//		var properties = PhotonNetwork.room.CustomProperties;
		//		using (var itr = properties.GetEnumerator())
		//		{
		//			while (itr.MoveNext())
		//			{
		//				if (itr.Current.Key is string)
		//				{
		//					var key = itr.Current.Key as string;

		//					if (key.StartsWith(MechSquadShared.RoomPropertyKey.UnitSpawnPrefix))
		//					{
		//						table.Add(key, 0);
		//					}
		//					else if (key.StartsWith(MechSquadShared.RoomPropertyKey.UnitSyncPrefix))
		//					{
		//						table.Add(key, 0);
		//					}
		//				}
		//			}
		//		}
		//	}
		//}
	}
}