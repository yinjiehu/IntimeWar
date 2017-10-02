using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyPassiveAttachmentListSubView : BaseView
	{
		[SerializeField]
		Haruna.UI.ListMemberUpdater _list;
		
		public void BindToView(List<AssemblyPassiveAttachmentElement.Model> data)
		{
			_list.OnListUpdate(data, (d, go, index) =>
			{
				var el = go.GetComponent<AssemblyPassiveAttachmentElement>();
				el.BindData(d);
			});
		}

		public void OnClickUnload()
		{
			AssemblyPresenter.UnloadPassiveSlot();
		}
	}
}