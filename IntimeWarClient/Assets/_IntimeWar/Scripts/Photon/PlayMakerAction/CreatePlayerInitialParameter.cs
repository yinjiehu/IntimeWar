using Demo;
using HutongGames.PlayMaker;
using UnityEngine;
using YJH.Unit;

namespace IntimeWar.RealTime
{
	[ActionCategory("MechSquad_Photon")]
	public class CreatePlayerInitialParameter : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();

            var p = GlobalCache.GetPlayerStatus();
            //PhotonHelper.SetVehicleAndPaint(p.CurrentSelectedID, "");
            //PhotonHelper.SetInitialParameter(UnitInitialParameter.Create(p));
            DemoPlayerSettings.InitSettings();
            DemoSkillSettings.InitSettings();
            //PhotonNetwork.player.UserId = Save.Account.Uid;
            //PhotonNetwork.player.NickName = Save.Player.Basic.NickName;
            p.PlayerClassify = "Player_1_1";
            GlobalCache.SetPlayerStatus(p);
            PhotonHelper.SetUnitTeam(1);
            PhotonHelper.SetSeqInTeam(0);
            PhotonHelper.SetClassify("Player_1_1");
            PhotonHelper.SetInitialParameter(UnitInitialParameter.Create(p));
            Finish();
		}
	}
}