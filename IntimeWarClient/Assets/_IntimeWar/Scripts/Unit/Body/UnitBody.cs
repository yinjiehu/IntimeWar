using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace YJH.Unit
{
    public enum BodyMaterialTypeEnum
    {
        Metal = 0,
        Stone = 1,
        Dirt = 90,
        Water = 91,
        Other = 99,
    }

    public enum PenetrationResistEnum
    {
        Weak,
        Medium,
        Strong,
    }

    public enum ReceiveDamageTypeEnum
    {
        Common,
        Enviroment,
    }

    public abstract class UnitBody : Ability, IPunObservable
    {
        [SerializeField]
        ReceiveDamageTypeEnum _receiveDamageType;
        public virtual ReceiveDamageTypeEnum ReceiveDamageType { get { return _receiveDamageType; } }

        [SerializeField]
        PenetrationResistEnum _penetrationResist;
        public virtual PenetrationResistEnum PenetrationResist { get { return _penetrationResist; } }

        [SerializeField]
        BodyMaterialTypeEnum _bodyMaterialType;
        public virtual BodyMaterialTypeEnum BodyMaterialType { get { return _bodyMaterialType; } }

        [SerializeField]
        protected float _maxHp;
        public virtual float MaxHp { get { return _maxHp; } }

        [SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
        protected float _currentHp;
        public virtual float CurrentHp { get { return _currentHp; } }

        public virtual bool IsBackDamage(Vector3 hitPosition)
        {
            return false;
        }

        public virtual void ResetHp()
        {
            _currentHp = _maxHp;
        }

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isReading)
            {
                _currentHp = (float)stream.ReceiveNext();
            }
            else
            {
                stream.SendNext(_currentHp);
            }
        }

        public override void BeforeDestroy()
        {
            base.BeforeDestroy();
        }

    }
}