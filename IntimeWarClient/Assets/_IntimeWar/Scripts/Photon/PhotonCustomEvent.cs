using System;
using System.Collections.Generic;
using System.Linq;

namespace IntimeWar
{
    public enum PhotonClientEvCodeEnum : byte
    {
        CustomEvent = 100,
        SpawnUnit = 130,
        DestroyUnit = 135,
        ResetBattle = 150
    }

    public enum PhotonServerEvCodeEnum : byte
    {
        CustomEvent = 100,
        PlayerDisconnected = 110
    }

    public abstract class PhotonCustomEvent
	{
	}
}
