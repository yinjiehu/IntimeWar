using HutongGames.PlayMaker;
using UnityEngine;

namespace IntimeWar.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class CreatePlayerInitialParameter : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();

			//var p = GlobalCache.GetPlayerStatus();
			//PhotonHelper.SetVehicleAndPaint(p.CurrentSelectedID, "");
			//PhotonHelper.SetInitialParameter(UnitInitialParameter.Create(p));

			Finish();
		}
	}
}