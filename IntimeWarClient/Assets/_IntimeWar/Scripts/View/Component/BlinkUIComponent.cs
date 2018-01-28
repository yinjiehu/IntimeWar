using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace View
{
	[RequireComponent(typeof(CanvasGroup))]
	public class BlinkUIComponent : MonoBehaviour
	{
		public enum FinallyVisibleTypeEnum
		{
			Visible,
			Invisible,
			Random,
		}
		[SerializeField]
		FinallyVisibleTypeEnum _finallyVisible;
		public FinallyVisibleTypeEnum FinallyVisible { set { _finallyVisible = value; } get { return _finallyVisible; } }

		[SerializeField]
		float _intervalMin = 0.2f;
		public float IntervalMin { set { _intervalMin = value; } get { return _intervalMin; } }
		[SerializeField]
		float _intervalMax = 0.5f;
		public float IntervalMax { set { _intervalMax = value; } get { return _intervalMax; } }
		[SerializeField]
		float _duration = 3;
		public float Duration { set { _duration = value; } get { return _duration; } }

		CanvasGroup _canvasGroup;

		void Start()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}
		public void Blink()
		{
			StopAllCoroutines();
			StartCoroutine(BlinkCoroutine(_duration, _finallyVisible));
		}
		public void Blink(float duration)
		{
			StopAllCoroutines();
			StartCoroutine(BlinkCoroutine(duration, _finallyVisible));
		}
		public void Blink(FinallyVisibleTypeEnum finalVisible)
		{
			if(gameObject.activeInHierarchy)
			{
				StopAllCoroutines();
				StartCoroutine(BlinkCoroutine(_duration, finalVisible));
			}
		}
		public void Blink(float duration, FinallyVisibleTypeEnum finalVisible)
		{
			StopAllCoroutines();
			StartCoroutine(BlinkCoroutine(duration, finalVisible));
		}

		IEnumerator BlinkCoroutine(float duration, FinallyVisibleTypeEnum finalVisible)
		{
			float elapsedTime = 0;
			
			while (elapsedTime < duration)
			{

				_canvasGroup.alpha = _canvasGroup.alpha == 0 ? 1 : 0;

				var temp = UnityEngine.Random.Range(_intervalMin, _intervalMax);
				elapsedTime += temp;
				yield return new WaitForSeconds(temp);
			}


			_canvasGroup.alpha = finalVisible == FinallyVisibleTypeEnum.Visible ? 1 : 0;

		}
#if UNITY_EDITOR
		[SerializeField]
		Haruna.Inspector.InspectorButton _test;
		public void Test()
		{
			Blink();
		}
#endif
	}
}