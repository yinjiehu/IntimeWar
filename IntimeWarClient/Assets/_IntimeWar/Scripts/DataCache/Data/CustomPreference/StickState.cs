using UnityEngine;

namespace MechSquad
{
	//public class BindedStickStateCollection
	//{
	//	IStickState[] _bindedStick = new IStickState[4];

	//	public Vector2 NormalizedDirection
	//	{
	//		get
	//		{
	//			Vector2 direction = Vector2.zero;
	//			for (var i = 0; i < _bindedStick.Length; i++)
	//			{
	//				direction += _bindedStick[i].NormalizedDirection;
	//			}
	//			return direction.normalized;
	//		}
	//	}

	//	public void ClearAll()
	//	{
	//		for (var i = 0; i < _bindedStick.Length; i++)
	//		{
	//			RemoveKey(i);
	//		}
	//	}
	//	public void RemoveKey(int index)
	//	{
	//		_bindedStick[index] = null;
	//	}

	//	public bool IsKeyBinded(int index)
	//	{
	//		return _bindedStick[index] != null;
	//	}

	//	public IStickState GetStick(int index)
	//	{
	//		return _bindedStick[index];
	//	}
	//	public void BindKey(IStickState stick, int index)
	//	{
	//		_bindedStick[index] = stick;
	//	}

	//	public void OnUpdate(float deltaTime)
	//	{
	//		for (var i = 0; i < _bindedStick.Length; i++)
	//		{
	//			_bindedStick[i].OnUpdate(deltaTime);
	//		}
	//	}
	//}

	public interface IStickState
	{
		Vector2 NormalizedDirection { get; }
		void OnUpdate(float deltaTime);
	}

	public class LeftStickState : IStickState
	{
		Vector2 _normalizedDirection;
		public Vector2 NormalizedDirection { get { return _normalizedDirection; } }

		public void OnUpdate(float deltaTime)
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			_normalizedDirection = new Vector2(horizontal, vertical).normalized;
		}
	}

	public class RightStickState : IStickState
	{
		Vector2 _normalizedDirection;
		public Vector2 NormalizedDirection { get { return _normalizedDirection; } }

		public void OnUpdate(float deltaTime)
		{
			var horizontal = Input.GetAxis("RightHorizontal");
			var vertical = Input.GetAxis("RightVertical");

			_normalizedDirection = new Vector2(horizontal, vertical).normalized;
		}
	}

	//public class KeyboardStickState : IStickState
	//{
	//	KeyCode _upKey;
	//	public KeyCode Up { set { _upKey = value; } get { return _upKey; } }
	//	KeyCode _downKey;
	//	public KeyCode Down { set { _downKey = value; } get { return _downKey; } }
	//	KeyCode _leftKey;
	//	public KeyCode Left { set { _leftKey = value; } get { return _leftKey; } }
	//	KeyCode _rightKey;
	//	public KeyCode Right { set { _rightKey = value; } get { return _rightKey; } }

	//	Vector2 _normalizedDirection;
	//	public Vector2 NormalizedDirection { get { return _normalizedDirection; } }

	//	public KeyboardStickState()
	//	{
	//		_upKey = KeyCode.W;
	//		_downKey = KeyCode.S;
	//		_leftKey = KeyCode.A;
	//		_rightKey = KeyCode.D;
	//	}

	//	public void OnUpdate(float deltaTime)
	//	{
	//		_normalizedDirection = Vector3.zero;
	//		_normalizedDirection.y += Input.GetKey(_upKey) ? 1 : 0;
	//		_normalizedDirection.y += Input.GetKey(_downKey) ? -1 : 0;
	//		_normalizedDirection.x += Input.GetKey(_rightKey) ? 1 : 0;
	//		_normalizedDirection.x += Input.GetKey(_leftKey) ? -1 : 0;

	//		_normalizedDirection.Normalize();
	//	}
	//}
}
