using System;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class DebugView : BaseView
	{
		[SerializeField]
		Haruna.UI.HarunaButton _detector;

		[SerializeField]
		Haruna.UI.HarunaButton _viewContent;


		[SerializeField]
		PlayMakerFSM _fsm;

		private void Update()
		{
			if(_detector.CurrentDown && _detector.DragDirectionInScreen.x > 10)
			{
				_fsm.SendEvent("SlideIn");
			}
			else if(_viewContent.CurrentDown && _viewContent.DragDirectionInScreen.x < -10)
			{
				_fsm.SendEvent("SlideOut");
			}
		}


		public void CreateDamage(float damage)
		{
			var player = Battle.UnitManager.ThisClientPlayerUnit;
			player.EventDispatcher.TriggerEvent(new Battle.DamageEvent()
			{
				Damage = damage,
				Attacker = new Battle.UnitInfo()
				{
					TypeID = "TestDamageBullet",
					Team = Battle.Team.B,
				}
			});
		}

		[SerializeField]
		Light _light;

		public void SwitchShadow()
		{
			if (_light.shadows == LightShadows.None)
				_light.shadows = LightShadows.Hard;
			else
				_light.shadows = LightShadows.None;
		}

		public void SwitchPhotonStatsGUI()
		{
			var pgui = FindObjectOfType<PhotonStatsGui>();
			pgui.statsWindowOn = !pgui.statsWindowOn;
		}
	}
}