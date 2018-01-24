using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace YJH.Unit
{
	public interface IExtraCollisionFilter
	{
		bool IsTaget(CollisionEventReceiver receiver);
	}
}
