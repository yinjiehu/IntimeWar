﻿using HutongGames.PlayMaker;
using MechSquad.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Unit")]
	public class PauseOrResumeUnit : FsmStateAction
	{
		public enum ControlTypeEnum
		{
			Pause,
			Resume
		}
		
		public ControlTypeEnum _control;

		public FsmObject _unit;

		public override void OnEnter()
		{
			base.OnEnter();

			((BattleUnit)_unit.Value).Fsm.SendEvent(_control.ToString());

			Finish();
		}
	}
}