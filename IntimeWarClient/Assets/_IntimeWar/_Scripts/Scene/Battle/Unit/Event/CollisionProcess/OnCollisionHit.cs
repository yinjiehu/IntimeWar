using System;
using UnityEngine;
using UnityEngine.Events;

namespace MechSquad.Battle
{
	public class OnCollisionHit : CollisionTiggerProcessor, IDamageCreator
	{
		[SerializeField]
		float _toUnitDamage;
		public float ToUnitDamage { get { return _toUnitDamage; } }

		[SerializeField]
		float _toEnvDamage;
		public float ToEnvDamage { get { return _toEnvDamage; } }
		
		public event Action<CollisionEventReceiver> EvOnHit;

		public override void ProcessCollisionEvent(CollisionEventReceiver eventTransfer, Collision collision)
		{
			EvOnHit(eventTransfer);
		}
	}
}
