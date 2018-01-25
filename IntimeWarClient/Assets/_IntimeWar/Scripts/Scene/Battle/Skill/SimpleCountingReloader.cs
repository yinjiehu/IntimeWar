using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using Haruna.Pool;
using YJH.Unit;

namespace IntimeWar.Battle
{
    [Serializable]
    public class SimpleCountingReloader
    {
        bool _reloading;
        public bool IsReloading { get { return _reloading; } }

        public float _reloadDuration = 5;

        float _elapsedTime;
        public float ReloadingCompleteRate { get { return _elapsedTime / _reloadDuration; } }

        protected float _reloadSeconds;

        BattleUnit _unit;

        public void Init(BattleUnit unit, float reloadSeconds)
        {
            _unit = unit;

            _reloadSeconds = reloadSeconds;
            _reloadDuration = _reloadSeconds;
        }


        public void Update(float deltaTime)
        {
            if (!_unit.IsControlByThisClient)
                return;

            if (_reloading)
            {
                _elapsedTime += deltaTime;
                if (_elapsedTime > _reloadDuration)
                {
                    _elapsedTime = 0;
                    _reloading = false;
                }

            }
        }

        public void Reload()
        {
            _reloading = true;
            _elapsedTime = 0;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(_reloading);
            }
            else
            {
                _reloading = (bool)stream.ReceiveNext();
            }
        }
    }
}