using System;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class VersionView : BaseView
	{
		[SerializeField]
		Text _versionDisplay;

		string _devFormat = "Development Build   {0}\n Head Shot Studio All Rights Reserverd";

		private void Start()
		{
			_versionDisplay.text = string.Format(_devFormat, Application.version);
		}
	}
}