using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Linq;
using System.Collections.Generic;

namespace  MechSquad.Battle
{
	public class PlayerSpawnerMode10 : MonoBehaviour, ISpawner
	{
		public string Name { get { return name; } }

		[SerializeField]
		List<BattleUnit> _playerPrefab;
		[SerializeField]
		int _level;
		[SerializeField]
		Transform[] _spawnPositionA;
		[SerializeField]
		Transform[] _spawnPositionB;

		[SerializeField]
		float _randomDistance = 50;

		[SerializeField]
		string _afterFsmEvent;
		//public BattleUnit _spawnedUnit;

		public void SpawnUnit()
		{
			if (PhotonNetwork.offlineMode)
				PhotonHelper.CreateOfflinePhotonPlayerProperties();

			var seqNo = PhotonNetwork.playerList.Where(p => p.GetTeam() == PhotonNetwork.player.GetTeam()).OrderBy(p => p.ID).ToList().FindIndex(p => p.ID == PhotonNetwork.player.ID);

			var team = PhotonNetwork.player.GetUnitTeam();
			var reverse = PhotonNetwork.offlineMode ? false : PhotonNetwork.room.GetMode10SpawnPositionReverse();
			Vector3 centerPosition = (team == Team.A && !reverse) || (team == Team.B && reverse) ? _spawnPositionA[seqNo].position : _spawnPositionB[seqNo].position;

			//var sameTeamUnits = UnitManager.Instance.GetSameTeamUnits(team).ToList();
			//if (sameTeamUnits.Count() != 0)
			//{ 
			//	var allyUnit = sameTeamUnits.RandomGet();
			//	centerPosition = allyUnit.Model.position;
			//}
			var randomAngle = UnityEngine.Random.Range(0, 360f);
			//var positionVar = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward * UnityEngine.Random.Range(0, _randomDistance);
			//var position = centerPosition + positionVar;
			var rotation = Quaternion.Euler(0, -randomAngle, 0);

			var viewID = PhotonNetwork.AllocateViewID();
			if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace))
				Debug.LogFormat("------ Titan fall Bug----- SpawnStart viewID {0} decide position {1}", viewID, centerPosition);
			PhotonCustomEventSender.RaiseInstantiateUnitEvent(
				this.GetPhotonView(), "OnInstantiateUnit", new int[] { viewID }, PhotonNetwork.player.GetUnitTeam(), PhotonNetwork.player.GetVehicleID(), centerPosition, rotation);
		}

		[PunRPC]
		void OnInstantiateUnit(int[] viewID, byte team, string vehicleID, Vector3 position, Quaternion rotation)
		{
			var actorID = viewID[0] / PhotonNetwork.MAX_VIEW_IDS;
			var photonPlayer = PhotonHelper.GetPlayer(actorID);

			var vehicleSettings = GlobalCache.GetVehicleSettingsCollection().Get(vehicleID);
			var prefab = _playerPrefab.Find(temp => temp.name == vehicleSettings.BattlePrefabName);
			if (prefab == null)
			{
				Debug.LogFormat("Can not find vehicle prefab for vehicle {0} for prefab name {1}", vehicleID, vehicleSettings.BattlePrefabName);
				return;
			}

			var p = Instantiate(prefab);
			p.name = vehicleID;

			var view = p.GetComponent<PhotonView>();
			view.viewID = viewID[0];

			p.transform.position = Vector3.zero;
			p.transform.rotation = Quaternion.identity;
			p.Model.position = position;
			p.Model.rotation = rotation;

			p.Init(new BattleUnit.UnitCreateArgs()
			{
				Team = team,
				Level = _level,
				Tag = "Player",
				InitialParameter = PhotonHelper.GetInitialParameter(photonPlayer)
			});
			
			UnitManager.Instance.AddPlayerUnit(p);

			if (p.IsPlayerForThisClient)
			{
				if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace))
					Debug.LogFormat("------ Titan fall Bug----- On instantiate view {0}, vehicle {1}. position {2} ", viewID[0], vehicleID, position);

				var turretAb = p.GetAbility<IMainFireControlSystem>();
				if (turretAb == null)
				{
					Debug.LogErrorFormat("Can not get IMainTurret ability from unit!");
					return;
				}

				turretAb.SetAsCameraFollow();
				Camera.main.GetComponent<Deftly.DeftlyCamera>().ApplyTargetPositionImmidiatly();

				UnitManager.Instance.SetThisClientPlayerUnit(p);

				if (!string.IsNullOrEmpty(_afterFsmEvent))
				{
					if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace))
						Debug.LogFormat("------ Titan fall Bug----- After spwan set titan fall {0}:{1}. decide position {2}", PhotonNetwork.player.NickName, p.name, p.Model.position);

					//p.Model.position = p.Model.position.ChangeY(0);
					p.Fsm.SendEvent(_afterFsmEvent);
				}
			}

			p.DisableAI();
		}
	}
}