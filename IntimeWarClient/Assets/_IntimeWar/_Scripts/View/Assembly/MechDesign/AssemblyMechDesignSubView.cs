using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyMechDesignSubView : BaseView
	{
		[SerializeField]
		//List<AssemblyMechDesignActiveSlotElement> _activeSlots;
		Haruna.UI.ListMemberUpdater _activeSlotList;


		[SerializeField]
		Haruna.UI.ListMemberUpdater _passiveSlotLeft;
		//[SerializeField]
		//Haruna.UI.ListMemberUpdater _passiveSlotRight;

		[SerializeField]
		Image _vehicleImage;
		public struct Model
		{
			public string VehicleID;

			public List<AssemblyMechDesignActiveSlotElement.Model> ActiveSlots;

			public List<string> PassiveSlot;
			//public List<string> PassiveSlotRight;

			public Sprite vehicleImage;
		}

		string _vehicleID;
		public string CurrentExamingVehicle { get { return _vehicleID; } }

		public void BindDataToView(Model data)
		{
			_vehicleID = data.VehicleID;
			_vehicleImage.sprite = data.vehicleImage;
			_activeSlotList.OnListUpdate(data.ActiveSlots, (d, go, index) =>
			{
				go.GetComponent<AssemblyMechDesignActiveSlotElement>().BindData(d);
			});

			_passiveSlotLeft.OnListUpdate(data.PassiveSlot, (t, go, index) =>
			{
				go.GetComponent<AssemblyMechDesignPassiveSlotElement>().BindData((byte)index, t);
			});
			//_passiveSlotLeft.OnListUpdate(data.PassiveSlotRight, (t, go, index) =>
			//{
			//	go.GetComponent<AssemblyMechDesignPassiveSlotElement>().BindData((byte)(index * 2 + 1), t);
			//});
		}
	}
}