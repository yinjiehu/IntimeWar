using UnityEngine;

namespace YJH.Unit
{
    public class MainFsm : Ability
    {
        [SerializeField]
        PlayMakerFSM _mainFsm;

        public override void SetupInstance(BattleUnit unit)
        {
            base.SetupInstance(unit);

            _unit.Fsm = _mainFsm;
        }

    }
}