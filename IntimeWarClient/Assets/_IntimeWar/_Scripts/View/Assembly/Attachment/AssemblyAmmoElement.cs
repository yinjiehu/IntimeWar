using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyAmmoElement : MonoBehaviour
	{
		[SerializeField]
		Text _displayName;
		[SerializeField]
		Text _introduceSimple;

		string _ammoAttID;

		public struct Model
		{
			public string AmmoID;
			public string DisplayName;
			public string IntroduceSimple;
		}

		public void BindData(Model data)
		{
			_ammoAttID = data.AmmoID;
			_displayName.text = data.DisplayName;
			_introduceSimple.text = data.IntroduceSimple;
		}

		public void OnClickAttach()
		{
			AssemblyPresenter.SelectedAmmoID = _ammoAttID;
			AssemblyPresenter.LoadActiveAttachmentAction();
		}
	}
}