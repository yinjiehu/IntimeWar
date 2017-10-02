using Haruna.Pool;
using Haruna.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad.Battle
{
	public class Bullet_Laser : Ability, IDamageCreator
	{
		[SerializeField]
		WeaponForceLevelEnum _weaponForceLevel;
		public WeaponForceLevelEnum WeaponForceLevel { get { return _weaponForceLevel; } }

		[SerializeField]
		PenetrationLevelEnum _penetrationLevel;
		public PenetrationLevelEnum PenetrationLevel { get { return _penetrationLevel; } }

		[SerializeField]
		protected HitFx _hitFx;
		[SerializeField]
		protected HitSE _hitSe;

		[SerializeField]
		float _toUnitDamage = 10;
		public float ToUnitDamage { set { _toUnitDamage = value; } get { return _toUnitDamage; } }
		[SerializeField]
		float _toEnvDamage = 1;
		public float ToEnvDamage { set { _toEnvDamage = value; } get { return _toEnvDamage; } }


		[SerializeField]
		QuadMesh _quadMesh;

		float _maxRange;
		public float MaxRange { set { _maxRange = value; } get { return _maxRange; } }

		BattleUnit _hitTarget;

		public void Activate()
		{
			_unit.gameObject.SetActive(true);
			_hitTarget = null;
			RaycastHit hit;
			CollisionEventReceiver receiver;
			var direction = (_unit.Model.position + _unit.Model.forward * MaxRange).ChangeY(0) - _unit.Model.position;
			if (Util.RaycastFirst(Unit.Model.position, direction, MaxRange, ~(1 << 0), out hit, out receiver))
			{
				if (receiver.Unit != null)
				{
					var ev = new DamageEvent()
					{
						HitPosition = hit.point,
						Direction = _unit.Model.forward,
						Attacker = _unit.Info.SpawnFrom,
						WeaponForceLevel = WeaponForceLevelEnum.Light,
					};
					DamageCalculator.CreateDamage(this, ev, receiver.Unit);

					var fx = _hitFx.GetFX(receiver.Unit.Body.BodyMaterialType);
					if(fx != null)
						fx.Show(hit.point, _unit.Model.forward * -1);


					var se = _hitSe.GetSE(receiver.Unit.Body.BodyMaterialType);
					if(se != null)
						se.Play(_unit.Model.position);
				}
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			RaycastHit hit;
			if (Physics.Raycast(_unit.Model.position, _unit.transform.forward, out hit, MaxRange))
			{
				var dis = Vector3.Distance(_unit.Model.position, hit.point);
				_quadMesh._uvRatioWidth = dis;
			}
			else
			{
				_quadMesh._uvRatioWidth = MaxRange;
			}

		}

		public virtual void DestroyBullet()
		{
			var poolElement = _unit.GetComponent<PoolElement>();
			if (poolElement != null)
			{
				_unit.gameObject.SetActive(false);
				poolElement.RecycleThis();
			}
			else
			{
				_unit.OnUnitDestroy();
				Destroy(_unit.gameObject);
			}
		}
	}
}