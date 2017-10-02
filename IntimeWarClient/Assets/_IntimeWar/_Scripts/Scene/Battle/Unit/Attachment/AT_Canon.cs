using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_Canon : ActiveAttachment, IWeaponTypeS, IPunObservable
	{
		public override bool ShowDragingCursor { get { return false; } }
		public override bool CanConnectToMainFireControl { get { return false; } }

		[SerializeField]
		PoolElement _bulletPrefab;
		ObjectPool _bulletPool;

		[SerializeField]
		Transform _muzzlePosition;
		[SerializeField]
		FxHandler _muzzleFxPrefab;
		ObjectPool _muzzleFxPool;

		[SerializeField]
		SePlayer _muzzleSe;
		[SerializeField]
		SePlayer _emptySe;

		public override bool ShowCartridge { get { return false; } }
		public override bool ShowTotalAmmo { get { return true; } }
		
		public override float CurrentAmmoCountInTotal { get { return _reloader.TotalAmmo; } }
		public override float ReloadingCompleteRate { get { return _reloader.ReloadingCompleteRate; } }

		public override ReloadStateEnum ReloadingState { get { return _reloader.IsReloading ? ReloadStateEnum.ForceReloading : ReloadStateEnum.None; } }

		[SerializeField]
		float _powerToUnit;
		[SerializeField]
		float _powerToEnv;

		[SerializeField]
		float _maxRange;

		[SerializeField]
		SimpleReloader _reloader;

		UnitAttachmentManager _attachmentManager;

		public override void Init()
		{
			base.Init();
			_bulletPool = new ObjectPool(_bulletPrefab);
			_muzzleFxPool = new ObjectPool(_muzzleFxPrefab);

			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(AttachmentID).ExtraParameters;

			_powerToUnit = parameters[ConstParameter.PowerToUnit];
			_powerToEnv = parameters[ConstParameter.PowerToEnv];
			_maxRange = parameters[ConstParameter.MaxRange];

			_reloader.Init(_unit, this, AttachToSlotID);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			_reloader.Update(Time.deltaTime);
		}

		public virtual void Activate()
		{
			if (_unit.IsControlByThisClient && !_reloader.IsReloading)
			{
				if (_reloader.TotalAmmo > 0)
				{
					CallRPC("Fire");
					_reloader.Reload();
				}
				else
				{
					_emptySe.Play(transform, _unit.name);
				}
			}
		}
		
		[PunRPC]
		void Fire()
		{
			//var muzzleFx = _muzzleFxPool.GetInstance() as FxHandler;
			_muzzleFxPrefab.Show(_muzzlePosition);
			_muzzleSe.Play(_muzzlePosition, _unit.name);

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

			u.Model.position = _muzzlePosition.position;
			u.Model.rotation = CalcCorrectionShootDirectionByMuzzle(_maxRange, _muzzlePosition, _unit.Model.position);

			var canon = u.GetAbility<Bullet_Canon>();
			canon.ToUnitDamage = _powerToUnit;
			canon.ToEnvDamage = _powerToEnv;
			canon.MaxRange = _maxRange;
			canon.Activate();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			_reloader.OnPhotonSerializeView(stream, info);
		}

		public void Reload()
		{
			//do nothing
		}
	}
}