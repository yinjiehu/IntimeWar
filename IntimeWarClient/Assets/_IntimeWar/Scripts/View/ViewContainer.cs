using System.Linq;
using UnityEngine;

namespace View
{
	public class ViewContainer : MonoBehaviour
	{
		[SerializeField]
		BaseView _viewPrefab;
		public BaseView ViewPrefab { get { return _viewPrefab; } }

		BaseView _viewInstance;
		public BaseView ViewInstance { get { return GetInstance(); } }

		public enum InstantiateTypeEnum
		{
			Start,
			Delay,
			Manual
		}
		[SerializeField]
		InstantiateTypeEnum _instantiateType;

		[SerializeField]
		float _delaySeconds = 0;
		float _elapsedTime = 0;

		private void Start()
		{
			if (_instantiateType == InstantiateTypeEnum.Start)
			{
				InstantiateInstance();
			}
		}

		private void Update()
		{
			if (_instantiateType != InstantiateTypeEnum.Delay)
			{
				enabled = false;
				return;
			}

			if (_viewInstance != null)
			{
				enabled = false;
				return;
			}

			if (_elapsedTime > _delaySeconds)
			{
				InstantiateInstance();
				enabled = false;
				return;
			}

			_elapsedTime += Time.deltaTime;
		}

		public void InstantiateInstance()
		{
			if (_viewInstance != null)
				return;

			var fromInstance = _viewPrefab.transform == transform || _viewPrefab.transform.parent == transform;
			if (fromInstance)
			{
				_viewInstance = _viewPrefab;
			}
			else
			{
				_viewInstance = Instantiate(_viewPrefab);
				_viewInstance.name = _viewPrefab.name;
				var rectTransform = _viewInstance.GetComponent<RectTransform>();
				rectTransform.SetParent(transform);
				rectTransform.anchoredPosition = _viewPrefab.transform.GetComponent<RectTransform>().anchoredPosition;
				rectTransform.localScale = _viewPrefab.transform.localScale;
				rectTransform.sizeDelta = _viewPrefab.transform.GetComponent<RectTransform>().sizeDelta;
			}
			_viewInstance.Init();

			if (!fromInstance)
			{
				var containers = _viewInstance.GetComponentsInChildren<ViewContainer>(true);
				ViewManager.Instance.AddSubViewContainer(containers);
			}
		}

		public BaseView GetInstance()
		{
			InstantiateInstance();

			return _viewInstance;
		}
	}
}