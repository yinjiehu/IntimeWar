using System;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class HudInstance : MonoBehaviour
	{
		public HudView View { set; get; }

		HudInstance _prefab;
		public HudInstance Prefab { set { _prefab = value; } get { return _prefab; } }
		public int PrefabInstanceID { get { return _prefab.GetInstanceID(); } }

		Camera _uiCamera;
		public Camera UICamera { set { _uiCamera = value; } get { return _uiCamera; } }

		Camera _worldCamera;
		public Camera WorldCamera { set { _worldCamera = value; } get { return _worldCamera; } }

		[SerializeField]
		bool _shouldFollowTransform = true;
		Transform _followTransform;
		public Transform FollowTransform
		{
			set
			{
				_followTransform = value;
				_previousTransformPosition = value.position;
			}
			get
			{
				return _followTransform;
			}
		}

		[SerializeField]
		Vector3 _offset;
		public Vector3 Offset { set { _offset = value; } get { return _offset; } }

		Vector3 _previousTransformPosition;
		Vector3 _previousOffset;
		Vector3 _previousWorldCameraPosition;
		Quaternion _previousWorldCameraRotation;

		public void RecycleInstance()
		{
			if (View != null)
				View.RecycleInstance(this);
		}

		private void Update()
		{
			if (_shouldFollowTransform)
			{
				if (_followTransform == null)
					return;
				if (_previousTransformPosition == _followTransform.position
					&& _previousOffset == _offset
					&& _previousWorldCameraPosition == _worldCamera.transform.position
					&& _previousWorldCameraRotation == _worldCamera.transform.rotation)
					return;

				SynchronizePosition();
			}
		}

		public void SynchronizePosition()
		{
			if (_followTransform == null)
				return;

			var worldPosition = _followTransform.position;
			var screenPoint = WorldCamera.WorldToScreenPoint(worldPosition);
			var uiPosition = _uiCamera.ScreenToWorldPoint(screenPoint);
			transform.position = uiPosition + _offset;
			//transform.position = transform.parent.InverseTransformPoint(uiPosition);

			_previousTransformPosition = _followTransform.position;
			_previousOffset = _offset;
			_previousWorldCameraPosition = _worldCamera.transform.position;
			_previousWorldCameraRotation = _worldCamera.transform.rotation;
		}
	}
}