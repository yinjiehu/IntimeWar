using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MechSquad.Battle
{
	public interface IExtraCollisionFilter
	{
		bool IsTaget(CollisionEventReceiver receiver);
	}
}
