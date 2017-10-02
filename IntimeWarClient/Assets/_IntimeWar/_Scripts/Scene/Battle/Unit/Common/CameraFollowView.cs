using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;
using UnityEngine.UI;

namespace MechSquad.Battle
{
	public class CameraFollowView : Ability
	{
		Deftly.DeftlyCamera _camera;
		IMainFireControlSystem _turret;

		[SerializeField]
		Vector3 _offset;

		[SerializeField]
		bool _on;

		public override void LateInit()
		{
			base.LateInit();
			if (_on && _unit.IsPlayerForThisClient)
			{
				_turret = _unit.GetAbility<IMainFireControlSystem>();
				_camera = Camera.main.GetComponent<Deftly.DeftlyCamera>();
				_camera.Offset = _offset;
			}
			else
			{
				_on = false;
			}
		}

		private void LateUpdate()
		{
			if (_unit != null && _on)
			{
				var y = Quaternion.FromToRotation(Vector3.forward, _turret.CurrentTurretDirection).eulerAngles.y;
				var cameraRotationEuler = _camera.transform.rotation.eulerAngles;
				cameraRotationEuler.y = y;
				_camera.transform.rotation = Quaternion.Euler(cameraRotationEuler);

				_camera.Offset = _turret.TurretRoot.TransformDirection(_offset);
			}
		}
	}
}