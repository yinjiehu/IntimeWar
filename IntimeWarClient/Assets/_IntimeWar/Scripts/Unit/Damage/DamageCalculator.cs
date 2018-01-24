using UnityEngine;
using System.Linq;
using System;

namespace YJH.Unit
{
	public interface IDamageCreator
	{
		float ToUnitDamage { get; }
		float ToEnvDamage { get; }
	}

	public class DamageCalculator
	{
		public static void CreateDamage(IDamageCreator damageCreater, DamageEvent ev, BattleUnit receiver)
		{
			receiver.EventDispatcher.TriggerEvent(ev);
		}
	}
}