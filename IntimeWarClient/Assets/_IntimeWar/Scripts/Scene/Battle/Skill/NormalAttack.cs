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

        public override void Init()
        {
            base.Init();
            _mobility = _unit.GetAbility<MobilityFull>();
        }

        public void Attack()
        {
            Debug.Log("attack:" + SkillID + "   " + SlotID);
            if (IsReloading)
                return;
            Reload();
            var angle = Vector2.Angle(_mobility.LastDirection, Vector2.up);
            float dir = (Vector2.Dot(Vector2.up, _mobility.LastDirection) < 0 ? -1 : 1);
            angle *= dir;
            _attackFx.Show(_unit.Model.position, angle);
        }
    }
}