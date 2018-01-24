using UnityEngine;
using YJH.Unit;
using System.Collections.Generic;

namespace  MechSquad.Battle
{
	public class CommonSpawner : MonoBehaviour, ISpawner
	{
		public string Name { get { return name; } }
		[SerializeField]
		string _tag;
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
				var positionVar = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward * UnityEngine.Random.Range(0, _randomDistance);
				var position = _spawnPosition.RandomMember().position + positionVar;
				var rotation = Quaternion.Euler(0, -randomAngle, 0);

				SpawnerManager.Instance.SendSpawnEvent(Name, "OnInstantiateUnit", viewID, position, rotation);
			}
		}

		[PunRPC]
		void OnInstantiateUnit(int viewID, Vector3 position, Quaternion rotation)
		{
			_spawnedUnits.Add(viewID);

			var p = Instantiate(_prefab);
			p.name = _prefab.name;

			var view = p.GetComponent<PhotonView>();
			view.viewID = viewID;

			p.transform.position = Vector3.zero;
			p.transform.rotation = Quaternion.identity;
			p.Model.position = position;
			p.Model.rotation = rotation;

			p.Init(new BattleUnit.UnitCreateArgs()
			{
				Team = Team.C,
				Level = _level,
				Tag = _tag,
				InitialParameter = UnitInitialParameter.Create(_prefab.name)
			});

			UnitManager.Instance.AddUnit(p);
			

			if (PhotonNetwork.isMasterClient && !string.IsNullOrEmpty(_afterFsmEvent))
			{
				p.Fsm.SendEvent(_afterFsmEvent);
			}
		}
	}
}