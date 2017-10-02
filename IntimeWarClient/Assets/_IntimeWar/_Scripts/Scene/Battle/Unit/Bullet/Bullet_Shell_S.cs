using UnityEngine;
using Haruna.Pool;
using Haruna.Utility;

namespace MechSquad.Battle
{
	public class Bullet_Shell_S : Ability, IDamageCreator, IBullet
	{
		[SerializeField]
		LayerMask _collisiontLayers;

		[SerializeField]
		WeaponForceLevelEnum _weaponForceLevel;
		public WeaponForceLevelEnum WeaponForceLevel { get { return _weaponForceLevel; } }

		[SerializeField]
		Renderer _head;
		[SerializeField]
		LineRenderer _trail;

		[SerializeField]
		float _speed = 300;

		public float MaxRange { set; get; }

		public enum StateEnum
		{
			NotActive,
			Flying,
			SmokeFadeOut,
		}
		StateEnum _currentState = StateEnum.NotActive;

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

		Vector3 _trailStartPosition;

		Vector3 _hitPosition;
		BattleUnit _hitTarget;
		float _movedDistance;
		float _toMoveDistance;

		[SerializeField]
		float _trailFadeOutDuration = 2f;

		//float _trailStartFadeOutElapsedTime = 0;
		float _trailEndFadeOutElapsedTime = 0;

		[SerializeField]
		float _initialWidth = 1;
		[SerializeField]
		float _finishiWidth = 3;

		public virtual void Activate()
		{
			_unit.gameObject.SetActive(true);
			_currentState = StateEnum.Flying;

			_hitTarget = null;
			_toMoveDistance = MaxRange;

			_movedDistance = 0;

			_head.enabled = true;

			_trailStartPosition = _unit.Model.position;

			_trail.SetPosition(0, _trailStartPosition);
			_trail.SetPosition(1, _unit.Model.position);
			_trail.startWidth = _trail.endWidth = _initialWidth;

			//_trailStartFadeOutElapsedTime = 0;
			_trailEndFadeOutElapsedTime = 0;
			_trail.startColor = Util.ChangeColorAlpha(_trail.startColor, 1);
			_trail.endColor = Util.ChangeColorAlpha(_trail.endColor, 1);

			RaycastHit hit; CollisionEventReceiver receiver;
			var direction = (_unit.Model.position + _unit.Model.forward * MaxRange).ChangeY(0) - _unit.Model.position;
			if (Util.RaycastFirst(_unit.Model.position, direction, MaxRange, _collisiontLayers, out hit, out receiver))
			{
				_hitPosition = hit.point;
				_hitTarget = receiver.Unit;
				_actualHitFx = _hitFx.GetFX(_hitTarget.Body.BodyMaterialType);
				_actualHitSe = _hitSe.GetSE(_hitTarget.Body.BodyMaterialType);

				_toMoveDistance = Vector3.Distance(_trailStartPosition, _hitPosition);

				var damage = new DamageEvent()
				{
					Attacker = _unit.Info.SpawnFrom,
					Direction = _unit.transform.forward,
					HitPosition = _hitPosition,
					WeaponForceLevel = _weaponForceLevel,
				};
				DamageCalculator.CreateDamage(this, damage, _hitTarget);
			}
			else
			{
				_hitPosition = _trailStartPosition + _unit.Model.forward * MaxRange;
				var groundMaterial = Util.GetGroundMaterial(_hitPosition);
				_actualHitFx = _hitFx.GetFX(groundMaterial);
				_actualHitSe = _hitSe.GetSE(groundMaterial);
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_currentState == StateEnum.Flying)
			{
				var moveDistance = _speed * Time.deltaTime;
				_movedDistance += moveDistance;
				if (_movedDistance > _toMoveDistance)
				{
					_unit.Model.position = _hitPosition;

					if (_actualHitFx != null)
					{
						if (_hitTarget == null)
							_actualHitFx.Show(_hitPosition.ChangeY(0));
						else
							_actualHitFx.Show(_hitPosition, _unit.Model.forward * -1);
					}

					if (_actualHitSe != null)
						_actualHitSe.Play(_hitPosition);

					_currentState = StateEnum.SmokeFadeOut;
					_head.enabled = false;
				}
				else
				{
					_unit.Model.position += _unit.Model.forward * moveDistance;
				}

				_trail.SetPosition(1, _unit.Model.position);
				var length = Vector3.Distance(_trailStartPosition, _unit.Model.position);
				_trail.material.SetTextureScale("_MainTex", new Vector2(length / MaxRange, 1));
			}
			else if (_currentState == StateEnum.SmokeFadeOut)
			{
				if (_trailEndFadeOutElapsedTime < _trailFadeOutDuration)
				{
					_trailEndFadeOutElapsedTime += Time.deltaTime;
					float endAlpha = Mathf.Lerp(1, 0, _trailEndFadeOutElapsedTime / _trailFadeOutDuration);
					_trail.startColor = Util.ChangeColorAlpha(_trail.startColor, endAlpha);
					_trail.endColor = Util.ChangeColorAlpha(_trail.endColor, endAlpha);

					var width = Mathf.Lerp(_initialWidth, _finishiWidth, _trailEndFadeOutElapsedTime / _trailFadeOutDuration);
					_trail.startWidth = _trail.endWidth = width;

					_trail.SetPosition(1, _unit.Model.position);
				}
				else
				{
					DestroyBullet();
				}
			}

			//if (_trailStartFadeOutElapsedTime < _trailFadeOutDuration)
			//{
			//    _trailStartFadeOutElapsedTime += Time.deltaTime;
			//    float startAlpha = Mathf.Lerp(1, 0, _trailStartFadeOutElapsedTime / _trailFadeOutDuration);
			//    _trail.startColor = Util.ChangeColorAlpha(_trail.startColor, startAlpha);
			//}
		}

		public virtual void DestroyBullet()
		{
			_currentState = StateEnum.NotActive;

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
	}
}