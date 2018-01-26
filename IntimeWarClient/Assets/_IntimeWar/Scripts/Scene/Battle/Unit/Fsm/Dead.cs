using HutongGames.PlayMaker;
using IntimeWar.View;
using UnityEngine;
using View;
using YJH.Unit;

namespace IntimeWar.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class Dead : FsmStateAbility
    {
        public FsmEvent Next;

        float _elapsedTime;

        public override void OnEnter()
        {
            base.OnEnter();

            if (_unit.GetAbility<Revive>() != null)
                Fsm.Event(Next);
        }
    }
}