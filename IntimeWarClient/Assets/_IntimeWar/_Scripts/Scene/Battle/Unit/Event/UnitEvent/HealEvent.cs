using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechSquad.Battle
{
	public class HealEvent : BattleUnitEvent
	{
		public float HealedHp { set; get; }
		public UnitInfo Healer { set; get; }

		public bool ShouldDestroyBullet { set; get; }
	}
}
