using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	[HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
	public class ReceiveMobilityInput : FsmStateAbility
	{
		List<IUnitMobilityInput> _mobilityInputs;
		UnitMobility _mobility;

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

			for(var i = 0; i < _mobilityInputs.Count; i++)
			{
				if (_mobilityInputs[i].Enabled)
				{
					var direction = _mobilityInputs[i].NormalizedMoveDirection;
					if (direction != null)
					{
						//_unit.GetAbility<IMainTurret>().SetTargetDirection(direction.Value);
						_mobility.SetTargetPosition(_unit.Model.position + direction.Value);
						return;
					}
				}
			}
		}
	}
}