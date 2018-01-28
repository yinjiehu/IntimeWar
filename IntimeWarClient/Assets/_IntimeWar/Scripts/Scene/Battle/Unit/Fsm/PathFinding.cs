using HutongGames.PlayMaker;
using IntimeWar.View;
using System.Linq;
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

        float _attackRange;
        float _turnDirection;
        bool _isTurn;
        float _elapsedTurn = 0;

        public override void Init()
        {
            base.Init();

            _attackRange = _unit.InitialParameter.GetParameter(ConstParameter.AttackRange);
            _aiInput = _unit.GetAbility<AIInput>();
        }

        public override void LateInit()
        {
            base.LateInit();
            Finish();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _turnDirection = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
            _isTurn = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var target = UnitManager.Instance.GetNearestPlayerUnit(_unit.SeqNo, _unit.Team, _unit.Model.position);
            if (target != null)
            {
                var direction = target.Model.position - _unit.Model.position;
                var hits = Physics2D.OverlapBoxAll(_unit.Model.position + direction.normalized * _attackRange/2, _unit.Model.GetComponent<BoxCollider2D>().size, Vector2.Angle(Vector2.up, direction));
                //var hits = Physics2D.RaycastAll(_unit.Model.position, direction, _attackRange);
                hits = hits.Where(h => h.GetComponentInParent<BattleUnit>() != null
                                  && h.GetComponentInParent<BattleUnit>() != _unit
                                  && h.GetComponentInParent<BattleUnit>().SeqNo != target.SeqNo).ToArray();
                if (hits.Length > 0 || !_isTurn)
                {
                    if (_turnDirection > 0)
                    {
                        direction = Quaternion.Euler(0, 0, 90) * direction;
                    }
                    else
                    {
                        direction = Quaternion.Euler(0, 0, -90) * direction;
                    }
                    _aiInput.NormalizedMoveDirection = direction;
                    _elapsedTurn += Time.deltaTime;
                    if (_elapsedTurn > 1)
                    {
                        _isTurn = true;
                    }
                    else
                    {
                        _isTurn = false;
                    }
                }
                else
                {
                    _elapsedTurn = 0;
                    _aiInput.NormalizedMoveDirection = direction;
                    if (Vector3.Distance(target.Model.position, _unit.Model.position) < _attackRange)
                    {
                        Fsm.Event(Next);
                    }
                }
                    
            }
            else
            {
                _aiInput.NormalizedMoveDirection = Vector3.zero;
            }
        }

        

        public override void OnExit()
        {
            base.OnExit();
            _aiInput.NormalizedMoveDirection = Vector3.zero;
        }
    }
}