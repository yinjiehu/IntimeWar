using System;
using System.Collections.Generic;
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

		KeyCode Keycode { get; }

		void OnUpdate(float deltaTime);
	}

	[Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
	public class BindedKeyCollection
	{
		[Newtonsoft.Json.JsonProperty]
		IKeyState[] _bindedKeys = new IKeyState[4];

		public void ClearAll()
		{
			for (var i = 0; i < _bindedKeys.Length; i++)
			{
				RemoveKey(i);
			}
		}
		public void RemoveKey(int index)
		{
			_bindedKeys[index] = null;
		}

		public bool IsKeyBinded(int index)
		{
			return _bindedKeys[index] != null;
		}

		public KeyCode GetKeyCode(int index)
		{
			if (_bindedKeys[index] == null)
				return KeyCode.None;
			return _bindedKeys[index].Keycode;
		}
		public List<KeyCode> GetAllKeyCode()
		{
			var list = new List<KeyCode>();
			for (var i = 0; i < _bindedKeys.Length; i++)
			{
				if (_bindedKeys[i] != null && _bindedKeys[i].Keycode != KeyCode.None)
					list.Add(_bindedKeys[i].Keycode);
			}
			return list;
		}

		public void BindKey(KeyCode key, int index)
		{
			_bindedKeys[index] = new KeyState(key);
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

	[Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
	public class KeyState : IKeyState
	{
		public const float CLICK_JUDGE_SECONDS = 0.35f;

		[Newtonsoft.Json.JsonProperty]
		KeyCode _keyCode;
		public KeyCode Keycode { get { return _keyCode; } }
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
