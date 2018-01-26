using UnityEngine;

namespace YJH.Unit
{
	public class DamageEvent : BattleUnitEvent
	{
		public float Damage { set; get; }
		public UnitInfo Attacker { set; get; }
		public Vector3 HitPosition { set; get; }
		public Vector3 Direction { set; get; }
        public Collider HitCollider { set; get; }
    }
}
