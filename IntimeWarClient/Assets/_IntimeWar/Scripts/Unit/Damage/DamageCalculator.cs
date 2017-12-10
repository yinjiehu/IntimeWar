using UnityEngine;
using System.Linq;
using System;

namespace MechSquad.Battle
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