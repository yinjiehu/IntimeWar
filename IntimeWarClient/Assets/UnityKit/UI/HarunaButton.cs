using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Haruna.UI
{
	public class HarunaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
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
		
		protected Vector2 _pressScreenPosition;
		protected Vector2 _currentScreenPosition;

		public float DragDistance { get { return Vector2.Distance(_pressScreenPosition, _currentScreenPosition); } }
		public Vector2 DragDirectionInScreen { get { return _currentScreenPosition - _pressScreenPosition; } }
		
		public bool Clicked { get { return !_currentDown && _previousDown && _elapsedSecondsSinceDown <= CLICK_JUDGE_SECONDS; } }
		public bool Holding { get { return _currentDown && _elapsedSecondsSinceDown > CLICK_JUDGE_SECONDS; } }

		public virtual void OnPointerDown(PointerEventData eventData)
		{
			_currentDown = true;
			_elapsedSecondsSinceDown = 0;
			
			_currentScreenPosition = _pressScreenPosition = eventData.pressPosition;
		}

		public virtual void OnPointerUp(PointerEventData eventData)
		{
			_currentDown = false;
			_currentScreenPosition = eventData.position;
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			_currentScreenPosition = eventData.position;
		}

		protected virtual void Update()
		{
			if (!_previousDown && !_currentDown)
				_state = StateEnum.None;
			else if (!_previousDown && _currentDown)
				_state = StateEnum.Press;
			else if (_previousDown && _currentDown)
				_state = StateEnum.Down;
			else if (_previousDown && !_currentDown)
				_state = StateEnum.Release;

			if (_currentDown)
				_elapsedSecondsSinceDown += Time.deltaTime;

			_previousDown = _currentDown;
		}
	}
}