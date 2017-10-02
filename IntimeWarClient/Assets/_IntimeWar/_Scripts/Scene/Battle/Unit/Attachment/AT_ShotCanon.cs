using UnityEngine;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class AT_ShotCanon : ActiveAttachment, IWeaponTypeS, IPunObservable
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
		SimpleReloader _reloader;
		public override float CurrentAmmoCountInTotal { get { return _reloader.TotalAmmo; } }
		public override float ReloadingCompleteRate { get { return _reloader.ReloadingCompleteRate; } }
		
		public override ReloadStateEnum ReloadingState { get { return _reloader.IsReloading ? ReloadStateEnum.ForceReloading : ReloadStateEnum.None; } }


		[SerializeField]
		//[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _scatterAngle = 15f;
		[SerializeField]
		//[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		float _scatterRange = 12f;
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		int _scatterCount = 15;

		string _attachedAmmoID;

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

			//var accurary = parameters[ConstParameter.Accuracy];
			//_scatterAngle = 15f;
			//_scatterRange = 12f;
			
			_scatterCount = (int)parameters[ConstParameter.ScatterCount];

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
					Fire();
					_reloader.Reload();
				}
				else
				{
					_emptySe.Play(transform);
				}
			}
		}
		
		void Fire()
		{
			float[] angleXDelta = new float[_scatterCount];
			float[] rangeDelta = new float[_scatterCount];
			for (var i = 0; i < _scatterCount; i++)
			{
				angleXDelta[i] = UnityEngine.Random.Range(-_scatterAngle, _scatterAngle);
				rangeDelta[i] = UnityEngine.Random.Range(-_scatterRange, _scatterRange);
			}
			CallRPC("RPCFire", angleXDelta, rangeDelta);
		}

		[PunRPC]
		void RPCFire(float[] angleXDelta, float[] rangeDelta)
		{
			//var muzzleFx = _muzzleFxPool.GetInstance() as FxHandler;
			_muzzleFxPrefab.Show(_muzzlePosition);
			_muzzleSe.Play(_muzzlePosition, _unit.name);

			for(var i = 0; i < angleXDelta.Length; i++)
			{
				Vector3 direction = Quaternion.Euler(0, angleXDelta[i], 0) * _muzzlePosition.forward;

				float actualRange = _maxRange + rangeDelta[i];

				//Vector3 targetPosition;
				//BattleUnit targetUnit = null;
				//CollisionEventReceiver receiver;
				//RaycastHit hit;
				//if (Util.RaycastFirst(transform.position, direction, actualRange, _collisiontLayers, out hit, out receiver))
				//{
				//	if (receiver.Unit == null)
				//	{
				//		Debug.LogErrorFormat(receiver, "collision receiver has not been initialize yet!");
				//		return;
				//	}
				//	else
				//	{
				//		targetPosition = hit.point;
				//		targetUnit = receiver.Unit;
				//	}
				//}
				//else
				//{
				//	targetPosition = transform.position + direction * actualRange;
				//}

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