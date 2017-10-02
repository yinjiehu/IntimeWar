using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyPassiveAttachmentElement : MonoBehaviour
	{
		[SerializeField]
		Text _displayName;
		[SerializeField]
		Text _introduceSimple;
		
		string _attachmentID;

		public struct Model
		{
			public string AttachmentID;
			public string DisplayName;
			public string IntroduceSimple;
		}

		public void BindData(Model data)
		{
			_attachmentID = data.AttachmentID;
			_displayName.text = data.DisplayName;
			_introduceSimple.text = data.IntroduceSimple;
		}

		public void OnClickAttach()
		{
			AssemblyPresenter.SelectedPassiveAttachmentID = _attachmentID;
			AssemblyPresenter.LoadPassiveAttachmentAction();
		}
	}
}