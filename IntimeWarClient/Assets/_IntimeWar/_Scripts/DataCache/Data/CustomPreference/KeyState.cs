using UnityEngine;

namespace MechSquad
{
	public interface IKeyState
	{
		bool PreviousDown { get; }
		bool CurrentDown { get; }

		bool Press { get; }
		bool Release { get; }
		bool Holding { get; }
		bool Clicked { get; }

		void OnUpdate(float deltaTime);
	}

	public class BindedKeyCollection
	{
		IKeyState[] _bindedKeys = new IKeyState[4];

		public void BindKey(KeyCode key, int index)
		{
			_bindedKeys[index] = new KeyState(key);
		}
		public void RemoveKey(int index)
		{
			_bindedKeys[index] = null;
		}

		public bool CurrentDown
		{
			get
			{
				for (var i = 0; i < _bindedKeys.Length; i++)
				{
					if (_bindedKeys[i] != null && _bindedKeys[i].CurrentDown)
						return true;
				}
				return false;
			}
		}

		public bool Press
		{
			get
			{
				for (var i = 0; i < _bindedKeys.Length; i++)
				{
					if (_bindedKeys[i] != null && _bindedKeys[i].Press)
						return true;
				}
				return false;
			}
		}
		public bool Release
		{
			get
			{
				for (var i = 0; i < _bindedKeys.Length; i++)
				{
					if (_bindedKeys[i] != null && _bindedKeys[i].Release)
						return true;
				}
				return false;
			}
		}
		public bool Holding
		{
			get
			{
				for (var i = 0; i < _bindedKeys.Length; i++)
				{
					if (_bindedKeys[i] != null && _bindedKeys[i].Holding)
						return true;
				}
				return false;
			}
		}
		public bool Clicked
		{
			get
			{
				for (var i = 0; i < _bindedKeys.Length; i++)
				{
					if (_bindedKeys[i] != null && _bindedKeys[i].Clicked)
						return true;
				}
				return false;
			}
		}

		public void OnUpdate(float deltaTime)
		{
			for (var i = 0; i < _bindedKeys.Length; i++)
			{
				if (_bindedKeys[i] != null)
					_bindedKeys[i].OnUpdate(deltaTime);
			}
		}
	}

	public class KeyState : IKeyState
	{
		public const float CLICK_JUDGE_SECONDS = 0.2f;

		KeyCode _keyCode;

		protected bool _previousDown;
		public bool PreviousDown { get { return _previousDown; } }
		protected bool _currentDown;
		public bool CurrentDown { get { return _currentDown; } }

		public bool Press { get { return _currentDown && !_previousDown; } }
		public bool Release { get { return !_currentDown && _previousDown; } }
		public bool Holding { get { return _currentDown && _elapsedSecondsSinceDown > CLICK_JUDGE_SECONDS; } }
		public bool Clicked { get { return !_currentDown && _previousDown && _elapsedSecondsSinceDown <= CLICK_JUDGE_SECONDS; } }

		float _elapsedSecondsSinceDown = 0;

		public KeyState(KeyCode keyCode)
		{
			_keyCode = keyCode;
		}

		public void OnUpdate(float deltaTime)
		{
			if (_currentDown)
				_elapsedSecondsSinceDown += Time.deltaTime;


			_previousDown = _currentDown;

			_currentDown = Input.GetKey(_keyCode);
			if (!_previousDown && _currentDown)
				_elapsedSecondsSinceDown = 0;
		}
	}
}
