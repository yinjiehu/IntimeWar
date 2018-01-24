using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YJH.Unit
{
	public class AbilityGroup : MonoBehaviour, IUnitAbility
	{
		public virtual string Name { get { return name; } }
		
		public bool IsSyncAbility { get { return false; } }

		protected BattleUnit _unit;
		public BattleUnit Unit { get { return _unit; } }
		

		public virtual void SetupInstance(BattleUnit unit)
		{
			_unit = unit;
			var abilities = GetComponentsInChildren<IUnitAbility>();
			for(var i = 0; i < abilities.Length; i++)
			{
				var a = abilities[i];
				if (a is AbilityGroup && ((AbilityGroup)a) == this)
					continue;
				_unit.AddAbilityToListOnSetup(abilities[i]);
			}
		}
		
		public virtual void Init()
		{
		}

		public void LateInit()
		{
		}
	}
}