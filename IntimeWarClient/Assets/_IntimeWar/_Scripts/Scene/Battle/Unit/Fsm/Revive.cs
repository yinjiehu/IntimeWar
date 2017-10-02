using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class Revive : FsmStateAbility
	{
		Vector3 _originSpawnPosition;
		public Vector3 OriginSpawnPosition
		{
			set { _originSpawnPosition = value; }
			get { return _originSpawnPosition; }
		}

		public float _reviveDuration = 10;

		float _elapsedTime;
		bool _isReviving;

		GameObject _tempFollow;

		public override void Init()
		{
			base.Init();
			_originSpawnPosition = _unit.Model.position;
		}

		public override void OnEnter()
		{
			base.OnEnter();

			if (_unit.IsControlByThisClient)
			{
				_isReviving = true;
				_elapsedTime = 0;
			}

			if (_unit.IsPlayerForThisClient)
			{
				_tempFollow = new GameObject("TempCameraFollow");
				var p = _unit.GetAbility<IMainFireControlSystem>();
				_tempFollow.transform.position = _unit.Model.position;
				_tempFollow.transform.rotation = p.TurretRoot.rotation;
				Camera.main.GetComponent<Deftly.DeftlyCamera>().ResetTarget(new GameObject[] { _tempFollow });
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_isReviving && _unit.IsControlByThisClient)
			{
				_elapsedTime += Time.deltaTime;
				if (_elapsedTime > _reviveDuration)
				{
					_unit.Model.position = SpawnerManager.PlayerRevivePositionPolicy.GetRevivePosition(_unit);
					_unit.Fsm.SendEvent("TitanFall");
					_unit.SendAbilityRPC(this, "RPCReviveAddUnit", null);
				}
			}
		}

		void RPCReviveAddUnit()
		{
			UnitManager.Instance.AddUnit(_unit);
		}

		public override void OnExit()
		{
			base.OnExit();

			if (_unit.IsPlayerForThisClient)
			{
				UnityEngine.Object.Destroy(_tempFollow);
			}
		}
	}
}