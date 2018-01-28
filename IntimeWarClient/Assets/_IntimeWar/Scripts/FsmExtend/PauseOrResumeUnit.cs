using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.Events;
using YJH.Unit;

namespace View.Fsm
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