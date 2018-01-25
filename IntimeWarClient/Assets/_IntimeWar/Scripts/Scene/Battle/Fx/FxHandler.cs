using Haruna.Pool;
using System;
using UnityEngine;

namespace IntimeWar
{
    public class FxHandler : PoolElement
    {
        public enum DestroyTypeEnum
        {
            UsePool,
            Deactive,
            Destroy
        }
        [SerializeField]
        DestroyTypeEnum _destroyType;
        public DestroyTypeEnum DestroyType { set { _destroyType = value; } get { return _destroyType; } }

        [SerializeField]
        bool _forwardShowDirection;

        [SerializeField]
        float _duration = 1;
        public float Duration { set { _duration = value; } get { return _duration; } }
        [SerializeField]
        float _stopParticleDuration = 1;
        public float StopParticleDuration { set { _stopParticleDuration = value; } get { return _stopParticleDuration; } }

        [SerializeField]
        float _minHeight = 0;
        public float MinHeight { set { _minHeight = value; } get { return _minHeight; } }

        float _elapsedTime;
        float _elapsedParticleTime;

        [SerializeField]
        UnityEngine.Events.UnityEvent _onShow;

        public FxHandler Show()
        {
            var ret = _show();
            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }

        public FxHandler Show(Vector3 position)
        {
            var ret = _show();
            ret.transform.position = position;
            if (position.y < _minHeight)
                ret.transform.position = new Vector3(position.x, _minHeight, position.z);
            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }
        public FxHandler Show(Vector3 position, Vector3 direction)
        {
            var ret = _show();
            ret.transform.position = position;
            if (position.y < _minHeight)
                ret.transform.position = new Vector3(position.x, _minHeight, position.z);

            if (_forwardShowDirection)
                ret.transform.forward = direction;

            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }

        public FxHandler Show(Vector3 position, Quaternion rotation)
        {
            var ret = _show();
            ret.transform.position = position;
            if (position.y < _minHeight)
                ret.transform.position = new Vector3(position.x, _minHeight, position.z);

            if (_forwardShowDirection)
                ret.transform.rotation = rotation;

            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }
        public FxHandler Show(Transform tran)
        {
            return Show(tran.position, tran.forward);
        }

        private FxHandler _show()
        {
            FxHandler ret = this;
            if (_destroyType == DestroyTypeEnum.UsePool)
            {
                ret = MonoPoolSingleTon.CreateFromPrefab(this) as FxHandler;
            }
            else if (_destroyType == DestroyTypeEnum.Destroy)
            {
                ret = Instantiate(this);
                ret.name = this.name;
            }

            ret.gameObject.SetActive(true);
            ret._elapsedTime = 0;
            ret._elapsedParticleTime = 0;
            ret.PlayParticleSystem();
            return ret;
        }

        public void Hide()
        {
            _elapsedTime = 0;
            _elapsedParticleTime = 0;

            if (_destroyType == DestroyTypeEnum.UsePool)
            {
                gameObject.SetActive(false);
                MonoPoolSingleTon.RecycleInstance(GetComponent<PoolElement>());
            }
            else if (_destroyType == DestroyTypeEnum.Deactive)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayParticleSystem()
        {
            var particleSystem = GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }

        public void StopParticleSystem()
        {
            var particleSystem = GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            _elapsedParticleTime += Time.deltaTime;
            if (_elapsedTime > _duration)
            {
                Hide();
            }

            if (_elapsedParticleTime > _stopParticleDuration)
            {
                StopParticleSystem();
            }
        }
    }
}
