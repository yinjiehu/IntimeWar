using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.Battle
{
    [ActionCategory("MechSquad_Photon")]
    public class SwitchSynchronizeState : FsmStateAction
    {
        public FsmEvent _onPlayerStateChanged;

        public override void OnEnter()
        {
            base.OnEnter();
            SpawnerManager.Instance.EnableSpawnSynchronization();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Fsm.Event(_onPlayerStateChanged);
        }
    }
}