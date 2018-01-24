using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;

namespace  IntimeWar.Battle
{
	[ActionCategory("MechSquad_Unit")]
	public class EnableSpawnSynchronization : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();

			SpawnerManager.Instance.EnableSpawnSynchronization();
			Finish();
		}
	}
}