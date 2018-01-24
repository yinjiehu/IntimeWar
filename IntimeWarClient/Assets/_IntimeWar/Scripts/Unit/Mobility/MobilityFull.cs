using UnityEngine;

namespace YJH.Unit
{
    public class MobilityFull : UnitMobility
    {
        [SerializeField]
        protected float _maxSpeed = 30;
        public float MaxSpeed { get { return _maxSpeed; } }

        [SerializeField]
        float _turnSpeed = 360f;

        Animator _animator;
        public override void Init()
        {
            base.Init();
            //_maxSpeed = _unit.InitialParameter.GetParameter(ConstParameter.MaxSpeed);
        }

        public override void LateInit()
        {
            base.LateInit();
            _animator = _unit.Animator;
        }

        protected override void CalculateMoving()
        {
            if (!_unit.IsControlByThisClient)
                return;

            if (_normalizedMoveDirection != Vector3.zero)
            {
                _unit.Model.GetComponent<Rigidbody2D>().velocity = new Vector2(_normalizedMoveDirection.x, _normalizedMoveDirection.y);
            }
            else
            {
                _unit.Model.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            _animator.SetFloat("VelocityX", _normalizedMoveDirection.x);
            _animator.SetFloat("VelocityY", _normalizedMoveDirection.y);
        }


    }
}