using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyMechLoadedAttachmentElement : MonoBehaviour
	{
		[SerializeField]
		Text _displayName;

		[SerializeField]
		Text _category;

		[SerializeField]
		Text _loadedAmmoDisplayName;
		[SerializeField]
		Text _loadedAmmoTotalCount;

		[SerializeField]
		Haruna.UI.ListMemberUpdater _parameters;

		public struct Model
		{
			public string AttachmentDisplayName;
			public string ControlType;
			public string Category;

			public string LoadedAmmoDisplayName;
			public string LoadedAmmoTotalCount;

			public List<AssemblyActiveAttachmentParameterElement.Model> ParameterList;
		}
		
		public void BindData(Model data)
		{
			_displayName.text = data.AttachmentDisplayName;

			//_controlTypeR.SetActive(data.ControlType == AttachmentControlTypeEnum.Rapid.ToString());
			//_controlTypeS.SetActive(data.ControlType == AttachmentControlTypeEnum.Single.ToString());
			//_controlTypeG.SetActive(data.ControlType == AttachmentControlTypeEnum.Grenade.ToString());
			//_controlTypeA.SetActive(data.ControlType == AttachmentControlTypeEnum.Artillery.ToString());

			_category.text = data.Category;

			_loadedAmmoDisplayName.text = data.LoadedAmmoDisplayName;
			_loadedAmmoTotalCount.text = data.LoadedAmmoTotalCount;

			_parameters.OnListUpdate(data.ParameterList, (d, go, index) =>
			{
				var el = go.GetComponent<AssemblyActiveAttachmentParameterElement>();
				el.BindData(new AssemblyActiveAttachmentParameterElement.Model()
				{
					Label = d.Label,
					Number = d.Number
				});
			});
		}
	}
}