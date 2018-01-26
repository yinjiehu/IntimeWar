using UnityEngine;

namespace YJH.Unit
{
	public class CollisionEventReceiver : Ability
	{
        [SerializeField]
        bool _isPartialUnit;
        public bool IsPartialUnit { set { _isPartialUnit = value; } get { return _isPartialUnit; } }

        Collider _collider;
        public Collider Collider { get { return _collider; } }

        public override void Init()
        {
            base.Init();
            _collider = GetComponent<Collider>();
        }

        public void OnEvent(BattleUnitEvent effectEvent)
        {
            if (_unit != null)
            {
                _unit.EventDispatcher.TriggerEvent(effectEvent);
            }
        }
    }
}