using UnityEngine;
using System.Collections;
using System;

namespace MechSquad.Battle
{
	public class PickUpWeapon : CollisionTiggerProcessor
	{
		public override void ProcessCollisionEvent(CollisionEventReceiver eventReceiver, Collision collision)
		{
			//var item = eventReceiver.Unit.GetAbility<BattleFieldItemWeapon>();
			//if (item == null)
			//	return;

			//var weaponBattleControl = _unit.GetAbility<PlayerWeaponControlInterface>();
			//if (weaponBattleControl.TryAttachWeapon(Config.PlayerWeaponSettings.Get(item.WeaponName), item.AmmoCount))
			//{
			//	item.OnPickedUpByUnit(_unit);
			//}
		}
	}
}