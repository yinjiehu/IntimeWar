using System;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class SendEventToFsm : MonoBehaviour
	{
		[SerializeField]
		string _fsmName;

		[SerializeField]
		string _eventName;

		public void SendEvent()
		{
			var fsm = PlayMakerFSM.FsmList.Find(s => s.FsmName == _fsmName);
			if(fsm == null)
			{
				Debug.LogErrorFormat(this, "Cannot find fsm named {0}", _fsmName);
				return;
			}

			if (MechSquadPreference.LogLevel > LogLevelEnum.Info)
				Debug.LogFormat(this, "Send event {0} to fsm {1}", _eventName, _fsmName);
			fsm.SendEvent(_eventName);
		}

	}
}