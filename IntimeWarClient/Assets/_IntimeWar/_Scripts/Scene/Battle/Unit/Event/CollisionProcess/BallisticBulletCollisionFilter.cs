using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MechSquad.Battle
{
	public class BallisticBulletCollisionFilter : Ability, IExtraCollisionFilter
	{
		public virtual bool IsTaget(CollisionEventReceiver receiver)
		{
			if (receiver.Unit.Info.SeqNo == _unit.Info.SpawnFrom.SeqNo)
				return false;

			return true;
		}
	}
}
