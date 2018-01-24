using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace IntimeWar.Battle
{
	public class MapEdge : MonoBehaviour
	{
		static MapEdge _instance;
		public static MapEdge Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				_instance = FindObjectOfType<MapEdge>();
				if (_instance != null)
					return _instance;

				throw new Exception("No Map Edge!");
			}
		}

		[SerializeField]
		Transform _leftEdge;
		public Transform LeftEdge { get { return _leftEdge; } }
		[SerializeField]
		Transform _rightEdge;
		public Transform RightEdge { get { return _rightEdge; } }
		[SerializeField]
		Transform _forwardEdge;
		public Transform ForwardEdge { get { return _forwardEdge; } }
		[SerializeField]
		Transform _backwardEdge;
		public Transform BackwardEdge { get { return _backwardEdge; } }

		public static bool IsInMap(Vector3 position)
		{
			return position.x > Instance._leftEdge.position.x
				&& position.x < Instance._rightEdge.position.x
				&& position.z < Instance._forwardEdge.position.z
				&& position.z > Instance._backwardEdge.position.z;
		}

		public static Vector3 GetCorrectedPosition(Vector3 position)
		{
			if (position.x < Instance._leftEdge.position.x)
				position.x = Instance._leftEdge.position.x + 5;
			else if (position.x > Instance._rightEdge.position.x)
				position.x = Instance._rightEdge.position.x - 5;

			if (position.z > Instance._forwardEdge.position.z)
				position.z = Instance._forwardEdge.position.z - 5;
			else if (position.z < Instance._backwardEdge.position.z)
				position.z = Instance._backwardEdge.position.z + 5;

			return position;
		}

	}
}