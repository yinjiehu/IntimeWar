using IntimeWar.View;
using UnityEngine;
using View;
using YJH.Unit;

namespace IntimeWar.Unit
{
    public class HudHpDisplay : Ability
    {
        [SerializeField]
        HudInstance _hudPrefab;
        HudHp _hudHpInstance;

        float _elapsedTime;
        float _updateInterval = 0.1f;

        bool _bodyVisible;
        bool _uiVisible;

        public override void LateInit()
        {
            base.LateInit();

            var hudInstance = ViewManager.Instance.GetView<HudView>().CreateFromPrefab(_hudPrefab);
            _hudHpInstance = hudInstance.GetComponent<HudHp>();
            _hudHpInstance.SetUnitAndStartFollow(_unit, _unit.Model);
            _hudHpInstance.SetVisible(true);
        }

        public override void BeforeDestroy()
        {
            if (_hudHpInstance != null)
            {
                _hudHpInstance.SetVisible(false);
                _hudHpInstance.GetComponent<HudInstance>().RecycleInstance();
            }
        }
    }
}