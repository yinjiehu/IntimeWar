using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	public class BasicStatusControl : FsmStateAbility
	{
		public bool IsDead;
		public FsmInt BodyVisibleControlSeq;
		public bool BodyVisible;
		public bool EventReceiverEnable;
		public bool BodyStrikeEnable;
		public bool ProvideSquadSight;
		public bool RaidarEnable;
		public bool DisplayInRaidar;
		public bool CanBeLockedOn;
		public bool AutoFireControlAvaliable;
		public bool TurretLineVisible;

		DataMixerBool.Control _bodyVisibleControl;

		public override void OnEnter()
		{
			base.OnEnter();

			if (_unit != null)
			{
				UpdateBodyVisible();

				_unit.STS.IsDead.Value = IsDead;
				_unit.STS.EventReceiverEnable.Value = EventReceiverEnable;
				_unit.STS.BodyStrikeEnable.Value = BodyStrikeEnable;
				_unit.STS.ProvideSquadSight.Value = ProvideSquadSight;
				_unit.STS.RaidarEnable.Value = RaidarEnable;
				_unit.STS.DisplayInRaidar.Value = DisplayInRaidar;
				_unit.STS.CanBeLockedOn.Value = CanBeLockedOn;
				_unit.STS.AutoFireControlAvaliable.Value = AutoFireControlAvaliable;
				_unit.STS.TurretLineVisible.Value = TurretLineVisible;
			}

			Finish();
		}

		void UpdateBodyVisible()
		{
			if(_bodyVisibleControl == null)
			{
				if (BodyVisibleControlSeq.Value < 0)
				{
					_bodyVisibleControl = _unit.STS.BodyVisible.CreateControl(false);
					BodyVisibleControlSeq.Value = _bodyVisibleControl.SequencNo;
				}
				else
				{
					_bodyVisibleControl = _unit.STS.BodyVisible.GetControl(BodyVisibleControlSeq.Value);
				}
			}
			_bodyVisibleControl.Value = BodyVisible;
		}
	}
}