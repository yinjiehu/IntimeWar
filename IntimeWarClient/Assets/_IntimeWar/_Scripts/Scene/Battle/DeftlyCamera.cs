// (c) Copyright Cleverous 2015. All rights reserved.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using WhiteCat.Tween;

namespace Deftly
{
	[AddComponentMenu("Deftly/Deftly Camera")]
	[ExecuteInEditMode]
	public class DeftlyCamera : MonoBehaviour
	{
		static DeftlyCamera _instance;
		public static DeftlyCamera Get()
		{
			if (_instance != null)
				return _instance;

			_instance = Object.FindObjectOfType<DeftlyCamera>();
			if (_instance != null)
				return _instance;

			Debug.LogError("can not find instance!");
			return null;
		}

		public enum MoveStyle { Loose, Stiff }
		public MoveStyle FollowingStyle;

		public enum TrackingStyle { PositionalAverage, AimingAverage }
		public TrackingStyle Tracking;

		public float TrackDistance;
		public float TrackSpeed;

		public List<GameObject> Targets = new List<GameObject>();
		public Vector3 Offset;

		public GameObject Arbiter; // The Arbiter also provides a source for orientation on the Controller.

		private Vector3[] _vectorArray;
		private Vector3 _averagePos;

		//private Tweener _whiteScreen;

		void Reset()
		{
			FollowingStyle = MoveStyle.Loose;
			Tracking = TrackingStyle.AimingAverage;
			TrackDistance = 2f;
			TrackSpeed = 5f;
			Offset = new Vector3(0.5f, 10.0f, -0.5f);
			Targets = new List<GameObject>();
		}
		void OnEnable()
		{
			if (Arbiter != null) return;
			Arbiter = new GameObject { name = "Camera Arbiter" };
			Arbiter.transform.SetParent(transform);
		}
		//***********************************************************
		//void Start()
		//{
		//	_whiteScreen = transform.FindChild("WhiteScreen").GetComponent<Tweener>();
		//}
		//public void ResetWhiteScrennColor()     //Tweener  event  
		//{
		//	_whiteScreen.gameObject.SetActive(false);
		//}

		//IEnumerator ScrennWhite(float duration)
		//{
		//	var time = Mathf.Max(0,duration - 3);
		//	_whiteScreen.gameObject.SetActive(true);
		//	yield return new WaitForSeconds(time);
		//	_whiteScreen.enabled = true;
		//}

		//public void Blind(float duration)
		//{
		//	StartCoroutine("ScrennWhite", duration);
		//}

		//void Update()
		//{
		//	if (Input.GetKeyDown(KeyCode.B))
		//	{
		//		Blind(3f);
		//	}
		//}
		//*************************************************************	
		void FixedUpdate()
		{
			if (Targets.Count > 0 && Targets.All(tar => tar != null))
			{
				FollowTargets();
				SetArbiterTransform();

				_averagePos.Set(0, 0, 0); // precaution
			}
			else
			{
				//Debug.LogWarning("Main Camera must have one or more targets!");
			}
		}

		public void ApplyTargetPositionImmidiatly()
		{
			_averagePos = GetAveragePos();
			var targetPostion = CalculateEdgeCorrection(_averagePos + Offset);
			transform.position = targetPostion;
		}

		void FollowTargets()
		{
			_averagePos = GetAveragePos();

			if (!Application.isPlaying)
			{
				transform.position = _averagePos + Offset;
				transform.LookAt(_averagePos);
			}
			else
			{
				var targetPostion = CalculateEdgeCorrection(_averagePos + Offset);

				if (FollowingStyle == MoveStyle.Loose)
					transform.position = Vector3.Lerp(transform.position, targetPostion, Time.deltaTime * TrackSpeed);
				else
					transform.position = targetPostion;
			}
		}

		void SetArbiterTransform()
		{
			if (!Arbiter) return;
			Arbiter.transform.position = new Vector3(_averagePos.x, gameObject.transform.position.y, _averagePos.z);
			Arbiter.transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y, 0);
		}
		Vector3 GetAveragePos()
		{
			_vectorArray = new Vector3[Targets.Count];
			for (int i = 0; i < Targets.Count; i++)
			{
				if (Targets[i] == null)
				{
					// handle in case a target was removed
					Debug.LogWarning("A Camera GameObject Target is null! Removing entry.");

					// find an alternative target
					int s = i > 0
						? Targets[i - 1] != null
							? i - 1
							: i + 1
						: 1;

					Debug.Log(s);

					_vectorArray[s] = Tracking == TrackingStyle.PositionalAverage
						? Targets[s].transform.position
						: Targets[s].transform.position + (Targets[s].transform.forward * TrackDistance);

					_averagePos += _vectorArray[s];

					// remove the null target
					Targets.RemoveAt(i);
				}
				else
				{
					// business as usual
					_vectorArray[i] = Tracking == TrackingStyle.PositionalAverage
					   ? Targets[i].transform.position
					   : Targets[i].transform.position + (Targets[i].transform.forward * TrackDistance);

					_averagePos += _vectorArray[i];
				}
			}

			//CorrectAveragePositionByMapArea();

			return _averagePos / _vectorArray.Length;
		}

		float _mapLeftSightEdge;
		float _mapRightSightEdge;
		float _mapForwardSightEdge;
		float _mapBackwardSightEdge;

		float _tanLeftRight;
		float _tanForward;
		float _tanBackward;

		float _actualLeftEdge;
		float _actualRightEdge;
		float _actualForwardEdge;
		float _actualBackwardEdge;

		public void SetMapSightEdge(float left, float right, float forward, float backward)
		{
			_mapLeftSightEdge = left;
			_mapRightSightEdge = right;
			_mapForwardSightEdge = forward;
			_mapBackwardSightEdge = backward;
		}

		//public void SetCameraField(float fieldView, float cameraX)
		//{
		//	_tanLeftRight = Mathf.Tan(Mathf.Deg2Rad * fieldView / 2);
		//	_tanForward = Mathf.Tan(Mathf.Deg2Rad * (cameraX + fieldView / 2));
		//	_tanBackward = Mathf.Tan(Mathf.Deg2Rad * (cameraX - fieldView / 2));
		//}

		Vector3 CalculateEdgeCorrection(Vector3 targetPostion)
		{
			var fieldView = Camera.main.fieldOfView;
			var cameraX = 90 - transform.rotation.eulerAngles.x;
			_tanLeftRight = Mathf.Tan(Mathf.Deg2Rad * fieldView / 2);
			_tanForward = Mathf.Tan(Mathf.Deg2Rad * (cameraX + fieldView / 2));
			_tanBackward = Mathf.Tan(Mathf.Deg2Rad * (cameraX - fieldView / 2));

			var cameraLeftEdge = _actualLeftEdge = _mapLeftSightEdge + transform.position.y * _tanLeftRight * Screen.width / Screen.height;
			var cameraRightEdge = _actualRightEdge = _mapRightSightEdge - transform.position.y * _tanLeftRight * Screen.width / Screen.height;
			var cameraForwardEdge = _actualForwardEdge = _mapForwardSightEdge - transform.position.y * _tanForward;
			float cameraBackEdge = 0;
			if (_mapBackwardSightEdge < _mapBackwardSightEdge + transform.position.y * _tanBackward)
			{
				cameraBackEdge = _mapBackwardSightEdge + transform.position.y * _tanBackward;
			}
			else
			{
				cameraBackEdge = _mapBackwardSightEdge - transform.position.y * _tanBackward;
			}
			_actualBackwardEdge = cameraBackEdge;
			if (targetPostion.x < cameraLeftEdge)
				targetPostion.x = cameraLeftEdge;
			else if (targetPostion.x > cameraRightEdge)
				targetPostion.x = cameraRightEdge;

			if (targetPostion.z > cameraForwardEdge)
				targetPostion.z = cameraForwardEdge;
			else if (targetPostion.z < cameraBackEdge)
				targetPostion.z = cameraBackEdge;

			return targetPostion;
		}

		[SerializeField]
		bool _showEdgeGizmo;
		[SerializeField]
		Color _gizmoColor;
		private void OnDrawGizmos()
		{
			if (!_showEdgeGizmo)
				return;

			Gizmos.DrawCube(new Vector3(_actualLeftEdge, 0, 0), new Vector3(5, 80, 300));
			Gizmos.DrawCube(new Vector3(_actualRightEdge, 0, 0), new Vector3(5, 80, 300));
			Gizmos.DrawCube(new Vector3(0, 0, _actualForwardEdge), new Vector3(300, 80, 5));
			Gizmos.DrawCube(new Vector3(0, 0, _actualBackwardEdge), new Vector3(300, 80, 5));
		}


		public GameObject GetArbiter()
		{
			return Arbiter;
		}
		void OnDestroy()
		{
			DestroyImmediate(Arbiter);
		}

		public void ResetTarget(GameObject[] objs)
		{
			Targets.Clear();
			Targets.AddRange(objs);
		}

		//public void SetOffSetY(float y, float duration)
		//{
		//	StopAllCoroutines();
		//	StartCoroutine(ChangeOffsetYCoroutine(y, duration));
		//}

		//IEnumerator ChangeOffsetYCoroutine(float targetY, float duration)
		//{
		//	var elapsedTime = 0f;
		//	var startY = Offset.y;
		//	while(elapsedTime < duration)
		//	{
		//		Offset.y = Mathf.Lerp(startY, targetY, elapsedTime / duration);
		//		yield return null;
		//		elapsedTime += Time.deltaTime;
		//	}
		//	Offset.y = targetY;
		//}
	}
}