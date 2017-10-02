using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;

namespace MechSquad.Battle
{
	public class UnitHpDisplay : Ability
	{
		[SerializeField]
		HudInstance _hudPrefab;
		HudHp _hudHpInstance;
		
		float _elapsedTime;
		float _updateInterval = 0.1f;

		public override void LateInit()
		{
			base.LateInit();

			var hudInstance = ViewManager.Instance.GetView<HudView>().CreateFromPrefab(_hudPrefab);
			_hudHpInstance = hudInstance.GetComponent<HudHp>();
			_hudHpInstance.SetUnitAndStartFollow(_unit, _unit.GetAbility<UnitPositionDefine>().Top);
			_hudHpInstance.SetVisible(false);

			_unit.STS.BodyVisible.EvOnValueChange += OnBodyVisibleChange;
		}

		private void OnBodyVisibleChange(bool visible)
		{
			_hudHpInstance.SetVisible(visible);
		}
				
		public override void BeforeDestroy()
		{
			if (_hudHpInstance != null)
			{
				_hudHpInstance.SetVisible(false);
				_hudHpInstance.GetComponent<HudInstance>().RecycleInstance();
			}
		}
	}
}