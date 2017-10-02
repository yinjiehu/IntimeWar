using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class UnitRaidarDisplay : Ability
	{
		[SerializeField]
		RaidarElement _prefab;

		RaidarElement _instance;

		UnitStateEnum _previousUnitState;

		public override void LateInit()
		{
			base.LateInit();

			if (!_unit.IsPlayerForThisClient)
			{
				_instance = ViewManager.Instance.GetView<RaidarView>().CreateFromPrefab(_unit, _prefab);
				_instance.Unit = _unit;

				if (_unit.Team == PhotonNetwork.player.GetUnitTeam())
				{
					_instance.SetAsAlly();
					_instance.Hide();
				}
				else
				{
					_instance.SetAsEnemy();
					_instance.Hide();
				}

				_unit.STS.DisplayInRaidar.EvOnValueChange += OnDisplayOnRaidarChanged;
			}
		}

		private void OnDisplayOnRaidarChanged(bool value)
		{
			_instance.gameObject.SetActive(value);
		}

		public override void BeforeDestroy()
		{
			if (_instance != null)
			{
				ViewManager.Instance.GetView<RaidarView>().RemoveRaidarElement(_unit.SeqNo);
			}
		}
	}
}