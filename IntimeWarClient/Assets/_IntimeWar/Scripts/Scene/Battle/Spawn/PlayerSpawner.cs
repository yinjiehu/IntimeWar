using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using IntimeWar;
using YJH.Unit;
using System.Linq;
using System.Collections.Generic;

namespace IntimeWar.Battle
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerSpawner : MonoBehaviour, ISpawner
    {
        public string Name { get { return name; } }

        [SerializeField]
        List<BattleUnit> _playerPrefab;
        [SerializeField]
        int _level;
        [SerializeField]
        Transform[] _spawnPositionsA;

        [SerializeField]
        Transform[] _spawnPositionsB;


        [SerializeField]
        float _randomDistance = 50;

        [SerializeField]
        string _afterFsmEvent;
        //public BattleUnit _spawnedUnit;

        public void SpawnUnit()
        {
            if (PhotonNetwork.offlineMode)
                PhotonHelper.CreateOfflinePhotonPlayerProperties();

            //var seqNo = PhotonNetwork.playerList.Where(p => p.GetTeam() == PhotonNetwork.player.GetTeam()).OrderBy(p => p.ID).ToList().FindIndex(p => p.ID == PhotonNetwork.player.ID);

            var team = PhotonNetwork.player.GetUnitTeam();

            var players = PhotonNetwork.playerList.Where(p => p.GetUnitTeam() == team).OrderBy(p => p.NickName).ToList();
            var index = players.FindIndex(p => p.NickName == PhotonNetwork.player.NickName);

            Transform pos = team == Team.A ? _spawnPositionsA[index] : _spawnPositionsB[index];

            var viewID = PhotonNetwork.AllocateViewID();
            SpawnerManager.Instance.SendSpawnEvent(Name, "OnInstantiateUnit", viewID, PhotonNetwork.player.GetUnitTeam(), PhotonNetwork.player.GetClassify(), pos.position);
        }

        [PunRPC]
        void OnInstantiateUnit(int viewID, byte team, string classify, Vector3 position)
        {
            var actorID = viewID < 0 ? -1 : viewID / PhotonNetwork.MAX_VIEW_IDS;
            var photonPlayer = PhotonHelper.GetPlayer(actorID);
            if (photonPlayer == null)
                return;

            var prefab = _playerPrefab.Find(temp => temp.name == classify);
            if (prefab == null)
            {
                Debug.LogFormat("Can not find vehicle prefab for classify {0}", classify);
                return;
            }

            var p = Instantiate(prefab);
            p.name = classify;

            var view = p.GetComponent<PhotonView>();
            view.viewID = viewID;

            p.transform.position = Vector3.zero;
            p.transform.rotation = Quaternion.identity;
            p.Model.position = position;
            p.Model.rotation = Quaternion.identity;

            p.Init(new BattleUnit.UnitCreateArgs()
            {
                Team = team,
                Level = _level,
                Tag = "Player",
                InitialParameter = PhotonNetwork.player.GetInitialParameter()
            });

            UnitManager.Instance.AddPlayerUnit(p);

            p.DisableAI();

            if (p.IsPlayerForThisClient)
            {
                var camera = Camera.main.GetComponent<FollowCamera>();
                camera.ClearTargets();
                camera.AddTarget(p.Model);

                UnitManager.Instance.SetThisClientPlayerUnit(p);

                if (!string.IsNullOrEmpty(_afterFsmEvent))
                {
                    p.Fsm.SendEvent(_afterFsmEvent);
                }
            }
        }
    }
}