using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using MechSquad.Event;

namespace MechSquad.Battle
{
	public class UnitAimingDisplay : Ability
	{
		[SerializeField]
		LineRenderer _turretLine;

		[SerializeField]
		LayerMask _turretLineCollideLayers;

		[SerializeField]
		float _turretLineFurtherDistance = 1;

		bool _visible;
		float _elapsedTime;
		float _updateInterval = 0.1f;

		public override void LateInit()
		{
			base.LateInit();

			transform.position = _unit.Model.position;
			transform.rotation = _unit.Model.rotation;
			_turretLine.transform.SetParent(_unit.GetAbility<IMainFireControlSystem>().TurretRoot);
			_turretLine.transform.localPosition = Vector3.zero;
			_turretLine.transform.rotation = _turretLine.transform.parent.rotation;

			OnTurretLineVisibleChange(false);

            if (_unit.IsPlayerForThisClient)
            {
                _unit.STS.TurretLineVisible.EvOnValueChange += OnTurretLineVisibleChange;
            }
		}

		private void OnTurretLineVisibleChange(bool value)
		{
			_visible = value;
			_turretLine.gameObject.SetActive(value);
		}
		
		private void FixedUpdate()
		{
			if (_visible)
			{
				RaycastHit hit;
				if (Physics.Raycast(_turretLine.transform.position, _turretLine.transform.forward, out hit, 300, _turretLineCollideLayers))
				{
					var endPosition = _turretLine.transform.InverseTransformPoint(hit.point);
					_turretLine.SetPosition(1, endPosition + Vector3.forward * _turretLineFurtherDistance);
				}
				else
				{
					_turretLine.SetPosition(1, Vector3.forward * 300);
				}
			}
		}
	}
}