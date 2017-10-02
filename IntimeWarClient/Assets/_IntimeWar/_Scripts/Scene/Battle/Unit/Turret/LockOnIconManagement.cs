using System;
using Haruna.Inspector;
using UnityEngine;
using WhiteCat.Tween;

namespace MechSquad.Battle
{
	[Serializable]
	public class LockOnIconManagement : MonoBehaviour
	{
		BattleUnit _playerUnit;
		IMainFireControlSystem _fireControlSystem;

		[SerializeField]
		LockOnIconOnUnit _prefab;

		LockOnIconOnUnit _instance;

		BattleUnit _previousTarget;
		BattleUnit _previousTargetSeqNo;

		private void Start()
		{
			_instance = Instantiate(_prefab);
			_instance.name = _prefab.name;
			_instance.Hide();
		}

		private void Update()
		{
			GetPlayerUnit();

			if (_fireControlSystem == null)
			{
				if (_instance != null && _instance.IsVisible)
				{
					_instance.Hide();
				}
				return;
			}

			var currentTarget = _fireControlSystem.GetAimingTarget();
			if (_previousTarget == null && _instance.IsVisible)
			{
				_instance.Hide();
			}

			if (currentTarget != _previousTarget && currentTarget != null)
			{
				_instance.Show(currentTarget.Model);
			}

			_previousTarget = currentTarget;
		}


		void GetPlayerUnit()
		{
			if (_playerUnit == null)
			{
				_playerUnit = UnitManager.ThisClientPlayerUnit;

				if (_playerUnit != null)
				{
					_fireControlSystem = _playerUnit.GetAbility<IMainFireControlSystem>();
				}
			}
		}
	}
}