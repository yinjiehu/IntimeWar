using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	[HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
	public class AsmPatrol : FsmStateAbility
	{
		RaidarSystem _raidar;
		public FsmObject _targetUnit;

		public override void LateInit()
		{
			base.LateInit();
			_raidar = _unit.GetAbility<RaidarSystem>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

		}

	}
}