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
			if(receiver.Body == null)
			{
				Debug.LogErrorFormat(receiver, "Create damage to [{0}] but does not have body", receiver.name);
				Debug.Break();
				return;
			}

			if (receiver.Body.ReceiveDamageType == ReceiveDamageTypeEnum.Enviroment)
				ev.Damage = damageCreater.ToEnvDamage;
			else
				ev.Damage = damageCreater.ToUnitDamage;

			receiver.EventDispatcher.TriggerEvent(ev);
		}
	}
}