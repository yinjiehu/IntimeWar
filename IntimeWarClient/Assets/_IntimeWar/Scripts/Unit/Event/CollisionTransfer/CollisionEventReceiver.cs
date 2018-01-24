using UnityEngine;

namespace YJH.Unit
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