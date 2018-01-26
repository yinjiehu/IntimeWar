using HutongGames.PlayMaker;
using IntimeWar.View;
using UnityEngine;
using View;
using YJH.Unit;

namespace IntimeWar.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class PathFinding : FsmStateAbility
    {
        public FsmEvent Next;
        AIInput _aiInput;

        public override void OnEnter()
        {
            base.OnEnter();
            _aiInput = _unit.GetAbility<AIInput>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var target = UnitManager.Instance.GetNearestPlayer(_unit.Model.position);
            if(target != null)
            {
                _aiInput.NormalizedMoveDirection = target.Model.position - _unit.Model.position;
                if(Vector3.Distance(target.Model.position, _unit.Model.position) < 5)
                {
                    Fsm.Event(Next);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _aiInput.NormalizedMoveDirection = Vector3.zero;
        }
    }
}