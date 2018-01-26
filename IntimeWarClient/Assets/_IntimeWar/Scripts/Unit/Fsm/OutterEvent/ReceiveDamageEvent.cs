using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace YJH.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class ReceiveDamageEvent : FsmStateAbility
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _unit.EventDispatcher.AddReceiveEventType(typeof(DamageEvent));
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _unit.EventDispatcher.RemoveReceiveEventType(typeof(DamageEvent));
        }
    }
}