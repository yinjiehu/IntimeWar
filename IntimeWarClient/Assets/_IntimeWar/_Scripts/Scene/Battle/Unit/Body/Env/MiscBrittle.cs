using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.Event;
using UnityEngine.Events;
using MechSquad.View;

namespace MechSquad.Battle
{
	public class MiscBrittle : UnitBody
	{
		[SerializeField]
		FxHandler _wreck;
		[SerializeField]
		SePlayer _destroySe;

		public override void Init()
		{
			base.Init();
			_unit.EventDispatcher.AddReceiveEventType(typeof(DamageEvent));
			_unit.EventDispatcher.ReigstEvent<DamageEvent>(OnDamage, 0);
			_unit.EventDispatcher.AddReceiveEventType(typeof(VehicleBodyStrikeEvent));
			_unit.EventDispatcher.ReigstEvent<VehicleBodyStrikeEvent>(OnStrike, 0);
			
		}

		void OnDamage(DamageEvent ev, EventControl control)
		{
			if (_wreck == null)
			{
				Debug.LogErrorFormat(this, "_wreck is null!");
				Debug.Break();
				return;
			}

			_wreck.Show(_unit.Model);
			UnitManager.Instance.DestroyUnitByAttack(_unit, true);
		}

		void OnStrike(VehicleBodyStrikeEvent ev, EventControl control)
		{
			if(_wreck == null)
			{
				Debug.LogErrorFormat(this, "_wreck is null!");
				Debug.Break();
				return;
			}

			_wreck.Show(_unit.Model);
			_destroySe.Play(_unit.Model);
			UnitManager.Instance.DestroyUnitByAttack(_unit, true);
		}
	}
}