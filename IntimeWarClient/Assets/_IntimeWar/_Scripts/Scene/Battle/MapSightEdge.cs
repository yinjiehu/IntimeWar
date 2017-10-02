using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class MapSightEdge : MonoBehaviour
	{
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

		[SerializeField]
		bool _showGizmo;
		[SerializeField]
		Color _gizmoColor;
		[SerializeField]
		float _zLen = 600;
		[SerializeField]
		float _xLen = 800;
				
		private void OnDrawGizmos()
		{
			if (!_showGizmo)
				return;
			
			Gizmos.color = _gizmoColor;
			if(_leftEdge != null)
			{
				Gizmos.DrawCube(_leftEdge.position, new Vector3(5, 80, _zLen));
			}
			if(_rightEdge != null)
			{
				Gizmos.DrawCube(_rightEdge.position, new Vector3(5, 80, _zLen));
			}
			if(_forwardEdge != null)
			{
				Gizmos.DrawCube(_forwardEdge.position, new Vector3(_xLen, 80, 5));
			}
			if (_backwardEdge != null)
			{
				Gizmos.DrawCube(_backwardEdge.position, new Vector3(_xLen, 80, 5));
			}
		}
	}
}