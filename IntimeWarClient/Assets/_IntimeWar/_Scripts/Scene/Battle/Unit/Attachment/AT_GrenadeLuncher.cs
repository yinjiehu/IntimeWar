using UnityEngine;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_GrenadeLuncher : ActiveAttachment, IWeaponTypeG, IPunObservable
	{
		public override bool ShowDragingCursor { get { return false; } }
		public override bool CanConnectToMainFireControl { get { return false; } }

		[SerializeField]
		Transform _muzzlePosition;
		[SerializeField]
		FxHandler _muzzleFxPrefab;
		[SerializeField]
		SePlayer _muzzleSe;
		[SerializeField]
		SePlayer _emptySe;
		public override bool ShowCartridge { get { return false; } }
		public override bool ShowTotalAmmo { get { return true; } }

		[SerializeField]
		PoolElement _bulletPrefab;
		ObjectPool _bulletPool;
		
		[SerializeField]
		SimpleReloader _reloader;
		public override float CurrentAmmoCountInTotal { get { return _reloader.TotalAmmo; } }
		public override float ReloadingCompleteRate { get { return _reloader.ReloadingCompleteRate; } }

		public override ReloadStateEnum ReloadingState { get { return _reloader.IsReloading ? ReloadStateEnum.ForceReloading : ReloadStateEnum.None; } }

		[SerializeField]
		float _elevationAngle = 45f;

		bool _isPreparing;
		public bool IsPrepairing { get { return _isPreparing; } }

		float _decideDropPointElapsedTime;
		float _cancelPreparingLimitSeconds = 0.1f;

		[SerializeField]
		DropPointHitTypeG _dropPointFxPrefab;
		DropPointHitTypeG _dropPointFxInstance;

		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _powerToUnit;
		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _powerToEnv;

		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _minRange;
		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _maxRange;

		IMainFireControlSystem _fireControl;

		public override void Init()
		{
			base.Init();
			_fireControl = _unit.GetAbility<IMainFireControlSystem>();
			_bulletPool = new ObjectPool(_bulletPrefab);

			InitParamters();

			_dropPointFxInstance = Instantiate(_dropPointFxPrefab);
			_dropPointFxInstance.name = _dropPointFxPrefab.name;
			_dropPointFxInstance.transform.SetParent(transform);
			_dropPointFxInstance.transform.position = _unit.Model.position;
			_dropPointFxInstance.MinRange = _minRange;
			_dropPointFxInstance.MaxRange = _maxRange;
			_dropPointFxInstance.Unit = _unit;
			_dropPointFxInstance.Hide();
		}

		void InitParamters()
		{
			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(AttachmentID).ExtraParameters;

			_powerToUnit = parameters[ConstParameter.PowerToUnit];
			_powerToEnv = parameters[ConstParameter.PowerToEnv];
			_minRange = parameters[ConstParameter.MinRange];
			_maxRange = parameters[ConstParameter.MaxRange];

			_reloader.Init(_unit, this, AttachToSlotID);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (_unit.IsControlByThisClient)
			{
				if (_reloader.IsReloading)
				{
					_reloader.Update(Time.deltaTime);
				}
				else if (IsPrepairing)
				{
					_decideDropPointElapsedTime += Time.deltaTime;
					if (_decideDropPointElapsedTime > _cancelPreparingLimitSeconds)
					{
						Cancel();
					}
				}
			}
		}

		public void Prepairing()
		{
			if (!_reloader.IsReloading && !_isPreparing)
			{
				_isPreparing = true;
				_dropPointFxInstance.SetMoving();
			}
			_decideDropPointElapsedTime = 0;
		}

		public void RapidFire()
		{
			if(!_reloader.IsReloading && !_isPreparing)
			{
				if(_reloader.TotalAmmo > 0)
				{
					var dropPosition = Vector3.zero;
					var target = _fireControl.GetAimingTarget();

					var actualRange = _minRange;
					if(target != null)
					{
						var distance = Vector3.Distance(_unit.Model.position, target.Model.position);
						if (distance > _maxRange)
							distance = _maxRange;
						else if (distance < _minRange)
							distance = _minRange;

						actualRange = distance;
					}

					dropPosition = _unit.Model.position + _fireControl.CurrentTurretDirection.normalized * actualRange;

					CallRPC("RPCFire", dropPosition, _muzzlePosition.position, _muzzlePosition.rotation);
					_reloader.Reload();
				}
				else
				{
					_emptySe.Play(transform, _unit.name);
				}
			}
		}

		public void Release()
		{
			if (_unit.IsControlByThisClient && _isPreparing)
			{
				if (_dropPointFxInstance.IsInMinRange())
				{
					Cancel();
				}
				else
				{
					if (_reloader.TotalAmmo > 0)
					{
						_decideDropPointElapsedTime = 0;
						_dropPointFxInstance.Hide();
						_reloader.Reload();
						_isPreparing = false;
						CallRPC("RPCFire", _dropPointFxInstance.transform.position, _muzzlePosition.position, _muzzlePosition.rotation);
					}
					else
					{
						_emptySe.Play(transform, _unit.name);
					}
				}
			}
		}

		public void Cancel()
		{
			_decideDropPointElapsedTime = 0;
			_dropPointFxInstance.Hide();
			_isPreparing = false;
		}

		[PunRPC]
		void RPCFire(Vector3 dropPoint, Vector3 muzzlePosition, Quaternion muzzleRotation)
		{
			_muzzleFxPrefab.Show(muzzlePosition, muzzleRotation * Quaternion.Euler(-_elevationAngle, 0, 0) * Vector3.forward);
			_muzzleSe.Play(muzzlePosition);

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
			u.Model.position = muzzlePosition;
			u.Model.rotation = muzzleRotation;

			var bullet = u.GetAbility<Bullet_Parabola>();
			bullet.ToUnitDamage = _powerToUnit;
			bullet.ToEnvDamage = _powerToEnv;
			bullet.DropPonit = dropPoint;
			bullet.ElevationAngle = _elevationAngle;
			bullet.Activate();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			_reloader.OnPhotonSerializeView(stream, info);
		}
	}
}