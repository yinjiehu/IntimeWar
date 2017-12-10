using UnityEngine;

namespace MechSquad.Battle
{
	public class CollisionEventReceiver : Ability
	{
		public void OnEvent(BattleUnitEvent effectEvent)
		{
			if(_unit != null)
				_unit.EventDispatcher.TriggerEvent(effectEvent);
		}
	}
}