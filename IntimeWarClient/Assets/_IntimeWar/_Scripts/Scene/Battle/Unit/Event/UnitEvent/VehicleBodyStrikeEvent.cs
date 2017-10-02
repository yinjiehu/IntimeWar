using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MechSquad.Battle
{
	public class VehicleBodyStrikeEvent : BattleUnitEvent
	{
		public Vector3 SourcePosition { set; get;}
		public UnitInfo Attacker { set; get; }
	}
}
