using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad.Battle
{
	public class InSceneEnvSpawner : MonoBehaviour, ISpawner
	{
		public string Name { get { return name; } }

		[SerializeField]
		GameObject _rootObj;

		[SerializeField]
		bool _activateAtStart;
		[SerializeField]
		bool _includeInactive;
		bool _activated;

		List<PhotonView> _photonViews;

		private void Start()
		{
			_photonViews = _rootObj.GetComponentsInChildren<PhotonView>(_includeInactive).ToList();
			if (_activateAtStart)
				SpawnUnit();
		}

		public void SpawnUnit()
		{
			if (_activated)
			{
				Debug.LogErrorFormat(this, "In scene env spawner is already activated.");
				return;
			}

			_activated = true;

			var units = _rootObj.GetComponentsInChildren<BattleUnit>(_includeInactive);
			for (var i = 0; i < units.Length; i++)
			{
				var u = units[i];
				u.Init(new BattleUnit.UnitCreateArgs()
				{
					Team = Team.None,
				});
			}
		}

		//[PunRPC]
		//void OnPhotonInstantiate(int[] viewIds)
		//{
		//	var view = _photonViews.Find(v => v.viewID == viewIds[0]);
		//	if (view == null)
		//	{
		//		Debug.LogErrorFormat("can not find view id {0}", viewIds[0]);
		//		return;
		//	}

		//	view.GetComponent<BattleUnit>().Init(new BattleUnit.UnitCreateArgs()
		//	{
		//		Team = Team.None,
		//	});
		//}
	}
}