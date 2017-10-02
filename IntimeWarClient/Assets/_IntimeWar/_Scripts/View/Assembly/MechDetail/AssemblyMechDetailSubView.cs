using UnityEngine;
using Haruna.UI;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MechSquad.View
{
	public class AssemblyMechDetailSubView : BaseView
	{
		[SerializeField]
		ListMemberUpdater _basicParametersRoot;
		[SerializeField]
		ListMemberUpdater _attachmentsRoot;

		[SerializeField]
		Text _fullName;

		[SerializeField]
		Text _Weight;
		[SerializeField]
		Text _category;
		[SerializeField]
		Text _sp;

		[SerializeField]
		Text _introduce;

		[SerializeField]
		GameObject _startBtn;
		[SerializeField]
		GameObject _completeBtn;

		public struct Model
		{
			public List<AssemblyMechParameterElement.Model> Parameters;
			public List<AssemblyMechLoadedAttachmentElement.Model> Attachments;

			public string FullName;

			public string Weight;
			public string Category;
			public string SP;

			public string Introduce;

			public bool ShowStartBtn;
			public bool ShowCompleteBtn;
		}

		public void BindToView(Model data)
		{
			_fullName.text = data.FullName;
			_Weight.text = data.Weight;
			_category.text = data.Category;
			_sp.text = data.SP;

			_basicParametersRoot.OnListUpdate(data.Parameters, (d, go, index) =>
			{
				go.GetComponent<AssemblyMechParameterElement>().BindData(d);
			});
			_attachmentsRoot.OnListUpdate(data.Attachments, (d, go, index) =>
			{
				go.GetComponent<AssemblyMechLoadedAttachmentElement>().BindData(d);
			});

			_introduce.text = data.Introduce;

			_startBtn.SetActive(data.ShowStartBtn);
			_completeBtn.SetActive(data.ShowCompleteBtn);
		}
	}
}