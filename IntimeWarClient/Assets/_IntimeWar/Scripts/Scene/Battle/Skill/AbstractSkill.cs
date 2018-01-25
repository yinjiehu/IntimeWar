using System;
using IntimeWar.Battle;
using UnityEngine;
using IntimeWar;
using IntimeWar.Data;

namespace YJH.Unit
{
    public abstract class AbstractSkill : UnitAttachment, IPunObservable
    {
        public bool IsReloading { get { return _reloader.IsReloading; } }
        public float ReloadingCompleteRate { get { return _reloader.ReloadingCompleteRate; } }

        protected SimpleCountingReloader _reloader;

        public override void Init()
        {
            base.Init();

            _reloader = new SimpleCountingReloader();
            var skillID = _unit.InitialParameter.GetSkills()[int.Parse(SlotID)];
            var skillSettings = GlobalCache.GetSkillSettings().Get(skillID);

            _reloader.Init(_unit, skillSettings.ReloadSeconds);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            _reloader.Update(Time.deltaTime);
        }

        public override void BeforeDestroy()
        {
            base.BeforeDestroy();
        }

        public void Reload()
        {
            _reloader.Reload();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            _reloader.OnPhotonSerializeView(stream, info);
        }
    }
}