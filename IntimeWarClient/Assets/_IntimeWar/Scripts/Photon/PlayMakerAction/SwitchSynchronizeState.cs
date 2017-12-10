using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class SwitchSynchronizeState : FsmStateAction
	{
        public FsmEvent _onPlayerStateChanged;

        public override void OnEnter()
        {
            base.OnEnter();
            PhotonCustomEventSender.RaiseResendInstantiateEvent();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (PhotonNetwork.player.IsInSynchronization())
                Fsm.Event(_onPlayerStateChanged);

        }

        //public override void OnUpdate()
        //{
        //    base.OnUpdate();
        //    //Debug.Log(IronFuryHot.PhotonHelper.GetBattlePrepairElapsedTime() + "  enable le =============");
        //}

    }
}