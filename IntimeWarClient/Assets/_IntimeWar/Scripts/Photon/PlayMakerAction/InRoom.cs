using HutongGames.PlayMaker;
using UnityEngine;

namespace MechSquad.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class InRoom : FsmStateAction
	{
        public FsmEvent Next;
        float _elapsedTime;

		public override void OnEnter()
		{
			base.OnEnter();
            _elapsedTime = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(PhotonNetwork.room.PlayerCount >=2)
            {
                Fsm.Event(Next);
            }
            _elapsedTime += Time.deltaTime;
            if(_elapsedTime >= 10)
            {
                Fsm.Event(Next);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}