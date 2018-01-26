using Haruna.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace IntimeWar.View
{
    public class BattleView : BaseView
    {
        [SerializeField]
        Text _gold;
        [SerializeField]
        Text _selfTeamScore;
        [SerializeField]
        Text _enemyTeamScore;
        [SerializeField]
        Text _killDescription;
        [SerializeField]
        ListMemberUpdater _selfTeams;
        [SerializeField]
        ListMemberUpdater _enemyTeams;

        public struct Model
        {
            public class PlayerInfo
            {
                public string NickName;
                public int Level;
            }
            public int Money;
            public int SelfTeamScore;
            public int EnemyTeamScore;
            public int KillNumber;
            public List<PlayerInfo> SelfTeams;
            public List<PlayerInfo> EnemyTeams;
        }

        public override void Init()
        {
            base.Init();


            InvokeRepeating("UpdateScore", 0, 1);
        }

        void UpdateScore()
        {
            var data = BattlePresenter.GetBattleViewModel();
            BindToView(data);
        }

        public void BindToView(Model data)
        {
            _gold.text = data.Money.ToString();
            _selfTeamScore.text = string.Format("{0:D2}", data.SelfTeamScore);
            _enemyTeamScore.text = string.Format("{0:D2}", data.EnemyTeamScore);
            _killDescription.text = string.Format("击杀敌人数量：{0}", data.EnemyTeamScore);
            _selfTeams.OnListUpdate<Model.PlayerInfo>(data.SelfTeams, (info, block, index) =>
            {
                block.GetComponent<BattlePlayerInfoBlock>().UpdateData(info);
            });
            _enemyTeams.OnListUpdate<Model.PlayerInfo>(data.EnemyTeams, (info, block, index) =>
            {
                block.GetComponent<BattlePlayerInfoBlock>().UpdateData(info);
            });
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void OnCheckPlayerClick()
        {

        }

        public void OnSettingsClick()
        {

        }
    }
}