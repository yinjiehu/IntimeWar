using UnityEngine;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_MachineGun : ActiveAttachment, IWeaponTypeR, IPunObservable
	{
		[SerializeField]
		BattleUnit _bulletPrefab;
		ObjectPool _bulletPool;

		bool _activated;
		[SerializeField]
		float _activatingStopDelay = 0.2f;
		float _activateElapsedTime;

		[SerializeField]
		GameObject _muzzleFxPrefab;
		FxHandler _muzzleFxInstance;

		[SerializeField]
		Transform _muzzlePosition;
		[SerializeField]
		SePlayer _muzzleSeLoop;
		SePlayer _muzzleSeInstance;

		[SerializeField]
		SePlayer _warmingUpSeLoop;
		SePlayer _warmingUpSeInstance;

		[SerializeField]
		SePlayer _emptySe;

		public override bool ShowDragingCursor { get { return true; } }
		public override bool CanConnectToMainFireControl { get { return true; } }
		[SerializeField]
		bool _isConnectedToMainFireControl = true;
		public override bool IsConnectedToMainFireControl { get { return _isConnectedToMainFireControl; } }

		public override bool ShowCartridge { get { return true; } }
		public override bool ShowTotalAmmo { get { return true; } }

		public override float CartridgeCapacity { get { return _reloader._cartridgeCapacity; } }

		[SerializeField]
		LayerMask _collisiontLayers;

		[Header("Running Parameter")]
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _fireInterval = 0.2f;
		float _fireElapsedTime;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _powerToUnit;
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _powerToEnv;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _maxRange;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _warmingUp;
		float _warmingUpElapsedTime;

		[SerializeField]
		CartridgeReloader _reloader;
		public override float CurrentAmmoCountInCartridge { get { return _reloader._ammoInCartridge; } }
		public override float CurrentAmmoCountInTotal { get { return _reloader.TotalAmmoExceptCartridge; } }

		public override ReloadStateEnum ReloadingState { get { return _reloader.State; } }
		public override float ReloadingCompleteRate { get { return _reloader.ReloadingCompleteRate; } }

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _scatterRange = 1;
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _scatterAngle = 10;

		[SerializeField]
		protected int _maxActivatingSeCount = 1;

		KeepTriggerHelper _fireSeKeeper;
		KeepTriggerHelper _warmingUpSeKeeper;
		
		public override void Init()
		{
			base.Init();

			_bulletPool = new ObjectPool(_bulletPrefab.GetComponent<PoolElement>());

			InitFireFxInstance();
			InitFireSeKeeper();
			InitParameter();
		}

		void InitFireFxInstance()
		{
			var fx = Instantiate(_muzzleFxPrefab);
			fx.name = _muzzleFxPrefab.name;
			fx.transform.SetParent(_unit.transform);
			fx.transform.position = transform.position;
			fx.transform.rotation = transform.rotation;
			fx.gameObject.SetActive(false);

			_muzzleFxInstance = fx.AddComponent<FxHandler>();
			_muzzleFxInstance.DestroyType = FxHandler.DestroyTypeEnum.Deactive;
			_muzzleFxInstance.Duration = _fireInterval * 1.1f;
		}

		void InitFireSeKeeper()
		{
			//pre instantiate se player.
			//{
			//	var instance = SeManager.Instance.GetInstanceFromPrefab(_muzzleSeLoop, _unit.name);
			//	instance.Sid = _unit.name;
			//	SeManager.Instance.RecycleInstance(instance, _unit.name);
			//}
			//{
			//	var instance = SeManager.Instance.GetInstanceFromPrefab(_warmingUpSeLoop, _unit.name);
			//	instance.Sid = _unit.name;
			//	SeManager.Instance.RecycleInstance(instance, _unit.name);
			//}

			_warmingUpSeKeeper = new KeepTriggerHelper()
			{
				KeepMinInterval = 0.15f,
				OnKeep = () =>
				{
					if (_warmingUpSeInstance == null)
					{
						_warmingUpSeInstance = _warmingUpSeLoop.Play(transform, _unit.name);
					}
				},
				OnStop = () =>
				{
					if (_warmingUpSeInstance != null)
					{
						_warmingUpSeInstance.CancelImmidiately();
						_warmingUpSeInstance = null;
					}
				}
			};

			_fireSeKeeper = new KeepTriggerHelper()
			{
				KeepMinInterval = 0.15f,
				OnKeep = () =>
				{
					if (_muzzleSeInstance == null)
					{
						if (UnityEngine.Random.Range(0, 5) == 0)
							_muzzleSeInstance = _muzzleSeLoop.Play(transform, _unit.name);
					}
				},
				OnStop = () =>
				{
					if (_muzzleSeInstance != null)
					{
						_muzzleSeInstance.CancelImmidiately();
						_muzzleSeInstance = null;
					}
				}
			};
		}

		void InitParameter()
		{
			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(AttachmentID).ExtraParameters;

			_powerToUnit = parameters[ConstParameter.PowerToUnit];
			_powerToEnv = parameters[ConstParameter.PowerToEnv];
			_maxRange = parameters[ConstParameter.MaxRange];
			_warmingUp = parameters[ConstParameter.WarmingUp];

			_fireInterval = 1 / parameters[ConstParameter.ShootRate];

			_reloader.Init(_unit, this, AttachToSlotID);

			var accurary = parameters[ConstParameter.Accuracy];
			_scatterAngle = (100 - accurary) / 5f;
			_scatterRange = (100 - accurary) / 4f;
		}

		public void ConnectToMainFireControl()
		{
			_isConnectedToMainFireControl = true;
		}

		public void DisconnectFromMainFireControl()
		{
			_isConnectedToMainFireControl = false;
			_reloader.Reload();
		}

		public virtual void StartActivate()
		{
			if (_isConnectedToMainFireControl && _reloader.State == ReloadStateEnum.None && _reloader._ammoInCartridge <= 0 && _unit.IsPlayerForThisClient)
			{
				_emptySe.Play(transform);
			}
		}

		public virtual void Activating(int delayIndex)
		{
			if (_unit.IsControlByThisClient)
			{
				if (!_isConnectedToMainFireControl)
					return;

				if (ReloadingState == ReloadStateEnum.ForceReloading)
					return;

				if (ReloadingState == ReloadStateEnum.CancelableReloading)
				{
					_reloader.Cancel();
				}

				if (!_activated)
					_activateElapsedTime = _fireInterval - (_fireInterval * ((delayIndex % _maxActivatingSeCount) / (float)_maxActivatingSeCount));

				_activated = true;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			_reloader.Update(Time.deltaTime);
			_fireSeKeeper.Update(Time.deltaTime);
			_warmingUpSeKeeper.Update(Time.deltaTime);
		}

		private void LateUpdate()
		{
			if (_unit != null && _unit.Initialized)
			{
				//SyncFireCheck();
				UpdateMuzzleFxTransform();
			}
		}

		private void FixedUpdate()
		{
			if (_unit != null && _unit.Initialized)
			{
				FireCheck();
			}
		}

		void UpdateMuzzleFxTransform()
		{
			_muzzleFxInstance.transform.position = transform.position;
			_muzzleFxInstance.transform.rotation = transform.rotation;
		}

		void FireCheck()
		{
			_fireElapsedTime += Time.fixedDeltaTime;

			if (_activated)
			{
				if(_warmingUpElapsedTime < _warmingUp)
				{
					_warmingUpSeKeeper.Keep();
				}
				else
				{
					if (_fireElapsedTime > _fireInterval)
					{
						if (_reloader._ammoInCartridge > 0)
						{
							Fire();
							_fireElapsedTime = 0;

							_reloader.ConsumeAmmo();
						}
						else if (_reloader.TotalAmmoExceptCartridge > 0)
						{
							_reloader.Reload();
						}
					}
				}

				_warmingUpElapsedTime += Time.fixedDeltaTime;
				_activateElapsedTime += Time.fixedDeltaTime;

				if (_activateElapsedTime > _activatingStopDelay)
				{
					_activated = false;
					_activateElapsedTime = 0;
					_warmingUpElapsedTime = 0;
				}
			}
		}
		
		void Fire()
		{
			var randomX = UnityEngine.Random.Range(-_scatterAngle, _scatterAngle);
			Vector3 actualDirection = Quaternion.Euler(0, randomX, 0) * _muzzlePosition.forward;

			var actualRange = _maxRange + _scatterRange * UnityEngine.Random.Range((float)-1, (float)1);


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
            u.Model.transform.position = _muzzlePosition.position;
            u.Model.transform.forward = actualDirection;

            var bullet = u.GetAbility<Bullet_Shell_SS>();
            bullet.ToUnitDamage = _powerToUnit;
			bullet.ToEnvDamage = _powerToEnv;
            bullet.MaxRange = actualRange;
            
			bullet.Activate();

			_warmingUpSeKeeper.Keep();
			_fireSeKeeper.Keep();
			_muzzleFxInstance.Show();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			_reloader.OnPhotonSerializeView(stream, info);
			if (stream.isWriting)
			{
				stream.SendNext(_activated);
			}
			else
			{
				_activated = (bool)stream.ReceiveNext();
				if (_activated)
					_activateElapsedTime = 0;
			}
		}
	}
}