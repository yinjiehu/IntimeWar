using System;

namespace MechSquad
{
	public static class PhotonCustomEventSender
	{
		public static void RaisePhotonCustomEvent<T>(T data, bool reliable = true) where T : PhotonCustomEvent
		{
			var evName = typeof(T).FullName;
			PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.CustomEvent,
				 new object[] { evName, JsonUtil.Serialize(data) }, reliable, RaiseEventOptions.Default);
		}
		public static void RaisePhotonCustomEvent(string evName, string data, bool reliable = true)
		{
			PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.CustomEvent,
				new object[] { evName, data }, reliable, RaiseEventOptions.Default);
		}

		public static void RaiseInstantiateUnitEvent(PhotonView view, string methodName, int[] viewIds, params object[] args)
		{
#if IRONFURY_DEBUG
			//Debug.LogFormat(view, "Raise instantiate unit event. Creater {0}.{1}  views {2}", view.viewID, methodName, viewIds.ArrayToString());
#endif
			var data = new ExitGames.Client.Photon.Hashtable();
			
			data.Add((byte)InstantiateUnitEventKey.CreaterViewID, view.viewID);
			data.Add((byte)InstantiateUnitEventKey.CreaterMethodName, methodName);

			var parameters = new object[args.Length + 1];
			parameters[0] = viewIds;
			Array.Copy(args, 0, parameters, 1, args.Length);
			data.Add((byte)InstantiateUnitEventKey.Parameters, parameters);
			data.Add((byte)InstantiateUnitEventKey.EventTimeStamp, DateTime.UtcNow.Ticks);

			//var ev = new EventData();
			//ev.Code = PunEvent.RPC;
			//ev.Parameters = new Dictionary<byte, object>();
			//ev.Parameters.Add(ParameterCode.ActorNr, view.ownerId);
			//ev.Parameters.Add(ParameterCode.Data, data);
			//PhotonNetwork.networkingPeer.OnEvent(ev);
			if (!PhotonNetwork.offlineMode)
				PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.InstantiateUnit, data, true, RaiseEventOptions.Default);

			PhotonCustomEventReceiver.Instance.OnInstantiateUnit(data);
		}

		public static void RaiseDestroyUnitEvent(int rootViewID)
		{
			PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.DestroyUnit, rootViewID, true, RaiseEventOptions.Default);
		}

		public static void RaiseResendInstantiateEvent()
		{
			PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.ResendInstantiateEvent, "", true, RaiseEventOptions.Default);
		}

		public static void RaiseResetBattleEvent()
		{
			PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.ResetBattle, "", true, RaiseEventOptions.Default);
		}
	}
}