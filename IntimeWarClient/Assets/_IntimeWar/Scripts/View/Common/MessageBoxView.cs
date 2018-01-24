using System;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace IntimeWar.View
{
	public class MessageBoxView : BaseView
	{
		[SerializeField]
		Text _text;

		[SerializeField]
		Action _onHideStart;

		[SerializeField]
		Action _onHideComplete;

		public static void Show(string message, Action onHideComplete = null)
		{
			var view = ViewManager.Instance.GetView<MessageBoxView>();
			view._text.text = message;
			view._onHideComplete = onHideComplete;
			view.Show();
		}

		public void OnHideStart()
		{
			if (_onHideStart != null)
				_onHideStart();
		}

		public void OnHideComplete()
		{
			if (_onHideComplete != null)
				_onHideComplete();
		}
	}
}