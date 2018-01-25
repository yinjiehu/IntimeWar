using UnityEngine;
using UnityEngine.EventSystems;

namespace Haruna.UI
{
	public class HarunaJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public const float CLICK_JUDGE_SECONDS = 0.15f;

        public enum StateEnum
        {
            None,
            Press,
            Down,
            Release,
        }
        protected StateEnum _state = StateEnum.None;
        public StateEnum CurrentState { get { return _state; } }

        protected bool _previousDown;
        public bool PreviousDown { get { return _previousDown; } }
        protected bool _currentDown;
        public bool CurrentDown { get { return _currentDown; } }
        protected float _elapsedSecondsSinceDown;
        public float ElapsedSecondsSinceDown { get { return _elapsedSecondsSinceDown; } }
        public bool Clicked { get { return !_currentDown && _previousDown && _elapsedSecondsSinceDown <= CLICK_JUDGE_SECONDS; } }
        public bool Holding { get { return _currentDown && _elapsedSecondsSinceDown > CLICK_JUDGE_SECONDS; } }
        protected Vector2 _pressScreenPosition;
        protected Vector2 _currentScreenPosition;

        Vector3 _defaultPadPosition;
		Vector3 _defaultStickPosition;

		[SerializeField]
		Transform _pad;
		[SerializeField]
		Transform _stick;
		[SerializeField]
		bool _activePad;
		[SerializeField]
		bool _activeStick;

		Camera _uiCamera;

		Vector3 _directionToActivePadInScreen;
		Vector3 _directionToActivePadInMainCamera;
		public Vector3 DirectionToActivePadInMainCamera { get { return _directionToActivePadInMainCamera; } }

		Vector3 _directionToCenterInScreen;
		Vector3 _directionToCenterInMainCamera;
		public Vector3 DirectionToCenterInMainCamera { get { return _directionToCenterInMainCamera; } }

		float _elapsedSecondsSinceRelease;
		[SerializeField]
		float _activePadMemorySeconds;

		Vector3 _pressWorldPosition;

        [SerializeField]
		float _maxRadius = 20f;
		public float NormalizedDragDistance { set; get; }

		private void Start()
		{
			_uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
			_elapsedSecondsSinceDown = _activePadMemorySeconds;

			_defaultPadPosition = _pad.transform.position;
			_defaultStickPosition = _stick.transform.position;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_currentDown = true;
			_elapsedSecondsSinceDown = 0;

			if (_elapsedSecondsSinceRelease > _activePadMemorySeconds)
				_currentScreenPosition = _pressScreenPosition = eventData.pressPosition;
			else
				_currentScreenPosition = eventData.pressPosition;

			_pressWorldPosition = _uiCamera.ScreenToWorldPoint(_pressScreenPosition);

			if (_activePad)
			{
				_pad.transform.position = _pressWorldPosition;
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_elapsedSecondsSinceRelease = 0;
            _currentScreenPosition = eventData.position;
            _currentDown = false;

            _pad.transform.position = _defaultPadPosition;
			_stick.transform.position = _defaultStickPosition;
		}

        public void OnDrag(PointerEventData eventData)
        {
            _currentScreenPosition = eventData.position;
        }

        protected void Update()
		{
			PressPositionMemoryCheck();
			DirectionCaluate();
			UpdateDragNormalizedDistance();
		}

		void PressPositionMemoryCheck()
		{
			if (CurrentState == StateEnum.None)
			{
				_elapsedSecondsSinceRelease += Time.deltaTime;
			}
		}

		void DirectionCaluate()
		{
			if (!_currentDown)
			{
				return;
			}

			var currentScreenPositon = new Vector3(_currentScreenPosition.x, _currentScreenPosition.y, 0);
			var currentWorldPosition = _uiCamera.ScreenToWorldPoint(currentScreenPositon);

			_directionToCenterInScreen = currentWorldPosition - _defaultPadPosition;
			_directionToCenterInMainCamera = Camera.main.transform.TransformDirection(_directionToCenterInScreen);
			_directionToCenterInMainCamera.y = 0;

			_directionToActivePadInScreen = currentWorldPosition - _pressWorldPosition;
			_directionToActivePadInMainCamera = Camera.main.transform.TransformDirection(_directionToActivePadInScreen);
			_directionToActivePadInMainCamera.z = 0;
			
			var padScreenPosition = _uiCamera.WorldToScreenPoint(_pad.transform.position);
			padScreenPosition.z = 0;
			float distance = Vector3.Distance(_currentScreenPosition, padScreenPosition);

			var radiusInScreenPoint = Screen.width / 1920f * _maxRadius;

			if (distance > radiusInScreenPoint)
			{
				if (_activeStick)
				{
					var stickScreenPosition = padScreenPosition + (currentScreenPositon - padScreenPosition).normalized * radiusInScreenPoint;
					_stick.transform.position = _uiCamera.ScreenToWorldPoint(stickScreenPosition);
				}
			}
			else
			{
				if (_activeStick)
				{
					_stick.transform.position = currentWorldPosition;
				}
			}

		}

		void UpdateDragNormalizedDistance()
		{
			if (_currentDown)
			{
				var distance = Vector2.Distance(_currentScreenPosition, _pressScreenPosition);
				var radiusInScreenPoint = Screen.width / 1920f * _maxRadius;
				NormalizedDragDistance = distance / radiusInScreenPoint;
				if (NormalizedDragDistance > 1)
					NormalizedDragDistance = 1;
			}
			else
			{
				NormalizedDragDistance = 0;
			}
		}
    }
}