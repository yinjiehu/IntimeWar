using UnityEngine;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_Shotgun : AbstractWeaponR, IWeaponTypeR, IPunObservable
	{
		int _scatterCount;

		protected override void InitParameter()
		{
			base.InitParameter();

			_scatterRange = _maxRange * 0.2f;

			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(AttachmentID).ExtraParameters;
			_scatterCount = (int)parameters[ConstParameter.ScatterCount];
		}


		protected override void Fire()
		{
			//float[] angleXDelta = new float[_scatterCount];
			//float[] rangeDelta = new float[_scatterCount];
			//for (var i = 0; i < _scatterCount; i++)
			//{
			//	angleXDelta[i] = Random.Range(-_scatterAngle, _scatterAngle);
			//	rangeDelta[i] = Random.Range(-_scatterRange, _scatterRange);
			//}
			//CallRPC("RPCFire", angleXDelta, rangeDelta);
		//}

		//[PunRPC]
		//void RPCFire(float[] angleXDelta, float[] rangeDelta)
		//{
			//var muzzleFx = _muzzleFxPool.GetInstance() as FxHandler;
			_muzzleFxPrefab.Show(_muzzlePosition);
			_muzzleSe.Play(_muzzlePosition, _unit.name);

			for (var i = 0; i < _scatterCount; i++)
			{
				var angleXDelta = Random.Range(-_scatterAngle, _scatterAngle);
				var rangeDelta = Random.Range(-_scatterRange, _scatterRange);

				Vector3 direction = Quaternion.Euler(0, angleXDelta, 0) * _muzzlePosition.forward;
				float actualRange = _maxRange + rangeDelta;

				var ins = _bulletPool.GetInstance();
				var u = ins.GetComponent<BattleUnit>();
				if (!u.Initialized)
				{
					u.Init(new BattleUnit.UnitCreateArgs()
					{
						SpawnFrom = _unit.Info,
						Team = _unit.Team,
					});
				}
				u.Model.localScale = _bulletPrefab.transform.localScale;
				u.Model.position = _muzzlePosition.position;
				u.Model.forward = direction;

				var bullet = u.GetAbility<Bullet_Shell_SS>();

				bullet.ToUnitDamage = _powerToUnit;
				bullet.ToEnvDamage = _powerToEnv;
				bullet.MaxRange = actualRange;

				bullet.Activate();
			}
		}
	}
}