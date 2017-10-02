using MechSquad.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class DisplayLockOnTypeView : BaseView
	{
		[SerializeField]
		Text _currentState;
		[SerializeField]
		Text _currentTargetName;

		[SerializeField]
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		BattleUnit _currentTarget;
		public BattleUnit CurrentTarget { get { return _currentTarget; } }

		BattleUnit _playerUnit;
		IMainFireControlSystem _fireControl;
		private void Update()
		{
			if (_fireControl == null)
				GetPlayer();

			if (_fireControl == null)
			{
				_currentState.text = "Not initialzied";
				return;
			}

			_currentState.text = _fireControl.CurrentAimingMode.ToString();

			var target = _fireControl.GetAimingTarget();
			if(target == null)
			{
				_currentTargetName.text = "no target";
				_currentTarget = null;
			}
			else
			{
				_currentTargetName.text = target.name;
				_currentTarget = target;
			}

		}


		void GetPlayer()
		{
			_playerUnit = UnitManager.ThisClientPlayerUnit;
			if (_playerUnit != null)
				_fireControl = _playerUnit.GetAbility<IMainFireControlSystem>();
		}

	}
}
