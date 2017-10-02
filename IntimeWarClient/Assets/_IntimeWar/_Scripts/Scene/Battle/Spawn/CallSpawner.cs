using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;

namespace  MechSquad.Battle
{
	[ActionCategory("MechSquad_Unit")]
	public class CallSpawner : FsmStateAction
	{
		[SerializeField]
		public string _spawnerName;

		public override void OnEnter()
		{
			base.OnEnter();

			var sp = SpawnerManager.Instance.Get(_spawnerName);
			sp.SpawnUnit();

			Finish();
		}
	}
}