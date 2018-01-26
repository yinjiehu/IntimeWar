using Haruna.Utility;
using HutongGames.PlayMaker;
using IntimeWar.View;
using UnityEngine;
using View;
using YJH.Unit;

namespace IntimeWar.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class RandomAttack : FsmStateAbility
    {
        public FsmEvent Next;
        AIInput _aiInput;

        float _elapsedTime;
        float _elapsedIntervalTime;

        public override void OnEnter()
        {
            base.OnEnter();
            _aiInput = _unit.GetAbility<AIInput>();
            _elapsedTime = 0;
            _elapsedIntervalTime = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            _elapsedTime += Time.deltaTime;

            if(_elapsedTime > 5)
            {
                Fsm.Event(Next);
            }
            else
            {
                _elapsedIntervalTime += Time.deltaTime;
                if(_elapsedIntervalTime > 1)
                {
                    _elapsedIntervalTime = 0;
                    int random = Random.Range(0, 4);
                    _aiInput.AttachmentPress[random] = true;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}