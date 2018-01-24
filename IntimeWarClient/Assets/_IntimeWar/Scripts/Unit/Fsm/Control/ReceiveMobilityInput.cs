using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace YJH.Unit
{
    [HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
    public class ReceiveMobilityInput : FsmStateAbility
    {
        List<IUnitMobilityInput> _mobilityInputs;
        UnitMobility _mobility;

        Vector3 _lastInputDirection;
        public Vector3 LastInputDirection
        {
            get
            {
                if (_lastInputDirection == Vector3.zero)
                {
                    _lastInputDirection = _unit.Model.forward;
                }
                return _lastInputDirection;
            }
        }
        public override void LateInit()
        {
            base.LateInit();
            _mobilityInputs = _unit.GetAllAbilities<IUnitMobilityInput>().ToList();
            _mobility = _unit.GetAbility<UnitMobility>();
            Finish();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            for (var i = 0; i < _mobilityInputs.Count; i++)
            {
                if (_mobilityInputs[i].Enabled)
                {
                    var direction = _mobilityInputs[i].NormalizedMoveDirection;
                    if (direction != null)
                    {
                        //_unit.GetAbility<IMainTurret>().SetTargetDirection(direction.Value);
                        //_mobility.SetTargetPosition(_unit.Model.position + direction.Value);
                        _mobility.SetMoveDirection(direction.Value);
                        _lastInputDirection = direction.Value;
                        return;
                    }
                }
            }
        }
    }
}