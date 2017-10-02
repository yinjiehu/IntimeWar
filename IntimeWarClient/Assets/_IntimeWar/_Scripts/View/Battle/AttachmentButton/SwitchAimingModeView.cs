using MechSquad.Battle;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MechSquad.View
{
	public class SwitchAimingModeView : BaseView, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		bool _clicked;
		public bool Clicked { get { return _clicked; } }

		bool _previousDown;
		bool _currentDown;

		public void OnPointerDown(PointerEventData eventData)
		{
			_currentDown = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_currentDown = false;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_currentDown = false;
		}

		void UpdateButtonState()
		{
			_clicked = !_previousDown && _currentDown;
			_previousDown = _currentDown;
		}

		[SerializeField]
		GameObject _autoMode;
		[SerializeField]
		GameObject _manualMode;

		IMainFireControlSystem _mainFireControl;

		void UpdateAimingModeDisplay()
		{
			if(_mainFireControl == null)
			{
				if (UnitManager.ThisClientPlayerUnit != null)
				{
					_mainFireControl = UnitManager.ThisClientPlayerUnit.GetAbility<IMainFireControlSystem>();
				}
			}

			if(_mainFireControl != null)
			{
				_autoMode.SetActive(_mainFireControl.CurrentAimingMode == AimingModeEnum.Auto);
				_manualMode.SetActive(_mainFireControl.CurrentAimingMode != AimingModeEnum.Auto);
			}
		}

		
		void Update()
		{
			UpdateButtonState();
			UpdateAimingModeDisplay();
		}
	}
}