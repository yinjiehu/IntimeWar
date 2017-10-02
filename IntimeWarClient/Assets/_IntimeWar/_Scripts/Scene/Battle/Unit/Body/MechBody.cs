using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.Event;
using UnityEngine.Events;
using MechSquad.View;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	public class MechBody : UnitBody
	{
		DamageEvent _lastDamageEvent;
		public DamageEvent LastDamageEvent { get { return _lastDamageEvent; } }

		[SerializeField]
		HudDamage _damageDisplayHeavy;
		[SerializeField]
		HudDamage _damageDisplayLight;

		HudView _hudView;
		
		[SerializeField]
		GameObject _wreck;
		[SerializeField]
		SePlayer _destroySe;
				
		public override void Init()
		{
			_maxHp = _unit.InitialParameter.GetParameter(ConstParameter.BodyHP);
			_currentHp = _maxHp;

			_hudView = ViewManager.Instance.GetView<HudView>();

			_unit.EventDispatcher.ReigstEvent<DamageEvent>(OnDamage, 0);
			_unit.EventDispatcher.ReigstEvent<HealEvent>(OnHeal, 0);

			_unit.EventDispatcher.AddReceiveEventType(typeof(TitanFallStrikeEvent));
			_unit.EventDispatcher.ReigstEvent<TitanFallStrikeEvent>(OnTitanFallStrike, 0);

			_unit.STS.IsDead.EvOnValueChange += v =>
			{
				if (v)
					ResetToMaxHP();
			};
		}

		void OnTitanFallStrike(TitanFallStrikeEvent ev, EventControl control)
		{
			if (_unit.IsControlByThisClient)
			{
				_currentHp = 0;

				_unit.Fsm.SetState("Dead");
				this.CallRPC("RPCDestroy");
			}
		}

		void OnDamage(DamageEvent ev, EventControl control)
		{
			_lastDamageEvent = ev;

			ShowDamage(new DamageDisplayParam()
			{
				Damage = (int)ev.Damage,
				WeaponForceLevel = ev.WeaponForceLevel,
			});
			//var dmgDisplay = ev.WeaponForceLevel == WeaponForceLevelEnum.Heavy ? _damageDisplayHeavy : _damageDisplayLight;
			//var ins = _hudView.CreateFromPrefab(dmgDisplay.GetComponent<HudInstance>());
			//ins.GetComponent<HudDamage>().ShowNumber(_unit.Model, (int)ev.Damage);

			if (_unit.IsControlByThisClient)
			{
				_currentHp -= ev.Damage;

				if (_currentHp <= 0)
				{
					_unit.Fsm.SetState("Dead");
					this.CallRPC("RPCDestroy");

					BattleScene.Instance.AddKillCount(ev.Attacker.ActorID, _unit.ActorID);

					if (PhotonNetwork.room.GetGameMode() == GameModeEnum.Mode10)
					{
						PhotonHelper.SetMode10PlayerBattleState(Mode10PlayerStateEnum.Dead);
					}
				}
			}
		}

		void OnHeal(HealEvent ev, EventControl control)
		{
			_currentHp += ev.HealedHp;
			if (_currentHp > MaxHp)
				_currentHp = MaxHp;

			var ins = _hudView.CreateFromPrefab(_damageDisplayHeavy.GetComponent<HudInstance>());
			ins.GetComponent<HudDamage>().ShowNumber(_unit.Model, (int)ev.HealedHp);
		}

		public void ResetToMaxHP()
		{
			_currentHp = MaxHp;
		}

		[PunRPC]
		void RPCDestroy()
		{
			var ins = Instantiate(_wreck);
			ins.transform.position = _unit.Model.position;
			_destroySe.Play(_unit.Model.position);

			UnitManager.Instance.DestroyUnitByAttack(_unit, _unit.GetAbility<Revive>() == null);
		}

        struct DamageDisplayParam
        {
            public int Damage;
            public WeaponForceLevelEnum WeaponForceLevel;
            //public bool ShowShieldFx;
            //public Vector3 HitPosition;
            //public Vector3 Direction;
        }
        //Queue<DamageDisplayParam> _toDisplayDamage = new Queue<DamageDisplayParam>();

        void ShowDamage(DamageDisplayParam p)
		{
			var dmgDisplay = p.WeaponForceLevel == WeaponForceLevelEnum.Heavy ? _damageDisplayHeavy : _damageDisplayLight;
			var ins = _hudView.CreateFromPrefab(dmgDisplay) as HudDamage;
			ins.ShowNumber(_unit.Model, p.Damage);
			
			//if (_unit.IsControlByThisClient)
			//{
			//	_toDisplayDamage.Enqueue(p);
			//}
		}
		//public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		//{
		//	base.OnPhotonSerializeView(stream, info);

		//	if (stream.isReading)
		//	{
		//		_currentHp = (float)stream.ReceiveNext();
		//		var damageDisplayCount = (byte)stream.ReceiveNext();
		//		for (var i = 0; i < damageDisplayCount; i++)
		//		{
		//			ShowDamage(new DamageDisplayParam()
		//			{
		//				Damage = (int)stream.ReceiveNext(),
		//				WeaponForceLevel = (WeaponForceLevelEnum)(byte)stream.ReceiveNext(),
		//			});
		//		}
		//	}
		//	else
		//	{
		//		stream.SendNext(_currentHp);
		//		stream.SendNext((byte)_toDisplayDamage.Count);
		//		while (_toDisplayDamage.Count > 0)
		//		{
		//			var p = _toDisplayDamage.Dequeue();
		//			stream.SendNext(p.Damage);
		//			stream.SendNext((byte)p.WeaponForceLevel);
		//		}
		//	}
		//}
	}
}