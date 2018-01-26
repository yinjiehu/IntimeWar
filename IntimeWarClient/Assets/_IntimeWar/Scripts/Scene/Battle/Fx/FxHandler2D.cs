using Haruna.Pool;
using System;
using UnityEngine;

namespace IntimeWar
{
    public class FxHandler2D : PoolElement
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
        float _duration = 1;
        public float Duration { set { _duration = value; } get { return _duration; } }
        [SerializeField]
        bool _isFollow = false;

        float _elapsedTime;
        Transform _followTarget;

        [SerializeField]
        UnityEngine.Events.UnityEvent _onShow;

        public FxHandler2D Show()
        {
            var ret = _show();
            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }

        public FxHandler2D Show(Vector3 position)
        {
            var ret = _show();
            ret.transform.position = position;
            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }
        public FxHandler2D Show(Vector3 position, float z)
        {
            var ret = _show();
            ret.transform.position = position;

            ret.transform.localEulerAngles = new Vector3(0, 0, z);
            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }

        public FxHandler2D Show(Transform target, float z)
        {
            var ret = _show();
            ret.transform.position = target.position;
            ret._followTarget = target;
            ret.transform.localEulerAngles = new Vector3(0, 0, z);
            if (_onShow != null)
                _onShow.Invoke();
            return ret;
        }

        public FxHandler2D Show(Transform tran)
        {
            return Show(tran.position);
        }

        private FxHandler2D _show()
        {
            FxHandler2D ret = this;
            if (_destroyType == DestroyTypeEnum.UsePool)
            {
                ret = MonoPoolSingleTon.CreateFromPrefab(this) as FxHandler2D;
            }
            else if (_destroyType == DestroyTypeEnum.Destroy)
            {
                ret = Instantiate(this);
                ret.name = this.name;
            }

            ret.gameObject.SetActive(true);
            ret._elapsedTime = 0;
            ret.PlayParticleSystem();
            return ret;
        }

        public void Hide()
        {
            _followTarget = null;
            _elapsedTime = 0;

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
            if(_isFollow && _followTarget != null)
            {
                transform.position = _followTarget.position;
            }
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _duration)
            {
                Hide();
            }
        }
    }
}
