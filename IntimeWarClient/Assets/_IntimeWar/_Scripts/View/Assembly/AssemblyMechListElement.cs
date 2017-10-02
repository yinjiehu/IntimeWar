using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyMechListElement : MonoBehaviour
	{
		//[SerializeField]
		//Text _displayName;
		[SerializeField]
		Image _icon;
		[SerializeField]
		Text _simpleName;
		[SerializeField]
		Text _category;

		public struct Model
		{
			public string DisplayNameFull;
			public string VehicleID;
			public Sprite Icon;
			public string SimpleName;
			public string CategoryName;
		}
		string _vehicleID;

		public void BindData(Model data)
		{
			//_displayName.text = data.DisplayNameFull;
			_vehicleID = data.VehicleID;
			_icon.sprite = data.Icon;
			_simpleName.text = data.SimpleName;
			_category.text = data.CategoryName;
		}

		public void OnClickVehicle(bool isOn)
		{
			if (isOn)
			{
				GlobalCache.GetPlayerStatus().CurrentSelectedID = _vehicleID;
				DemoPlayerStatus.SaveToPlayerPref();

				AssemblyPresenter.SelectedVehicleID = _vehicleID;
				AssemblyPresenter.ShowMechDesignAction();
				AssemblyPresenter.ShowMechDetailAction();
			}
		}
	}
}