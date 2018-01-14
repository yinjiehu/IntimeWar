using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Linq;
using System.Collections.Generic;

namespace  MechSquad.Battle
{
	public class PlayerSpawner : MonoBehaviour, ISpawner
	{
		public string Name { get { return name; } }

		[SerializeField]
		List<BattleUnit> _playerPrefab;
		[SerializeField]
		Transform _spawnPositionA;
		[SerializeField]
		Transform _spawnPositionB;

		[SerializeField]
		string _afterFsmEvent;

		public void SpawnUnit()
		{
			if (PhotonNetwork.offlineMode)
				PhotonHelper.CreateOfflinePhotonPlayerProperties();

			var seqNo = PhotonNetwork.playerList.Where(p => p.GetTeam() == PhotonNetwork.player.GetTeam()).OrderBy(p => p.ID).ToList().FindIndex(p => p.ID == PhotonNetwork.player.ID);

			var team = PhotonNetwork.player.GetUnitTeam();
			Vector3 centerPosition = team == Team.A ? _spawnPositionA.position : _spawnPositionB.position;
            
			var randomAngle = UnityEngine.Random.Range(0, 360f);
			var rotation = Quaternion.Euler(0, -randomAngle, 0);

            PhotonNetwork.RPC(GetComponent<PhotonView>(), "OnInstantiateUnit", PhotonTargets.All, true);

            var viewID = PhotonNetwork.AllocateViewID();
            PhotonCustomEventSender.RaiseInstantiateUnitEvent(
				this.GetPhotonView(), "OnInstantiateUnit", new int[] { viewID }, PhotonNetwork.player.GetUnitTeam(), PhotonNetwork.player.GetClassify(), centerPosition);
		}

		[PunRPC]
		void OnInstantiateUnit(int[] viewID, byte team, string classifyID, Vector3 position)
		{
			var actorID = viewID[0] / PhotonNetwork.MAX_VIEW_IDS;
			var photonPlayer = PhotonHelper.GetPlayer(actorID);

			var prefab = _playerPrefab.Find(temp => temp.name == classifyID);
			if (prefab == null)
			{
				return;
			}

			var p = Instantiate(prefab);
			p.name = classifyID;

			var view = p.GetComponent<PhotonView>();
			view.viewID = viewID[0];

			p.transform.position = Vector3.zero;
			p.transform.rotation = Quaternion.identity;
			p.Model.position = position;
			p.Model.rotation = Quaternion.identity;

			p.Init(new BattleUnit.UnitCreateArgs()
			{
				Team = team,
				Tag = "Player",
				InitialParameter = PhotonHelper.GetInitialParameter(photonPlayer)
			});
			
			UnitManager.Instance.AddPlayerUnit(p);

			if (p.IsPlayerForThisClient)
			{
				Camera.main.GetComponent<Deftly.DeftlyCamera>().ApplyTargetPositionImmidiatly();

				UnitManager.Instance.SetThisClientPlayerUnit(p);

                p.Fsm.SendEvent(_afterFsmEvent);
            }

		}
	}
}