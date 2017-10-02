using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;

namespace MechSquad.Battle
{
	public class SquadSight : Ability
	{
		[Range(0.0f, 1.0f)]
		public float minFogStrength = 0.2f;
		
		InvisibleInFog _invisibleInFog;

		bool _isAllyToThisClientPlayer;
		bool _isInFog;
				
		public override void Init()
		{
			base.Init();

			_invisibleInFog = _unit.GetAbility<InvisibleInFog>();

			
		}

		private void OnValueChange(bool obj)
		{
		}
	}
}