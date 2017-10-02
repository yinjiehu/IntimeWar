using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	public class MainFsm : Ability
	{
		[SerializeField]
		PlayMakerFSM _mainFsm;

		public override void SetupInstance(BattleUnit unit)
		{
			base.SetupInstance(unit);

			_unit.Fsm = _mainFsm;
		}

	}
}