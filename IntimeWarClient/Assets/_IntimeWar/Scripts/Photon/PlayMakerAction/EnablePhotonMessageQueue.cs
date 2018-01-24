using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class EnablePhotonMessageQueue : FsmStateAction
	{
        public FsmBool _setEnable;

        public override void OnEnter()
        {
            base.OnEnter();

            //PhotonNetwork.isMessageQueueRunning = _setEnable.Value;

            Finish();
        }

        //public override void OnUpdate()
        //{
        //    base.OnUpdate();
        //    //Debug.Log(IronFuryHot.PhotonHelper.GetBattlePrepairElapsedTime() + "  enable le =============");
        //}

    }
}