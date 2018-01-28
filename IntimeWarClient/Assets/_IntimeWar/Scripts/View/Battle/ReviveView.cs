using IntimeWar.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using YJH.Unit;

namespace IntimeWar.View
{
    public class ReviveView : BaseView
    {
        [SerializeField]
        float _reviveTime = 5;
        [SerializeField]
        Text _reviveTxt;
        [SerializeField]
        GameObject _reviveBtn;

        float _elapsedTime;
        
        Revive _revive;


        public override void Init()
        {
            base.Init();
            var unit = UnitManager.ThisClientPlayerUnit;
            if (unit != null)
            {
                _revive = unit.GetAbility<Revive>();
            }
        }

        public override void Show()
        {
            base.Show();
            _elapsedTime = _reviveTime;
        }

        private void Update()
        {
            _elapsedTime -= Time.deltaTime;
            if(_elapsedTime >= 0)
            {
                _reviveTxt.text = string.Format("复活倒计时：{0:D2}", ((int)_elapsedTime));
                _reviveBtn.SetActive(false);
            }
            else
            {
                _reviveBtn.SetActive(true);
            }
            
        }

        public void OnReviveClick()
        {
            if (_revive != null)
            {
                _revive.ReviveUnit();
            }
        }
    }
}
