using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace IntimeWar.Battle
{
	public class CameraZoomControl : MonoBehaviour
	{
		MapSightEdge _mapSightEdge;
		Deftly.DeftlyCamera _deflyCamera;

		int _currentZoomIndex;

		private void Start()
		{
			_mapSightEdge = FindObjectOfType<MapSightEdge>();
			_deflyCamera = Camera.main.GetComponent<Deftly.DeftlyCamera>();

			_deflyCamera.SetMapSightEdge(
				_mapSightEdge.LeftEdge.position.x,
				_mapSightEdge.RightEdge.position.x,
				_mapSightEdge.ForwardEdge.position.z,
				_mapSightEdge.BackwardEdge.position.z);

			_currentZoomIndex = CameraParameter.DefaultPresetIndex;
			SetCameraOffset(CameraParameter.CameraHeighPreset[CameraParameter.DefaultPresetIndex]);
		}

		private void Update()
		{
			_deflyCamera.SetMapSightEdge(
				_mapSightEdge.LeftEdge.position.x,
				_mapSightEdge.RightEdge.position.x,
				_mapSightEdge.ForwardEdge.position.z,
				_mapSightEdge.BackwardEdge.position.z);

			UpdateCameraX();
		}

		void UpdateCameraX()
		{
			var y = _deflyCamera.transform.position.y;
			var angleX = CameraParameter.GetCameraX(y);
			_deflyCamera.transform.rotation = Quaternion.Euler(angleX, 0, 0);
		}

		public void SwitchCameraHeightPreset()
		{
			var zoomParametersPreset = CameraParameter.CameraHeighPreset;

			_currentZoomIndex++;
			if (_currentZoomIndex >= zoomParametersPreset.Length)
			{
				_currentZoomIndex = 0;
			}
			
			var zoomParam = zoomParametersPreset[_currentZoomIndex];
			SetCameraOffset(zoomParam);
		}

		void SetCameraOffset(float height)
		{
			var angleX = CameraParameter.GetCameraX(height);
			var zshift = height * Mathf.Tan(Mathf.Deg2Rad * (90 - angleX)) * -1;
			_deflyCamera.Offset = new Vector3(0, height, zshift);
		}
	}
}