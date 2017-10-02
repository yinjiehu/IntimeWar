using UnityEngine;

namespace MechSquad
{
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

	public class KeyboardStickState : IStickState
	{
		KeyCode _upKey;
		public KeyCode Up { set { _upKey = value; } }
		KeyCode _downKey;
		public KeyCode Down { set { _downKey = value; } }
		KeyCode _leftKey;
		public KeyCode Left { set { _leftKey = value; } }
		KeyCode _rightKey;
		public KeyCode Right { set { _rightKey = value; } }

		Vector2 _normalizedDirection;
		public Vector2 NormalizedDirection { get { return _normalizedDirection; } }
		
		public KeyboardStickState()
		{
			_upKey = KeyCode.W;
			_downKey = KeyCode.S;
			_leftKey = KeyCode.A;
			_rightKey = KeyCode.D;
		}
		
		public void OnUpdate(float deltaTime)
		{
			_normalizedDirection = Vector3.zero;
			_normalizedDirection.y += Input.GetKey(KeyCode.W) ? 1 : 0;
			_normalizedDirection.y += Input.GetKey(KeyCode.S) ? -1 : 0;
			_normalizedDirection.x += Input.GetKey(KeyCode.D) ? 1 : 0;
			_normalizedDirection.x += Input.GetKey(KeyCode.A) ? -1 : 0;

			_normalizedDirection.Normalize();
		}
	}
}
