using MechSquad;
using System;

namespace IntimeWar
{
    public static class PhotonCustomEventSender
    {
        public static void RaisePhotonCustomEvent(string evName, bool reliable = true)
        {
            PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.CustomEvent,
                new object[] { evName }, reliable, RaiseEventOptions.Default);
        }
        public static void RaisePhotonCustomEvent(string evName, object data, bool reliable = true)
        {
            PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.CustomEvent,
                new object[] { evName, data }, reliable, RaiseEventOptions.Default);
        }

        public static void RaisePhotonCustomEvent<T>(T data, bool reliable = true) where T : PhotonCustomEvent
        {
            var evName = typeof(T).FullName;
            PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.CustomEvent,
                 new object[] { evName, JsonUtil.Serialize(data) }, reliable, RaiseEventOptions.Default);
        }

        public static void RaiseResetBattleEvent()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonClientEvCodeEnum.ResetBattle, "", true, RaiseEventOptions.Default);
        }
    }
}