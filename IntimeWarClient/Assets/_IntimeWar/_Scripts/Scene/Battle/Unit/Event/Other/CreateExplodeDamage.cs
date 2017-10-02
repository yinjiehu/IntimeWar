using System.Collections.Generic;
using UnityEngine;

namespace MechSquad.Battle
{
	public class CreateExplodeDamage : Ability
	{
		[SerializeField]
		float _damage = 100;

		[SerializeField]
		float _radius = 20;
		[SerializeField]
		LayerMask _targetLayers;
		
		List<int> _hittedList = new List<int>();

		public void DoExplode()
		{
			var hits = Physics.OverlapSphere(_unit.Model.position, _radius, _targetLayers.value);
			foreach(var hit in hits)
			{
				var t = hit.GetComponent<CollisionEventReceiver>();
				if(t != null && !_hittedList.Contains(t.Unit.SeqNo))
				{
					var damageEvent = new DamageEvent()
					{
						Attacker = _unit.IsSpawnFromUnit ? _unit.Info.SpawnFrom : _unit.Info,
						Damage = _damage
					};

					if(t.Unit.EventDispatcher != null)
						t.Unit.EventDispatcher.TriggerEvent(damageEvent);

					_hittedList.Add(t.Unit.SeqNo);
				}
			}
		}
	}
}
