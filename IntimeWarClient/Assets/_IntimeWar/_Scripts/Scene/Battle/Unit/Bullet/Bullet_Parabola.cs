using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Haruna.Pool;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	public class Bullet_Parabola : CollisionBullet, IDamageCreator
	{
		[SerializeField]
		float _toUnitDamage = 10;
		public float ToUnitDamage { set { _toUnitDamage = value; } get { return _toUnitDamage; } }
		[SerializeField]
		float _toEnvDamage = 1;
		public float ToEnvDamage { set { _toEnvDamage = value; } get { return _toEnvDamage; } }

		[SerializeField]
		float _blastRaiuds;
		[SerializeField]
		LayerMask _collisionLayer;
		[SerializeField]
		[Range(0.5f, 100f)]
		float _speedMagnification = 1;

		float _applyGravityY;

		[SerializeField]
		FxHandler _blastFx;
		[SerializeField]
		SePlayer _blastSe;

		Vector3 _dropPoint;
		public Vector3 DropPonit { set { _dropPoint = value; } get { return _dropPoint; } }

		float _elevationAngle;
		public float ElevationAngle { set { _elevationAngle = value; } get { return _elevationAngle; } }

		float _velocityZ;
		float _initialVelocityY;
		float _currentVelocityY;
		float _duration;
		
		float _elapsedTime;
		
		Vector3 _initialForward;
		Vector3 _initialRotationEuler;

		public override void Activate()
		{
			base.Activate();

			_initialForward = _dropPoint - _unit.Model.position;
			_initialForward.y = 0;
			_initialForward.Normalize();
			_initialRotationEuler = Quaternion.FromToRotation(Vector3.forward, _initialForward).eulerAngles;

			_applyGravityY = Physics.gravity.y * _speedMagnification;

			var yDistance = _unit.Model.position.y - _dropPoint.y;
			var zDistance = Util.GetHorizontalDistance(_unit.Model.position, _dropPoint);

			var _elevationTan = Mathf.Tan(Mathf.Deg2Rad * _elevationAngle);

			_velocityZ = Mathf.Sqrt(-0.5f * _applyGravityY * zDistance * zDistance / (_elevationTan * zDistance + yDistance));
			_currentVelocityY = _initialVelocityY = _elevationTan * _velocityZ;
			_duration = zDistance / _velocityZ;

			_elapsedTime = 0;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			_elapsedTime += Time.deltaTime;

			var currentPosition = _unit.Model.position;
			currentPosition += _initialForward * _velocityZ * Time.deltaTime;
			currentPosition.y += _currentVelocityY * Time.deltaTime;
			_currentVelocityY += _applyGravityY * Time.deltaTime;
			_unit.Model.position = currentPosition;
			if(_elapsedTime >= _duration)
			{
				OutOfRangeDestroy();
			}


			var angle = Mathf.Atan(_currentVelocityY / _velocityZ) * Mathf.Rad2Deg;
			var rotationEuler = _initialRotationEuler;
			rotationEuler.x = -angle;
			_unit.Model.rotation = Quaternion.Euler(rotationEuler);
			//_unit.Model.forward = direction;
		}

		public override void ProcessCollisionEvent(CollisionEventReceiver receiver, Collision collision)
		{
			if (receiver.Unit.SeqNo == _unit.Info.SpawnFrom.SeqNo)
				return;

			if ((int)PenetrationLevel <= (int)receiver.Unit.Body.PenetrationResist)
			{
				this.DestroyBullet();
				CreateBlastDamage();
			}
			else
			{
				var damageInfo = new DamageEvent();
				damageInfo.Attacker = _unit.Info.SpawnFrom;
				DamageCalculator.CreateDamage(this, damageInfo, receiver.Unit);
			}
		}

		void OutOfRangeDestroy()
		{
			this.DestroyBullet();
			CreateBlastDamage();
		}

		void CreateBlastDamage()
		{
			_blastFx.Show(_unit.Model);
			_blastSe.Play(_unit.Model);

			var colliders = Physics.OverlapSphere(_unit.Model.position, _blastRaiuds, _collisionLayer, QueryTriggerInteraction.Collide);
			var collidedUnits = new HashSet<int>();

			var damageInfo = new DamageEvent();
			damageInfo.Attacker = _unit.Info.SpawnFrom;

			for (var i = 0; i < colliders.Length; i++)
			{
				var receiver = colliders[i].GetComponent<CollisionEventReceiver>();
				if(receiver != null)
				{
					if (receiver.Unit == null)
					{
						Debug.LogErrorFormat(this, "unit is not inialized!");
						continue;
					}
					Debug.LogFormat("grenade hit {0}", receiver.Unit.name);

					if (!collidedUnits.Contains(receiver.Unit.SeqNo))
					{
						collidedUnits.Add(receiver.Unit.SeqNo);

						var direction = receiver.Unit.Model.position - _unit.Model.position;
						direction.y = 0;
						damageInfo.Direction = Util.GetHorizontalDirection(_unit.Model.position, receiver.Unit.Model.position);

						DamageCalculator.CreateDamage(this, damageInfo, receiver.Unit);
					}
				}
			}

		}
	}
}