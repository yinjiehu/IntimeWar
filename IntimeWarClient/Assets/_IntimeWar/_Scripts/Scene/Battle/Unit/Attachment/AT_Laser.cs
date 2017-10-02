using UnityEngine;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_Laser : AbstractWeaponR, IWeaponTypeR, IPunObservable
	{
		Bullet_Laser _bullet;

		protected override void Fire()
		{
			//direction.Normalize();
			var actualRange = _maxRange + _scatterRange * UnityEngine.Random.Range((float)-1, (float)1);


			if(_bullet == null)
			{
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
				b.transform.SetParent(_muzzlePosition);
				u.Model.localScale = _bulletPrefab.transform.localScale;
				u.Model.rotation = _muzzlePosition.rotation;
				u.Model.position = _muzzlePosition.position + u.Model.forward * _muzzleFxDistance;
				_bullet = u.GetAbility<Bullet_Laser>();

				_bullet.ToUnitDamage = _powerToUnit;
				_bullet.ToEnvDamage = _powerToEnv;
				_bullet.MaxRange = actualRange;
			}
			_bullet.Activate();
			_bullet.transform.position = _muzzlePosition.position + _bullet.transform.forward * _muzzleFxDistance;
			_muzzleSe.Play(_muzzlePosition.position);
			var muzzleFxPrefab = _muzzleFxPrefab.Show(_muzzlePosition);
			muzzleFxPrefab.transform.SetParent(_muzzlePosition);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if(_bullet != null)
			{
				if(!_activated)
				{
					_bullet.DestroyBullet();
					_bullet = null;
				}
			}
		}
	}
}