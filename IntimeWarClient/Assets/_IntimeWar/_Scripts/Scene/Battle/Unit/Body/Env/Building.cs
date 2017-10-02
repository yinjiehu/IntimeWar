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
	public class Building : UnitBody
	{
		//DamageEvent _lastDamageEvent;
		//public DamageEvent LastDamageEvent { get { return _lastDamageEvent; } }

		[SerializeField]
		GameObject[] _appearances;

		[SerializeField]
		RandomPlayFx[] _destroyFx;
		[SerializeField]
		FxHandler _commonDestroyFx;

		[SerializeField]
		SePlayer _phaseChangeSE;

		byte _currentPhase;

		float _previousHp;

		public override void Init()
		{
			_currentHp = _previousHp = _maxHp;
			ChangeAppearance(0);
		}

		public override void LateInit()
		{
			base.LateInit();
			_unit.EventDispatcher.AddReceiveEventType(typeof(DamageEvent));
			_unit.EventDispatcher.ReigstEvent<DamageEvent>(OnDamage, 0);
			_unit.EventDispatcher.AddReceiveEventType(typeof(TitanFallStrikeEvent));
			_unit.EventDispatcher.ReigstEvent<TitanFallStrikeEvent>(OnTitanFallStrike, 0);
		}

		void OnDamage(DamageEvent info, EventControl control)
		{
			if (_unit.IsControlByThisClient)
			{
				_currentHp -= info.Damage;
				if (_currentHp < 0)
				{
					_currentHp = 0;

				}
			}
		}
		void OnTitanFallStrike(TitanFallStrikeEvent info, EventControl control)
		{
			if (_unit.IsControlByThisClient)
			{
				_currentHp = 0;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_previousHp != _currentHp)
			{
				byte previousPhase = GetPhase(_previousHp, MaxHp, _appearances.Length - 1);
				byte currentPhase = GetPhase(_currentHp, MaxHp, _appearances.Length - 1);

				if (previousPhase != currentPhase)
				{
					ChangeAppearance(currentPhase);
					if (_unit.IsControlByThisClient)
					{
						var toDisplayPhases = new List<byte>();
						for(var i = previousPhase; i < currentPhase; i++)
						{
							toDisplayPhases.Add(i);
						}
						this.CallRPC("RPCShowDestroyFx", toDisplayPhases.ToArray());
					}
				}
				_previousHp = _currentHp;
			}
		}

		static byte GetPhase(float currentHp, float maxHp, int appearancesLength)
		{
			return (byte)Mathf.FloorToInt((maxHp - currentHp) / (maxHp / appearancesLength));
		}

		void ChangeAppearance(byte phase)
		{
			if (phase >= _appearances.Length)
			{
				Debug.LogErrorFormat(this, "phase [{0}] is not match the appearance length [{1}]", phase, _appearances.Length);
				return;
			}

			for (var i = 0; i < _appearances.Length; i++)
			{
				_appearances[i].SetActive(i == phase);
			}
		}
		
		void RPCShowDestroyFx(byte[] phase)
		{
			for (var i = 0; i < phase.Length; i++)
			{
				var fx = _destroyFx[i];
				fx.Show(_unit.Model);
			}
			_commonDestroyFx.Show(_unit.Model.position);
			_phaseChangeSE.Play(_unit.Model.position);
		}
	}
}