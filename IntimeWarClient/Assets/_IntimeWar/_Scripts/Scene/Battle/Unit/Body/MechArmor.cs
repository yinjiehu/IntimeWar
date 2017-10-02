using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.Event;
using MechSquad.View;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	public class MechArmor : Ability, IPunObservable
	{
		[SerializeField]
		float _radius = 5f;
		[SerializeField]
		HudDamage _damageDisplayHeavy;
		[SerializeField]
		HudDamage _damageDisplayLight;
		[SerializeField]
		FxHandler _hitArmorFxHeavy;
		[SerializeField]
		FxHandler _hitArmorFxLight;

		[SerializeField]
		Transform _activatingFxPrefab;
		[SerializeField]
		FxHandler _brokenFx;

		[SerializeField]
		SePlayer _brokenSe;

		HudView _hudView;
		Transform _activatingFxInstance;

		[SerializeField]
		protected float _maxHp;
		public virtual float MaxHp { get { return _maxHp; } }

		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		protected float _currentHp;
		public virtual float CurrentHp { get { return _currentHp; } }

		[SerializeField]
		float _secondsToStartRecovering;
		[SerializeField]
		float _armorRecoverSpeed;

		public enum StateEnum
		{
			Common,
			Damaged,
			Recovering,
			Broken,
		}
		StateEnum _state = StateEnum.Common;
		public bool IsArmorBroken { get { return _state == StateEnum.Broken; } }

		float _elapsedSecondsSinceLastDamaged;

		IMainFireControlSystem _fireControl;

		UnitStateEnum _previousState;

		public override void Init()
		{
			_currentHp = _maxHp = _unit.InitialParameter.GetParameter(ConstParameter.ArmorHP);
			_fireControl = _unit.GetAbility<IMainFireControlSystem>();
			_hudView = ViewManager.Instance.GetView<HudView>();
			_unit.EventDispatcher.ReigstEvent<DamageEvent>(OnDamage, 1);
		}

		public override void LateInit()
		{
			base.LateInit();

			_activatingFxInstance = Instantiate(_activatingFxPrefab);
			_activatingFxInstance.SetParent(_fireControl.TurretRoot);
			_activatingFxInstance.transform.localPosition = Vector3.zero;
			_activatingFxInstance.transform.localRotation = Quaternion.identity;
		}

		void OnDamage(DamageEvent ev, EventControl control)
		{
			if (_state == StateEnum.Broken)
			{
				_elapsedSecondsSinceLastDamaged = 0;
			}
			else
			{
				var tempHp = _currentHp;
				tempHp -= ev.Damage;

				var dmgDisplay = ev.WeaponForceLevel == WeaponForceLevelEnum.Heavy ? _damageDisplayHeavy : _damageDisplayLight;
				if (tempHp < 0)
				{
					ShowDamage(new DamageDisplayParam()
					{
						DamageCount = (int)(ev.Damage + tempHp),
						WeaponForceLevel = ev.WeaponForceLevel,
						ShowArmorFx = false,
					});

					ev.Damage = tempHp * -1;

					if (_unit.IsControlByThisClient)
					{
						_currentHp = 0;
						_state = StateEnum.Broken;
						_elapsedSecondsSinceLastDamaged = 0;

						this.CallRPC("DoBroken");
					}
				}
				else
				{
					control.CancelNextFiring = true;

					ShowDamage(new DamageDisplayParam()
					{
						DamageCount = (int)ev.Damage,
						WeaponForceLevel = ev.WeaponForceLevel,
						ShowArmorFx = true,
						HitPosition = ev.HitPosition,
						Direction = ev.Direction,
					});


					if (_unit.IsControlByThisClient)
					{
						_currentHp = tempHp;
						_state = StateEnum.Damaged;
						_elapsedSecondsSinceLastDamaged = 0;
					}
				}
			}
		}

		[PunRPC]
		void DoBroken()
		{
			_brokenFx.Show(_activatingFxInstance.position);
			_brokenSe.Play(_activatingFxInstance.position);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_unit.IsControlByThisClient)
			{
				if (_unit.IsDead)
				{
					_elapsedSecondsSinceLastDamaged = 0;
					_state = StateEnum.Common;
					_currentHp = MaxHp;
				}
				if (_state == StateEnum.Damaged || _state == StateEnum.Broken)
				{
					_elapsedSecondsSinceLastDamaged += Time.deltaTime;
					if (_elapsedSecondsSinceLastDamaged > _secondsToStartRecovering)
					{
						_state = StateEnum.Recovering;
					}
				}
				else if (_state == StateEnum.Recovering)
				{
					_currentHp += _armorRecoverSpeed * Time.deltaTime;
					if (_currentHp > MaxHp)
					{
						_currentHp = MaxHp;
						_state = StateEnum.Common;
					}
				}
			}

            if (!_unit.STS.BodyVisible.GetValue() || _state == StateEnum.Broken)
            {
                _activatingFxInstance.gameObject.SetActive(false);
            }
        }

		public void ResetHPToMax()
		{
			_currentHp = MaxHp;
		}

		struct DamageDisplayParam
		{
			public int DamageCount;
			public WeaponForceLevelEnum WeaponForceLevel;
			public bool ShowArmorFx;
			public Vector3 HitPosition;
			public Vector3 Direction;
		}
		//Queue<DamageDisplayParam> _toDisplayDamage = new Queue<DamageDisplayParam>();

		void ShowDamage(DamageDisplayParam p)
		{
			var dmgDisplay = p.WeaponForceLevel == WeaponForceLevelEnum.Heavy ? _damageDisplayHeavy : _damageDisplayLight;
			var ins = _hudView.CreateFromPrefab(dmgDisplay) as HudDamage;
			ins.ShowNumber(_unit.Model, p.DamageCount);

			if (p.ShowArmorFx)
			{
				var fx = p.WeaponForceLevel == WeaponForceLevelEnum.Heavy ? _hitArmorFxHeavy : _hitArmorFxLight;
                var position = new Vector3(_unit.Model.position.x, p.HitPosition.y, _unit.Model.position.z) + p.Direction.normalized * -1 * _radius;

                fx.Show(position, p.Direction);
			}

			//if (_unit.IsControlByThisClient)
			//{
			//	_toDisplayDamage.Enqueue(p);
			//}
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isReading)
			{
				_currentHp = (float)stream.ReceiveNext();
				_state = (StateEnum)(byte)stream.ReceiveNext();
				//var damageDisplayCount = (byte)stream.ReceiveNext();
				//for (var i = 0; i < damageDisplayCount; i++)
				//{
				//	ShowDamage(new DamageDisplayParam()
				//	{
				//		DamageCount = (int)stream.ReceiveNext(),
				//		WeaponForceLevel = (WeaponForceLevelEnum)(byte)stream.ReceiveNext(),
				//		ShowArmorFx = (bool)stream.ReceiveNext(),
				//		HitPosition = (Vector3)stream.ReceiveNext(),
				//		Direction = (Vector3)stream.ReceiveNext(),
				//	});
				//}
			}
			else
			{
				stream.SendNext(_currentHp);
				stream.SendNext((byte)_state);
				//stream.SendNext((byte)_toDisplayDamage.Count);
				//while (_toDisplayDamage.Count > 0)
				//{
				//	var p = _toDisplayDamage.Dequeue();
				//	stream.SendNext(p.DamageCount);
				//	stream.SendNext((byte)p.WeaponForceLevel);
				//	stream.SendNext(p.ShowArmorFx);
				//	stream.SendNext(p.HitPosition);
				//	stream.SendNext(p.Direction);
				//}
			}
		}
	}
}