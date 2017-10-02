using System;
using Haruna.Inspector;
using UnityEngine;
using WhiteCat.Tween;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public class LockOnIconOnUnit : MonoBehaviour
	{
		public bool IsVisible { get { return gameObject.activeSelf; } }

		Transform _followTarget;

		[SerializeField]
		Tweener _showTweener;

		[SerializeField]
		Tweener _hideTweener;

		public void Hide()
		{
			gameObject.SetActive(false);
			//RecycleThis();
			//_showTweener.enabled = false;
			//_hideTweener.PlayForward();
		}

		public void Show(Transform followTarget)
		{
			gameObject.SetActive(true);

			_followTarget = followTarget;
			//_showTweener.PlayForward();
		}

		private void Update()
		{
			if (_followTarget != null)
			{
				transform.position = _followTarget.position;
			}
		}
	}
}