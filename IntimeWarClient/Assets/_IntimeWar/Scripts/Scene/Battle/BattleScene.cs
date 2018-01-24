using UnityEngine;
using System.Collections.Generic;
using YJH.Unit;

namespace IntimeWar.Battle
{
    public class BattleScene : MonoBehaviour
    {
        static BattleScene _instance;
        public static BattleScene Instance { get { return _instance; } }

        AudioListener _audioListener;

        private void Awake()
        {
            _instance = this;
            _instance._audioListener = FindObjectOfType<AudioListener>();

        }


        public void AddKillCount(int attackerActorID, int receiverActorID)
        {
            this.GetPhotonView().RPC("RPCKillTip", PhotonTargets.All, PhotonHelper.GetPlayer(attackerActorID), PhotonHelper.GetPlayer(receiverActorID));
            if (PhotonNetwork.player.ID == attackerActorID)
            {
                RPCAddKillCount(attackerActorID);
            }
            else
            {
                if (PhotonHelper.GetPlayer(attackerActorID).GetUnitTeam() != PhotonHelper.GetPlayer(receiverActorID).GetUnitTeam())
                    this.GetPhotonView().RPC("RPCAddKillCount", PhotonTargets.Others, attackerActorID);
            }
            if (PhotonNetwork.player.ID == receiverActorID)
            {
                RPCAddDeathCount(receiverActorID);
            }
            else
            {
                this.GetPhotonView().RPC("RPCAddDeathCount", PhotonTargets.Others, receiverActorID);
            }
        }

        [PunRPC]
        void RPCKillTip(PhotonPlayer attacker, PhotonPlayer death)
        {
            //ViewManager.Instance.GetView<KillTipView>().CreateKillTipPrefab(attacker, death);
        }


        [PunRPC]
        void RPCAddKillCount(int attackerActorID)
        {
            if (PhotonNetwork.player.ID == attackerActorID)
                PhotonHelper.AddKillCount();
        }

        [PunRPC]
        void RPCAddDeathCount(int receiverActorID)
        {
            if (PhotonNetwork.player.ID == receiverActorID)
                PhotonHelper.AddDeathCount();
        }

        public void PauseAllUnit()
        {
            var allUnits = UnitManager.Instance.AllUnits;
            using (var itr = allUnits.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (!itr.Current.IsDead)
                        itr.Current.Pause();
                }
            }
        }

        Dictionary<string, GameObject> _warningDic = new Dictionary<string, GameObject>();

        public void GenerateWarningTip(string key, GameObject prefab, Vector3 position)
        {
            var warningTranform = Instantiate(prefab);
            if (_warningDic.ContainsKey(key))
            {
                DeleteWarningTip(key);
            }

            _warningDic.Add(key, warningTranform);
            warningTranform.transform.position = position;
        }

        public void DeleteWarningTip(string key)
        {
            if (_warningDic.ContainsKey(key))
            {
                DestroyObject(_warningDic[key]);
                _warningDic.Remove(key);
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        Haruna.Inspector.InspectorButton _testAddKill;
        void TestAddKill()
        {
            //PhotonHelper.AddKillCount();
            int len = 0;
            var random = new System.Random();
            for (int i = 0; i < 100; i++)
            {
                if (random.Next(100) < 50)
                {
                    len++;
                }
            }
            Debug.Log("len:" + len);
        }
        [SerializeField]
        Haruna.Inspector.InspectorButton _testAddDead;
        void TestAddDead()
        {
            PhotonHelper.AddDeathCount();
        }
#endif
    }
}