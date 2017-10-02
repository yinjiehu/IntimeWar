using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Haruna.Pool;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	public class Bullet_Shell_SS : Ability, IDamageCreator, IBullet
	{
		[SerializeField]
		LayerMask _collisiontLayers;

		[SerializeField]
		WeaponForceLevelEnum _weaponForceLevel;
		public WeaponForceLevelEnum WeaponForceLevel { get { return _weaponForceLevel; } }

		[SerializeField]
		PenetrationLevelEnum _penetrationLevel;
		public PenetrationLevelEnum PenetrationLevel { get { return _penetrationLevel; } }

		[SerializeField]
		LineRenderer _line;

		[SerializeField]
		float _stretchSpeed = 100;
		[SerializeField]
		float _flySpeed = 100;

		[SerializeField]
		float _lineMaxLength = 25;

		public float MaxRange { set; get; }

		Vector3 _startPosition;
		Vector3 _finalTargetPosition;

		public enum StateEnum
		{
			NoActive,
			Stretch,
			Move,
			Shorten
		}
		StateEnum _currentState = StateEnum.NoActive;

		//[SerializeField]
		//FxHandler _hitDirtFx;
		//[SerializeField]
		//FxHandler _hitMetalFx;
		//[SerializeField]
		//FxHandler _hitStoneFx;

		//FxHandler _actualHitFx;
        [SerializeField]
        HitFx _hitFx;
        FxHandler _actualHitFx;

		[SerializeField]
		HitSE _hitSe;
		SePlayer _actualHitSe;
        
		[SerializeField]
		float _toUnitDamage = 10;
		public float ToUnitDamage { set { _toUnitDamage = value; } get { return _toUnitDamage; } }
		[SerializeField]
		float _toEnvDamage = 1;
		public float ToEnvDamage { set { _toEnvDamage = value; } get { return _toEnvDamage; } }

		bool _hitTarget;

		public void SetStartEndPosition(Vector3 startPosition, Vector3 targetPosition)
		{
			_startPosition = startPosition;
			_finalTargetPosition = targetPosition;
		}

		public virtual void Activate()
		{
			_unit.gameObject.SetActive(true);
			_currentState = StateEnum.Stretch;

			_hitTarget = false;
			Vector3 targetPosition;
            BattleUnit targetUnit = null;
            CollisionEventReceiver receiver;
            RaycastHit hit;

			var direction = (_unit.Model.position + _unit.Model.forward * MaxRange).ChangeY(0) - _unit.Model.position;
			if (Util.RaycastFirst(Unit.Model.position, direction, MaxRange, _collisiontLayers, out hit, out receiver))
            {
                if (receiver.Unit == null)
                {
                    Debug.LogErrorFormat(receiver, "collision receiver has not been initialize yet!");
                    return;
                }
                else
                {
                    targetPosition = hit.point;
                    targetUnit = receiver.Unit;
					_hitTarget = true;

					var ev = new DamageEvent()
                    {
                        HitPosition = hit.point,
                        Direction = _unit.Model.forward,
                        Attacker = _unit.Info.SpawnFrom,
                        WeaponForceLevel = WeaponForceLevelEnum.Light,
                    };
                    DamageCalculator.CreateDamage(this, ev, targetUnit);

                    _actualHitFx = _hitFx.GetFX(targetUnit.Body.BodyMaterialType);
                    _actualHitSe = _hitSe.GetSE(targetUnit.Body.BodyMaterialType);
                }
            }
            else
            {
                targetPosition = (transform.position + _unit.Model.forward * MaxRange).ChangeY();
                var groundMat = Util.GetGroundMaterial(targetPosition);
                _actualHitFx = _hitFx.GetFX(groundMat);
                _actualHitSe = _hitSe.GetSE(groundMat);

			}

			_startPosition = _unit.Model.position;
            _finalTargetPosition = targetPosition;

            _line.SetPosition(0, _startPosition);
			_line.SetPosition(1, _startPosition);
            
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

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_currentState == StateEnum.Stretch)
			{
				var currentStartPosition = _line.GetPosition(0);
				var currentEndPosition = _line.GetPosition(1);

				var newEndPosition = Util.Vector3LerpBySpeed(currentEndPosition, _finalTargetPosition, _stretchSpeed, Time.deltaTime);

				var newlength = Vector3.Distance(currentStartPosition, newEndPosition);
				if (newlength >= _lineMaxLength)
				{
					newEndPosition = currentStartPosition + (newEndPosition - currentStartPosition).normalized * _lineMaxLength;
					_currentState = StateEnum.Move;
				}
				else if (newEndPosition == _finalTargetPosition)
				{
					_currentState = StateEnum.Move;
				}
				_line.SetPosition(1, newEndPosition);
			}
			else if (_currentState == StateEnum.Move)
			{
				var currentStartPosition = _line.GetPosition(0);
				var currentEndPosition = _line.GetPosition(1);
				var newEndPosition = Util.Vector3LerpBySpeed(currentEndPosition, _finalTargetPosition, _flySpeed, Time.deltaTime);
				var newStartPosition = currentStartPosition + (newEndPosition - currentEndPosition);
				_line.SetPosition(0, newStartPosition);
				_line.SetPosition(1, newEndPosition);

				if (newEndPosition == _finalTargetPosition)
				{
					_currentState = StateEnum.Shorten;

					if (_actualHitFx != null)
					{
						if (!_hitTarget)
							_actualHitFx.Show(_finalTargetPosition.ChangeY(0));
						else
							_actualHitFx.Show(_finalTargetPosition, _unit.Model.forward * -1);
					}

					if (_actualHitSe != null)
						_actualHitSe.Play(_finalTargetPosition);
				}
			}
			else if (_currentState == StateEnum.Shorten)
			{
				var currentStartPosition = _line.GetPosition(0);
				var currentEndPosition = _line.GetPosition(1);
				var newStartPosition = Util.Vector3LerpBySpeed(currentStartPosition, currentEndPosition, _stretchSpeed, Time.deltaTime);

				_line.SetPosition(0, newStartPosition);
				if (newStartPosition == currentEndPosition)
				{
					this.DestroyBullet();
					_currentState = StateEnum.NoActive;
				}
			}
		}
	}
}