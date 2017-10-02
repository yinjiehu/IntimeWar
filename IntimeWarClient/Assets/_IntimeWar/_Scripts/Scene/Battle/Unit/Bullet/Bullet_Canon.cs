using System;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad.Battle
{
	public class Bullet_Canon : CollisionBullet, IDamageCreator
	{
		[SerializeField]
		float _toUnitDamage = 10;
		public float ToUnitDamage { set { _toUnitDamage = value; } get { return _toUnitDamage; } }
		[SerializeField]
		float _toEnvDamage = 1;
		public float ToEnvDamage { set { _toEnvDamage = value; } get { return _toEnvDamage; } }

		[SerializeField]
		float _flySpeed;
		
		float _maxRange;
		public float MaxRange { set { _maxRange = value; } get { return _maxRange; } }

		float _fliedDistance;

		Vector3 _initialPosition;
		Vector3 _maxEndPosition;

		HashSet<int> _collidedSeqNo = new HashSet<int>();

		public override void Activate()
		{
			base.Activate();
			_collidedSeqNo.Clear();
			_collidedSeqNo.Add(_unit.Info.SpawnFrom.SeqNo);

			_fliedDistance = 0;
			_initialPosition = _unit.Model.position;
			_maxEndPosition = (_unit.Model.position + _unit.Model.forward * _maxRange).ChangeY(0);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			var dis = _flySpeed * Time.deltaTime;
			_fliedDistance += dis;

			var ratio = _fliedDistance / _maxRange;
			_unit.Model.position = Vector3.Lerp(_initialPosition, _maxEndPosition, ratio);

			if (ratio > 1)
				OutOfRangeDestroy();
			
			_unit.Model.position += _unit.Model.forward * dis;
		}

		public override void ProcessCollisionEvent(CollisionEventReceiver receiver, Collision collision)
		{
			if (_collidedSeqNo.Contains(receiver.Unit.SeqNo))
			{
				return;
			}

			_collidedSeqNo.Add(receiver.Unit.SeqNo);

			var damageInfo = new DamageEvent()
			{
				Attacker = _unit.Info.SpawnFrom,
				HitPosition = _unit.Model.position,
				Direction = _unit.Model.forward,
				WeaponForceLevel = WeaponForceLevel,
			};
			DamageCalculator.CreateDamage(this, damageInfo, receiver.Unit);

			if ((int)PenetrationLevel <= (int)receiver.Unit.Body.PenetrationResist)
			{
				this.DestroyBullet();

				var fx = _hitFx.GetFX(receiver.Unit.Body.BodyMaterialType);
				if(fx != null)
					fx.Show(_unit.Model.position, _unit.Model.forward * -1);

				var se = _hitSe.GetSE(receiver.Unit.Body.BodyMaterialType);
				if(se != null)
					se.Play(_unit.Model.position);
			}
		}

		void OutOfRangeDestroy()
		{
			var groundMat = Util.GetGroundMaterial(_unit.Model.position);
			var actualHitFx = _hitFx.GetFX(groundMat);
			if(actualHitFx != null)
				actualHitFx.Show(_unit.Model.position.ChangeY(0));

			var actualHitSe = _hitSe.GetSE(groundMat);
			if(actualHitSe != null)
				actualHitSe.Play(_unit.Model.position);

			this.DestroyBullet();
		}
	}
}