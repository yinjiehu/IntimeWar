using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class AssemblyView : BaseView
	{
		[SerializeField]
		Haruna.UI.ListMemberUpdater _mechListUpdater;
		
		public override void Show()
		{
			base.Show();
			//SelectedVehicleID = GlobalCache.GetPlayerStatus().CurrentSelectedID;
			//AssemblyPresenter.ShowMechDesignAction();
			//AssemblyPresenter.ShowMechDetailAction();

			var vehidleSettingsList = GlobalCache.GetVehicleSettingsCollection().Settings;
			_mechListUpdater.OnListUpdate(vehidleSettingsList, (MechSquadShared.VehicleSettings settings, GameObject go, int index) =>
			{
				go.GetComponent<AssemblyMechListElement>().BindData(new AssemblyMechListElement.Model()
				{
					VehicleID = settings.VehicleID,
					DisplayNameFull = TextForVehicle.GetNameSimple(settings.VehicleID),
					Icon = GlobalCache.GetIcon().GetVehicleIcon(settings.IconPath),
					SimpleName = TextForVehicle.GetNameSimple(settings.VehicleID),
					CategoryName = "--",
				});

				if (GlobalCache.GetPlayerStatus().CurrentSelectedID == settings.VehicleID)
				{
					AssemblyPresenter.SelectedVehicleID = settings.VehicleID;
					go.GetComponent<Toggle>().isOn = true;
				}
			});
		}

		void Refesh()
		{
			AssemblyPresenter.ShowMechDesignAction();
			AssemblyPresenter.ShowMechDetailAction();
		}
	}
}