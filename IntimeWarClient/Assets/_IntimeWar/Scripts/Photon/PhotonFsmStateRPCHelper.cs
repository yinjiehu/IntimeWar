using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using HutongGames.PlayMaker;
using System.Reflection;

namespace MechSquad.Battle
{
	[RequireComponent(typeof(PhotonView))]
	[RequireComponent(typeof(PlayMakerFSM))]
	public class PhotonFsmStateRPCHelper : MonoBehaviour
	{
		Dictionary<string, FsmStateAction> _indexToActionMapping = new Dictionary<string, FsmStateAction>();
		Dictionary<FsmStateAction, string> _actionToIndexMapping = new Dictionary<FsmStateAction, string>();
		
		PlayMakerFSM _fsm;

		private void Start()
		{
			_fsm = GetComponent<PlayMakerFSM>();
			
			for (var i = 0; i < _fsm.FsmStates.Length; i++)
			{
				var state = _fsm.FsmStates[i];

				for (var j = 0; j < state.Actions.Length; j++)
				{
					var action = state.Actions[j];

					string id;
					if (!string.IsNullOrEmpty(action.Name))
					{
						id = string.Concat(state.Name, "_", action.Name);
					}
					else
					{
						id = string.Format("{0}_$[{1}]", state.Name, j);
					}
					_indexToActionMapping.Add(id, action);
					_actionToIndexMapping.Add(action, id);
				}
			}
		}
		
		public void RaiseInstantiateEvent(FsmStateAction action, string methodName, int[] viewIds, params object[] parameters)
		{
			string id;
			if (_actionToIndexMapping.TryGetValue(action, out id))
			{
				PhotonCustomEventSender.RaiseInstantiateUnitEvent(this.GetPhotonView(), "OnReceiveInstantiateEvent", viewIds,
					new object[] { id, methodName, parameters });
			}
		}

		[PunRPC]
		void OnReceiveInstantiateEvent(int[] viewIds, string id, string methodName, object[] parameters)
		{
			var p = new object[parameters.Length + 1];
			p[0] = viewIds;
			Array.Copy(parameters, 0, p, 1, parameters.Length);

			OnReceivedRPC(id, methodName, p);
		}

		public void CallRPC(FsmStateAction action, string methodName, params object[] parameters)
		{
			string id;
			if (_actionToIndexMapping.TryGetValue(action, out id))
			{
				PhotonNetwork.RPC(this.GetPhotonView(), "OnReceivedRPC", PhotonTargets.All, false,
					new object[] { id, methodName, parameters });
			}
		}

		[PunRPC]
		public void OnReceivedRPC(string id, string methodName, object[] parameters)
		{
			FsmStateAction action;
			if (!_indexToActionMapping.TryGetValue(id, out action))
			{
				Debug.LogWarningFormat(this, "Can not get index {0} when received rpc. method name {1}. parameters {2} ",
					id, methodName, JsonUtil.Serialize(parameters));
			}

			var method = action.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				Debug.LogWarningFormat(this, "Can not get method {0} for state {1} action {2} when received rpc. {3} ",
					methodName, action.State.Name, action.Name, JsonUtil.SerializeArgs(parameters));
			}

			try
			{
				method.Invoke(action, parameters);
			}
			catch(Exception e)
			{
				Debug.LogException(e, this);
				Debug.LogWarningFormat(this, "Error when invoke rpc method {0} for state {1} action {2}. {3} ",
					methodName, action.State.Name, action.Name, JsonUtil.SerializeArgs(parameters));
			}
		}
	}
}