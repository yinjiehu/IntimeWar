using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyAmmoListSubView : BaseView
	{
		[SerializeField]
		Haruna.UI.ListMemberUpdater _list;
		
		public void BindToView(List<AssemblyAmmoElement.Model> data)
		{
			_list.OnListUpdate(data, (d, go, index) =>
			{
				var el = go.GetComponent<AssemblyAmmoElement>();
				el.BindData(d);
			});
		}
	}
}