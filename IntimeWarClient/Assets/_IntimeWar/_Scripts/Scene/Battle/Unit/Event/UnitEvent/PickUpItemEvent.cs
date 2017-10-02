using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechSquad.Battle
{
	public class PickUpItemEvent : BattleUnitEvent
	{
		public string ItemName { set; get; }
		public int Count { set; get; }
	}

	public class PickUpWeaponEvent : PickUpItemEvent
	{
	}
}
