using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public abstract class AbstractWeaponR : ActiveAttachment, IWeaponTypeR, IPunObservable
	{
		protected bool _activated;
		[SerializeField]
		protected float _activateDuration = 0.15f;
		protected float _activateElapsedTime;

		[SerializeField]
		protected BattleUnit _bulletPrefab;
		protected ObjectPool _bulletPool;

		[SerializeField]
		protected FxHandler _muzzleFxPrefab;

		[SerializeField]
		protected Transform _muzzlePosition;
		[SerializeField]
		protected float _muzzleFxDistance = 1f;

		[SerializeField]
		protected SePlayer _muzzleSe;
		[SerializeField]
		protected SePlayer _emptySe;

		public override bool ShowDragingCursor { get { return true; } }
		public override bool CanConnectToMainFireControl { get { return true; } }
		[SerializeField]
		protected bool _isConnectedToMainFireControl = true;
		public override bool IsConnectedToMainFireControl { get { return _isConnectedToMainFireControl; } }

		public override bool ShowCartridge { get { return true; } }
		public override bool ShowTotalAmmo { get { return true; } }

		public override float CartridgeCapacity { get { return _reloader._cartridgeCapacity; } }

		[SerializeField]
		protected int _maxActivatingSeCount = 1;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _fireInterval = 0.2f;
		protected float _fireElapsedTime;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _powerToUnit;
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _powerToEnv;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _maxRange;

		[SerializeField]
		protected CartridgeReloader _reloader;
		public override float CurrentAmmoCountInCartridge { get { return _reloader._ammoInCartridge; } }
		public override float CurrentAmmoCountInTotal { get { return _reloader.TotalAmmoExceptCartridge; } }

		public override ReloadStateEnum ReloadingState { get { return _reloader.State; } }
		public override float ReloadingCompleteRate { get { return _reloader.ReloadingCompleteRate; } }

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _scatterRange = 1;
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _scatterAngle = 10;

		public override void Init()
		{
			base.Init();

			_bulletPool = new ObjectPool(_bulletPrefab.GetComponent<PoolElement>());
			InitParameter();
		}

		protected virtual void InitParameter()
		{
			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(AttachmentID).ExtraParameters;

			_powerToUnit = parameters[ConstParameter.PowerToUnit];
			_powerToEnv = parameters[ConstParameter.PowerToEnv];
			_maxRange = parameters[ConstParameter.MaxRange];

			_fireInterval = 1 / parameters[ConstParameter.ShootRate];
			
			_reloader.Init(_unit, this, AttachToSlotID);

			var accurary = parameters[ConstParameter.Accuracy];
			_scatterAngle = (100 - accurary) / 5f;
			_scatterRange = (100 - accurary) / 4f;
		}

		public virtual void ConnectToMainFireControl()
		{
			_isConnectedToMainFireControl = true;
		}

		public virtual void DisconnectFromMainFireControl()
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
				_activateElapsedTime = 0;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			_reloader.Update(Time.deltaTime);
		}

		private void FixedUpdate()
		{
			if (_unit != null && _unit.Initialized)
			{
				FireCheck();
			}
		}
		
		void FireCheck()
		{
			_fireElapsedTime += Time.fixedDeltaTime;

			if (_activated)
			{
				_activateElapsedTime += Time.fixedDeltaTime;

				if (_activateElapsedTime > _activateDuration)
				{
					_activated = false;
					_activateElapsedTime = 0;
				}

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
		}

		protected abstract void Fire();

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