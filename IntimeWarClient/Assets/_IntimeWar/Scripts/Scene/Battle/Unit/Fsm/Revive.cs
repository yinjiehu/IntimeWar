using HutongGames.PlayMaker;
using IntimeWar.View;
using UnityEngine;
using View;
using YJH.Unit;

namespace IntimeWar.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class Revive : FsmStateAbility
    {
        public FsmEvent Next;

        float _elapsedTime;

        public override void OnEnter()
        {
            base.OnEnter();
            _elapsedTime = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= 5)
            {
                if (_unit.IsControlByThisClient)
                {
                    _unit.SendAbilityRPC(this, "RPCReviveAddUnit", null);
                }
            }
        }

        void RPCReviveAddUnit()
        {
            Fsm.Event(Next);
            _unit.Body.ResetHp();
            UnitManager.Instance.AddUnit(_unit);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}