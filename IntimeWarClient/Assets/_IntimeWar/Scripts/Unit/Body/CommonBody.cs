using UnityEngine;
using View;
using IntimeWar.View;
using YJH.Unit.Event;
using IntimeWar.Battle;
using IntimeWar.Unit;
using IntimeWar;
using System.Collections.Generic;

namespace YJH.Unit
{
    public class CommonBody : UnitBody
    {
        [SerializeField]
        HudDamage _damageDisplay;
        [SerializeField]
        FxHandler2D _deadFx;
        [SerializeField]
        List<FxHandler2D> _reviveFxs;

        HudView _hudView;

        public override void Init()
        {
            _currentHp = _maxHp;
            _hudView = ViewManager.Instance.GetView<HudView>();

            _unit.EventDispatcher.ReigstEvent<DamageEvent>(OnDamage, 0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        void OnDamage(DamageEvent ev, EventControl control)
        {

            if (ev.Attacker.Team != _unit.Team)
            {
                ShowDamage((int)ev.Damage, ev);

                if (_unit.IsControlByThisClient)
                {
                    _currentHp -= ev.Damage;

                    if (_currentHp <= 0)
                    {
                        _unit.Fsm.SetState("Dead");
                        this.CallRPC("RPCDestroy");

                        BattleScene.Instance.AddKillCount(ev.Attacker.ActorID, _unit.ActorID);
                    }
                }
            }
        }


        public override void Revive()
        {
            base.Revive();
            foreach(var fx in _reviveFxs)
            {
                fx.Show(_unit.Model.position);
            }
        }

        [PunRPC]
        void RPCDestroy()
        {
            _deadFx.Show(_unit.Model.position);
            UnitManager.Instance.DestroyUnitByAttack(_unit, _unit.GetAbility<Revive>() == null);
        }

        void ShowDamage(int damage, DamageEvent ev)
        {
            var ins = _hudView.CreateFromPrefab(_damageDisplay) as HudDamage;
            ins.ShowNumber(_unit.Model, damage);
        }

        public override float PermanentSynchronizeInterval { get { return 1; } }

        public override object OnSendPermanentSynchronization()
        {
            return _currentHp;
        }
        public override void OnInitSynchronization(object data)
        {
            _currentHp = (float)data;
        }

    }
}