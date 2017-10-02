using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.Event;
using UnityEngine.Events;
using MechSquad.View;

namespace MechSquad.Battle
{
	public class Tree : UnitBody
	{
		float _downDuration = 5f;
		float _elapsedTime;
		float _elapsedTimeCorrection;

		bool _treeDown;

		Quaternion _startRotation;
		Quaternion _targetRotation;

		UnitInfo _attacker;

		[SerializeField]
		SePlayer _treeDownSe;

		[SerializeField]
		Haruna.Inspector.InspectorButton _down;
		private void Down()
		{
			SetTreeDown(Vector3.forward);
		}

		public override void Init()
		{
			base.Init();

			_unit.EventDispatcher.AddReceiveEventType(typeof(DamageEvent));
			_unit.EventDispatcher.ReigstEvent<DamageEvent>(OnDamage, 0);
			_unit.EventDispatcher.AddReceiveEventType(typeof(VehicleBodyStrikeEvent));
			_unit.EventDispatcher.ReigstEvent<VehicleBodyStrikeEvent>(OnVehicleBodyStrike, 0);
			_unit.EventDispatcher.AddReceiveEventType(typeof(TitanFallStrikeEvent));
			_unit.EventDispatcher.ReigstEvent<TitanFallStrikeEvent>(OnTitanFallStrike, 0);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_treeDown)
			{
				_elapsedTimeCorrection += Time.deltaTime;
				_elapsedTime += Time.deltaTime + _elapsedTimeCorrection;

				var ratio = _elapsedTime / _downDuration / 2f;
				if (ratio < 1)
				{
					_unit.transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, ratio);
				}
				else
				{
					_unit.transform.rotation = _targetRotation;
					Destroy(_unit.gameObject);
				}
			}
		}

		void OnDamage(DamageEvent damageInfo, EventControl control)
		{
			SetTreeDown(damageInfo.Direction);
		}
		void OnVehicleBodyStrike(VehicleBodyStrikeEvent ev, EventControl control)
		{
			SetTreeDown(_unit.Model.position - ev.SourcePosition);
		}
		void OnTitanFallStrike(TitanFallStrikeEvent ev, EventControl control)
		{
			SetTreeDown(_unit.Model.position - ev.SourcePosition);
		}

		void SetTreeDown(Vector3 direction)
		{
			if (!_treeDown)
			{
				_treeDownSe.Play(_unit.Model.position);

				GetComponent<Collider>().enabled = false;

				direction.y = 0;

				var direcRotation = Quaternion.LookRotation(direction, Vector3.up);

				_startRotation = direcRotation;
				_targetRotation = direcRotation * Quaternion.Euler(90, 0, 0);

				_treeDown = true;
			}
		}
	}
}