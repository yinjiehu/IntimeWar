using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace Haruna.UI
{
	public class ListMemberUpdater : MonoBehaviour
	{
		[SerializeField]
		Transform _template;

		[SerializeField]
		bool _isAsync;
		[SerializeField]
		[Inspector.HarunaInspect(ShowIf = "_isAsync", Indent = 0f)]
		int _initialDisplayCount = 1;
		[SerializeField]
		[Inspector.HarunaInspect(ShowIf = "_isAsync", Indent = 0f)]
		int _displayCountEveryFrame = 1;

		int _currentIndex;
		List<Transform> _instanceList = new List<Transform>();

		private void Awake()
		{
			if(_template == null)
			{
				Debug.LogErrorFormat(this, "template is null !!");
				return;
			}
			_template.gameObject.SetActive(false);
		}
		
		public void OnListUpdate<T>(IList<T> data, Action<T, GameObject, int> onUpdateCallback)
		{
			if (this._isAsync)
			{
				if (gameObject.activeInHierarchy && gameObject.activeSelf)
				{
					StopAllCoroutines();
					StartCoroutine(UpdateCoroutine(data, onUpdateCallback));
				}
				else
				{
					Debug.LogErrorFormat(this, "game object is not active, can not use coroutine to async update");
				}
			}
			else
			{
				UpdateList(data, onUpdateCallback, 0, data.Count);
				DestroyExcessInstance(data.Count);
			}
		}

		void UpdateList<T>(IList<T> data, Action<T, GameObject, int> Callback, int currentIndex, int toInstantiateCount)
		{
			var targetIndex = currentIndex + toInstantiateCount;
			for (var i = currentIndex; i < targetIndex; i++)
			{
				if (i >= data.Count)
				{
					break;
				}

				var d = data[i];
				Transform instance = null;
				if(i >= _instanceList.Count)
				{
					instance = Instantiate(_template);
					instance.SetParent(transform, false);
					instance.localPosition = Vector3.zero;
					instance.localScale = _template.localScale;
					instance.gameObject.SetActive(true);

					_instanceList.Add(instance);
				}
				else
				{
					instance = _instanceList[i];
					instance.gameObject.SetActive(true);
				}

				Callback(d, instance.gameObject, i);
			}
		}

		IEnumerator UpdateCoroutine<T>(IList<T> data, Action<T, GameObject, int> callback)
		{
			UpdateList(data, callback, 0, _initialDisplayCount);
			yield return null;

			var currentIndex = _initialDisplayCount;
			while (currentIndex < data.Count)
			{
				UpdateList<T>(data, callback, currentIndex, _displayCountEveryFrame);
				currentIndex += _displayCountEveryFrame;
				yield return null;
			}

			DestroyExcessInstance(data.Count);
		}
		
		void DestroyExcessInstance(int dataCount)
		{
			if(_instanceList.Count > dataCount)
			{
				for (var i = dataCount; i < _instanceList.Count; i++)
				{
					//Destroy(_instanceList[i]);
					_instanceList[i].gameObject.SetActive(false);
				}
				//_instanceList.RemoveRange(dataCount, _instanceList.Count - 1);
			}
		}
	}
}