using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using HutongGames.PlayMaker;

namespace IntimeWar
{
	[RequireComponent(typeof(PlayMakerFSM))]
	public class PhotonMessageReceiverForPlayMakerFSM : MonoBehaviour
    {
		void Awake()
		{
			RegistedPlayMakerFsmForPhotonEvent.Add(GetComponents<PlayMakerFSM>());
		}

		void OnDestroy()
		{
			RegistedPlayMakerFsmForPhotonEvent.Remove(GetComponents<PlayMakerFSM>());
		}
    }

	public class RegistedPlayMakerFsmForPhotonEvent : MonoBehaviour
	{
		static RegistedPlayMakerFsmForPhotonEvent _instance;
		public static RegistedPlayMakerFsmForPhotonEvent Instance
		{
			get
			{
				if (_instance == null)
				{
					var go = new GameObject("RegistedPlayMakerFsmForPhotonEvent");
					_instance = go.AddComponent<RegistedPlayMakerFsmForPhotonEvent>();
				}
				return _instance;
			}
		}
		
		HashSet<PlayMakerFSM> _playMakerFsms = new HashSet<PlayMakerFSM>();
		public HashSet<PlayMakerFSM> All { get { return _playMakerFsms; } }

		public static void Add(PlayMakerFSM[] fsms)
		{
			Instance._playMakerFsms.UnionWith(fsms);
		}
		public static void Remove(PlayMakerFSM[] fsms)
		{
			if (_instance == null)
				return;

			_instance._playMakerFsms.ExceptWith(fsms);
			_instance._playMakerFsms.RemoveWhere(t => t == null);
		}

		public void SendPhotonEvent(string methodName, object[] parameters)
		{
			using(var itr = _playMakerFsms.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					var fsm = itr.Current;
					if (fsm.gameObject.activeInHierarchy && fsm.Active)
					{
						var activeState = fsm.Fsm.ActiveState;
						var actions = new List<FsmStateAction>(activeState.Actions);
						if (actions.All(
							action => !SendPhotonEventToTask(fsm, action, methodName, parameters)))
						{
							if(PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
								Debug.LogFormat(fsm, "photon message has no receiver. {0} {1} {2}", fsm.name, methodName, parameters.ToStringFull());
						}
					}
				}
			}
		}

		public bool SendPhotonEventToTask(PlayMakerFSM fsm, FsmStateAction action, string methodName, object[] parameters)
		{
			var type = action.GetType();

			var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (method != null)
			{
				if (!action.State.Active)
					return true;

				try
				{
					if(parameters == null || parameters.Length == 0)
					{
						//Debug.LogFormat("Receive photon message. {0}", methodName);
						method.Invoke(action, null);
					}
					else if(parameters.Length == 1)
					{
						//Debug.LogFormat("Receive photon message. {0} {1}", methodName, parameters);
						method.Invoke(action, parameters);
					}
					else
					{
						//Debug.LogFormat("Receive photon message. {0} {1}", methodName, parameters.ArrayToString());
						method.Invoke(action, new object[] { parameters });
					}
				}
				catch (System.Exception e)
				{
					Debug.LogErrorFormat(fsm, "Call error on photon message {0} to behaviour task {1}, {2}.", method, action.Name, type.Name);
					Debug.LogException(e);
				}
				return true;
			}
			return false;
		}
	}
}