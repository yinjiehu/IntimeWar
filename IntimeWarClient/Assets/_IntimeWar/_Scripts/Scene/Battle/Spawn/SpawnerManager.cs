using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;

namespace MechSquad.Battle
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
	}
}