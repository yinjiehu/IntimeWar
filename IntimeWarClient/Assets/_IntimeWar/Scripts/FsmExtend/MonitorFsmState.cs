using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections.Generic;

namespace View.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class MonitorFsmState : FsmStateAction
	{
		public PlayMakerFSM _fsm;

		public enum MonitoringTypeEnum
		{
			Enter,
			Exit,
		}

		public MonitoringTypeEnum _monitorType;

		public FsmString _stateName;
		public FsmEvent _forwardEvent;
		
		public override void OnUpdate()
		{
			base.OnUpdate();
			
			if(_monitorType == MonitoringTypeEnum.Enter)
			{
				if(_fsm.ActiveStateName == _stateName.Value)
				{
					Fsm.Event(_forwardEvent);
					Finish();
				}
			}
			else
			{
				if (_fsm.ActiveStateName != _stateName.Value)
				{
					Fsm.Event(_forwardEvent);
					Finish();
				}
			}

		}
	}
}