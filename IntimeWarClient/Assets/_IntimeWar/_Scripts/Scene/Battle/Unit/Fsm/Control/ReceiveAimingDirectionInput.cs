using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	[HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
	public class ReceiveAimingDirectionInput : FsmStateAbility
	{
		List<IUnitAimingInput> _aimingInputs;

		RaidarSystem _raidar;
		IMainFireControlSystem _fireControl;

		public override void LateInit()
		{
			base.LateInit();
			_aimingInputs = _unit.GetAllAbilities<IUnitAimingInput>().ToList();
			_raidar = _unit.GetAbility<RaidarSystem>();
			_fireControl = _unit.GetAbility<IMainFireControlSystem>();
			Finish();
		}
		
		public override void OnUpdate()
		{
			base.OnUpdate();
			UpdateAimingDirection();
			UpdateAimingMode();
		}

		void UpdateAimingDirection()
		{
			for (var i = 0; i < _aimingInputs.Count; i++)
			{
				if (_aimingInputs[i].Enabled)
				{
					var diretion = _aimingInputs[i].AimingDirection;
					if (diretion != null)
					{
						_fireControl.SetAimingDirection(diretion.Value);
						return;
					}
				}
			}
			_fireControl.SetAimingDirection(Vector3.zero);
		}

		void UpdateAimingMode()
		{
			for (var i = 0; i < _aimingInputs.Count; i++)
			{
				var current = _aimingInputs[i];

				if (current.Enabled)
				{
					if (_fireControl.CurrentAimingMode == AimingModeEnum.Auto && current.SwitchTargetOnAutoMode)
					{
						_fireControl.SwitchTarget();
					}

					if (current.SwitchAimingMode)
					{
						if (current.ToSwitchAimingMode == AimingModeEnum.Auto)
						{
							_fireControl.SwitchAimingMode(AimingModeEnum.Auto);
							if (_fireControl.GetAimingTarget() != null)
								_fireControl.SwitchTarget();
						}
						else
						{
							_fireControl.SwitchAimingMode(current.ToSwitchAimingMode);
						}
					}
				}
			}
		}
	}
}