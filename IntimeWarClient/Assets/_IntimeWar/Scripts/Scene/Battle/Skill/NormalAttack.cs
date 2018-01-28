using System;
using IntimeWar.Battle;
using UnityEngine;
using IntimeWar;
using IntimeWar.Data;

namespace YJH.Unit
{
    public class NormalAttack : AbstractSkill, INormalAttack
    {
        [SerializeField]
        FxHandler2D _attackFx;

        MobilityFull _mobility;

        float _attackRange;

        public override void Init()
        {
            base.Init();
            _mobility = _unit.GetAbility<MobilityFull>();
            _attackRange = _unit.InitialParameter.GetParameter(ConstParameter.AttackRange);
        }

        public void Attack()
        {
            if (IsReloading)
                return;
            var nearestUnit = UnitManager.Instance.GetNearestUnit(_unit.Info.SeqNo, _unit.Team, _unit.Model.position, _attackRange);
            if (nearestUnit == null)
                return;
            Reload();
            var direction = nearestUnit.Model.position - _unit.Model.position;
            var angle = Vector2.SignedAngle(Vector2.up, direction);
            _attackFx.Show(_unit.Model, angle);
            var damageInfo = new DamageInfo();
            damageInfo.Damage = 20;
            DamageCalculator.CreateRaycastLine2DDamge(_unit.Info, damageInfo, _unit.Model.position, direction, 5, LayerMask.NameToLayer("Unit"), null);
        }
    }
}