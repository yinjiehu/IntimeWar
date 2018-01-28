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
        float _attackRange;

        float _attackIntervalTime = 1;
        float _increaseIntervalTime = 1;

        public override void LateInit()
        {
            base.LateInit();
            Finish();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _attackRange = _unit.InitialParameter.GetParameter(ConstParameter.AttackRange);
            _aiInput = _unit.GetAbility<AIInput>();
            _elapsedTime = 0;
            _elapsedIntervalTime = 0;
            _attackIntervalTime = 1;
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
                if(_elapsedIntervalTime > _attackIntervalTime)
                {
                    _elapsedIntervalTime = 0;
                    var target = UnitManager.Instance.GetNearestPlayerUnit(_unit.SeqNo, _unit.Team, _unit.Model.position, _attackRange);

                    if (target != null)
                    {
                        _attackIntervalTime += _increaseIntervalTime;
                        int random = Random.Range(0, 4);
                        _aiInput.AttachmentPress[random] = true;
                    }
                    else
                    {
                        Fsm.Event(Next);
                    }
                }
                else
                {
                    DisablePress();
                }
            }
        }

        void DisablePress()
        {
            _aiInput.AttachmentPress[0] = false;
            _aiInput.AttachmentPress[1] = false;
            _aiInput.AttachmentPress[2] = false;
            _aiInput.AttachmentPress[3] = false;
        }

        public override void OnExit()
        {

            base.OnExit();
        }
    }
}