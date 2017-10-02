using MechSquad.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Haruna.Pool;
using System.Collections;

namespace MechSquad.View
{
	public class RaidarView : BaseView
	{
		[SerializeField]
		RectTransform _content;

		[SerializeField]
		float _maxRadius = 100;

		float _currentRaidarScale;

		BattleUnit _playerUnit;
		RaidarSystem _playerRaidar;
		IMainFireControlSystem _fireControl;

		Dictionary<int, RaidarElement> _raidarElements = new Dictionary<int, RaidarElement>();

		public RaidarElement CreateFromPrefab(BattleUnit unit, RaidarElement prefab)
		{
			var ins = Instantiate(prefab);
			ins.name = prefab.name;
			ins.transform.SetParent(_content);
			ins.transform.localScale = prefab.transform.localScale;

			var el = ins.GetComponent<RaidarElement>();
			_raidarElements.Add(unit.SeqNo, el);

			return el;
		}

		public void RemoveRaidarElement(int seqNo)
		{
			if (_raidarElements.ContainsKey(seqNo))
			{
				Destroy(_raidarElements[seqNo].gameObject);
				_raidarElements.Remove(seqNo);
			}
		}

		private void Update()
		{
			//}
			//void IntervalUpdate()
			//{
			if (_playerUnit == null)
				GetPlayerUnit();

			if (_playerUnit == null)
				return;

			UpdateRaidarScale();

			var currentLockTarget = _fireControl.GetAimingTarget();
			var targets = _playerRaidar.GetSharedTargets();
			foreach (var kv in _raidarElements)
			{
				if (kv.Value == null)
					continue;

				var unit = kv.Value.Unit;
				if (unit.Team != _playerUnit.Team)
				{
					if (!targets.ContainsKey(kv.Key))
						kv.Value.Hide();
					else
						UpdateElementPosition(kv.Value);
				}
				else
				{
					UpdateElementPosition(kv.Value);
				}

				kv.Value.SetLock(currentLockTarget == kv.Value.Unit);
			}
		}

		void UpdateElementPosition(RaidarElement element)
		{
			var unit = element.Unit;

			var angle = Quaternion.FromToRotation(Vector3.forward, unit.Model.position - _playerUnit.Model.position).eulerAngles.y;
			var distance = Util.GetHorizontalDistance(unit.Model.position, _playerUnit.Model.position);
			
			var screenDistance = distance * _currentRaidarScale;

			if (screenDistance > _maxRadius)
			{
				screenDistance = _maxRadius;
				element.SetOut();
				element.transform.rotation = Quaternion.Euler(0, 0, 180 - angle);
			}
			else
			{
				element.SetIn();
			}

			var uiDirection = Quaternion.Euler(0, 0, -angle) * Vector3.up;
			element.transform.localPosition = uiDirection.normalized * screenDistance;
		}

		void UpdateRaidarScale()
		{
			//var camera = Camera.main;
			//Vector3 leftPosition;
			//Vector3 rightPosition;
			//{
			//	var ray = Camera.main.ViewportPointToRay(new Vector3(0f, 0.5f));
			//	RaycastHit hit;
			//	if (!Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground")))
			//	{
			//		Debug.LogWarning("camera left out ground!");
			//		return;
			//	}
			//	leftPosition = hit.point;
			//}
			//{
			//	var ray = Camera.main.ViewportPointToRay(new Vector3(1, 0.5f));
			//	RaycastHit hit;
			//	if (!Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground")))
			//	{
			//		Debug.LogWarning("camera left out ground!");
			//		return;
			//	}
			//	rightPosition = hit.point;
			//}
			//var width = Mathf.Abs(leftPosition.x - rightPosition.x);
			//_currentRaidarScale = 150f / width;

			_currentRaidarScale = 150f / 400f;
		}

		private void OnDrawGizmos()
		{
			{
				Gizmos.color = Color.red;
				var ray = Camera.main.ViewportPointToRay(new Vector3(0f, 0.5f));
				Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 1000);
			}

			{
				Gizmos.color = Color.yellow;
				var ray = Camera.main.ViewportPointToRay(new Vector3(1, 0.5f));
				Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 1000);
			}
		}

		void GetPlayerUnit()
		{
			_playerUnit = UnitManager.ThisClientPlayerUnit;

			if (_playerUnit != null)
			{
				_playerRaidar = _playerUnit.GetAbility<RaidarSystem>();
				_fireControl = _playerUnit.GetAbility<IMainFireControlSystem>();
			}
		}

		public void SwitchRaidarScale()
		{
			FindObjectOfType<CameraZoomControl>().SwitchCameraHeightPreset();
		}
	}
}