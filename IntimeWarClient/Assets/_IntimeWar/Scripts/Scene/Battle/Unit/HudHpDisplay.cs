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


            OnBodyVisibleChange(_unit.STS.BodyVisible.GetValue());
            OnUIVisibleChange(_unit.STS.UIVisible.Value);
            _unit.STS.UIVisible.EvOnValueChange += OnUIVisibleChange;
            _unit.STS.BodyVisible.EvOnValueChange += OnBodyVisibleChange;

        }
        private void OnBodyVisibleChange(bool visible)
        {
            _bodyVisible = visible;
        }

        private void OnUIVisibleChange(bool visible)
        {
            if (visible || _unit.Team != PhotonNetwork.player.GetUnitTeam())
                _uiVisible = visible;

        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_hudHpInstance != null)
                _hudHpInstance.SetVisible(_bodyVisible && _uiVisible);
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