using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace IntimeWar.View
{
    public class BattlePlayerInfoBlock : MonoBehaviour
    {
        [SerializeField]
        Text _nickNameTxt;
        [SerializeField]
        Text _levelTxt;
        
        public void UpdateData(BattleView.Model.PlayerInfo info)
        {
            _nickNameTxt.text = info.NickName;
            _levelTxt.text = info.Level.ToString();
        }
    }
}
