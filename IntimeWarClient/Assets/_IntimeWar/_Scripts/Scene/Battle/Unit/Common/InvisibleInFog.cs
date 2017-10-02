using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;


namespace MechSquad.Battle
{
	public class InvisibleInFog : Ability
	{
		[SerializeField]
		float _minFogStrength;

		[SerializeField]
		float _invisibleDelaySecondsAfterInFog = 3.5f;
		float _elapsedTimeSinceInvisible;
		
		bool _currentIsVisibleInEnemyRaidar;

		DataMixerBool.Control _visibleControl;
		public bool InvisibleInEnemyRaidar { get { return !_visibleControl.Value; } }
		
		public override void Init()
		{
			base.Init();

			if (_unit.Team != PhotonNetwork.player.GetUnitTeam())
			{
				_visibleControl = _unit.STS.BodyVisible.CreateControl(false);
				_elapsedTimeSinceInvisible = _invisibleDelaySecondsAfterInFog;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_visibleControl != null)
			{
				//var inFog = FogOfWar.current.IsInFog(_unit.Model.position, _minFogStrength);
				//if (inFog)
				//{
				//	if (_currentIsVisibleInEnemyRaidar)
				//	{
				//		_elapsedTimeSinceInvisible += Time.deltaTime;
				//		if (_elapsedTimeSinceInvisible > _invisibleDelaySecondsAfterInFog)
				//		{
				//			_visibleControl.Value = _currentIsVisibleInEnemyRaidar = false;
				//		}
				//	}
				//}
				//else
				//{
				//	_visibleControl.Value = _currentIsVisibleInEnemyRaidar = true;
				//	_elapsedTimeSinceInvisible = 0;
				//}
			}
		}
	}
}