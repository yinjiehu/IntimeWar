using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_AutoCanon : AbstractWeaponR, IWeaponTypeR, IPunObservable
	{
		protected override void Fire()
		{
			var randomX = UnityEngine.Random.Range(-_scatterAngle, _scatterAngle);
			//Vector3 direction = Quaternion.Euler(0, randomX, 0) * transform.forward;
			//direction.Normalize();
			var actualRange = _maxRange + _scatterRange * UnityEngine.Random.Range((float)-1, (float)1);

			var b = _bulletPool.GetInstance();
			var u = b.GetComponent<BattleUnit>();
			if (!u.Initialized)
			{
				u.Init(new BattleUnit.UnitCreateArgs()
				{
					SpawnFrom = _unit.Info,
					Team = _unit.Team,
				});
			}
			u.Model.localScale = _bulletPrefab.transform.localScale;
			u.Model.rotation = _muzzlePosition.rotation * Quaternion.Euler(0, randomX, 0);
			u.Model.position = _muzzlePosition.position + u.Model.forward * _muzzleFxDistance;

			var bullet = u.GetAbility<Bullet_Shell_S>();

			bullet.ToUnitDamage = _powerToUnit;
			bullet.ToEnvDamage = _powerToEnv;
			bullet.MaxRange = actualRange;
			bullet.Activate();

			_muzzleSe.Play(_muzzlePosition.position);
			_muzzleFxPrefab.Show(_muzzlePosition);
		}
	}
}