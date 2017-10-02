using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class FsmStateSynchronizer : Ability, IPunObservable
	{
		[SerializeField]
		PlayMakerFSM _fsm;

		List<string> _states = new List<string>();

		private void Start()
		{
			var states = _fsm.Fsm.States;
			for (var i = 0; i < states.Length; i++)
			{
				_states.Add(states[i].Name);
			}
			_states.Sort();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isReading)
			{
				var index = (byte)stream.ReceiveNext();
                if (_states.Count == 0)
                    return;

                if (index >= _states.Count)
				{
					Debug.LogErrorFormat(this, "index {0} is over than states length {1}", index, _states.Count);
				}
				else
				{
					var stateName = _states[index];
					if (_fsm.ActiveStateName != stateName) 
						_fsm.SetState(stateName);
				}
			}
			else
			{
				var index = _states.FindIndex(s => s == _fsm.ActiveStateName);
                stream.SendNext((byte)index);
            }
		}
	}
}