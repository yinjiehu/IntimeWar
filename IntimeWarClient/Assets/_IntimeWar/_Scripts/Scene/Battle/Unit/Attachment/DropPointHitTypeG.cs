using UnityEngine;

namespace MechSquad.Battle
{
	public class DropPointHitTypeG : MonoBehaviour
	{
		[SerializeField]
		float _minRange;
		public float MinRange { set { _minRange = value; } get { return _minRange; } }
		[SerializeField]
		float _maxRange;
		public float MaxRange { set { _maxRange = value; } get { return _maxRange; } }

		IMainFireControlSystem _mainFireControl;

		BattleUnit _unit;
		public BattleUnit Unit
		{
			set
			{
				_unit = value;
				_mainFireControl = _unit.GetAbility<IMainFireControlSystem>();
			}
			get { return _unit; }
		}

		[SerializeField]
		float _moveSpeed;

		float _movedDistance;

		enum StateEnum
		{
			Forwarding,
			OnMaxRange
		}
		StateEnum _state;

		float _elapsedTimeOnMaxRange;
		[SerializeField]
		float _staySecondsOnMaxRange = 3;

		public void SetMoving()
		{
			gameObject.SetActive(true);
			ResetToInitialPosition();
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public bool IsInMinRange()
		{
			return _movedDistance < MinRange;
		}

		private void Update()
		{
			if (Unit != null)
			{
				if (_state == StateEnum.Forwarding)
				{
					_movedDistance += _moveSpeed * Time.deltaTime;
					if (_movedDistance > MaxRange)
					{
						_movedDistance = MaxRange;
						_state = StateEnum.OnMaxRange;
						_elapsedTimeOnMaxRange = 0;
					}

					transform.position = Unit.Model.position + _mainFireControl.TurretRoot.forward * _movedDistance;
				}
				else if (_state == StateEnum.OnMaxRange)
				{
					transform.position = Unit.Model.position + _mainFireControl.TurretRoot.forward * MaxRange;

					_elapsedTimeOnMaxRange += Time.deltaTime;
					if (_elapsedTimeOnMaxRange > _staySecondsOnMaxRange)
					{
						ResetToInitialPosition();
					}
				}
			}
		}

		void ResetToInitialPosition()
		{
			transform.position = Unit.Model.position + _mainFireControl.TurretRoot.forward * MinRange;

			_movedDistance = MinRange;
			_elapsedTimeOnMaxRange = 0;

			_state = StateEnum.Forwarding;
		}
	}
}