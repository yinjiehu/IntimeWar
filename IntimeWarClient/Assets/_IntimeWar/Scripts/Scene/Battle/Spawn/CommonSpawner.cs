using UnityEngine;
using YJH.Unit;
using System.Collections.Generic;
using Haruna.Utility;

namespace  IntimeWar.Battle
{
	public class CommonSpawner : MonoBehaviour, ISpawner
	{
		public string Name { get { return name; } }
		[SerializeField]
		string _tag;
        [SerializeField]
        byte _team;
        [SerializeField]
		BattleUnit _prefab;
		[SerializeField]
		int _level;
		[SerializeField]
		Transform[] _spawnPosition;
		[SerializeField]
		float _randomDistance = 50;

		[SerializeField]
		int _maxCountInScene;
		[SerializeField]
		int _totalCount;

		[SerializeField]
		string _afterFsmEvent;

		List<int> _spawnedUnits = new List<int>();

		float _elapsedSeconds;
		float _recheckInterval = 1;

		private void Awake()
		{
			enabled = false;
		}
		
		public void SpawnUnit()
		{
			enabled = true;
		}

		private void Update()
		{
			if (!PhotonNetwork.offlineMode && !PhotonNetwork.isMasterClient)
				return;

			_elapsedSeconds += Time.deltaTime;
			if(_elapsedSeconds > _recheckInterval)
			{
				_elapsedSeconds = 0;
				RecheckSpawnedUnits();
			}
		}

		void RecheckSpawnedUnits()
		{
			_spawnedUnits.RemoveAll(p => PhotonView.Find(p) == null);

			if(_spawnedUnits.Count < _maxCountInScene)
			{
				var viewID = PhotonNetwork.AllocateSceneViewID();

				var randomAngle = UnityEngine.Random.Range(0, 360f);
				var positionVar = Quaternion.Euler(0, 0, randomAngle) * Vector3.forward * UnityEngine.Random.Range(0, _randomDistance);
				var position = _spawnPosition.RandomMember().position + positionVar;
                position.z = 0;
				SpawnerManager.Instance.SendSpawnEvent(Name, "OnInstantiateUnit", viewID, position);
			}
		}

		[PunRPC]
		void OnInstantiateUnit(int viewID, Vector3 position)
		{
			_spawnedUnits.Add(viewID);

			var p = Instantiate(_prefab);
			p.name = _prefab.name;

			var view = p.GetComponent<PhotonView>();
			view.viewID = viewID;

			p.transform.position = Vector3.zero;
			p.transform.rotation = Quaternion.identity;
			p.Model.position = position;
			p.Model.rotation = Quaternion.identity;

			p.Init(new BattleUnit.UnitCreateArgs()
			{
				Team = _team,
				Level = _level,
				Tag = _tag,
				InitialParameter = UnitInitialParameter.Create("Player_1_1")
			});

			UnitManager.Instance.AddUnit(p);

            p.EnableAI();

			if (PhotonNetwork.isMasterClient && !string.IsNullOrEmpty(_afterFsmEvent))
			{
				p.Fsm.SendEvent(_afterFsmEvent);
			}
		}
	}
}