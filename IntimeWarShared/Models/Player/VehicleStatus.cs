using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    [System.Serializable]
    public class PlayerStatus
    {
        public string CurrentSelectedID;
        public List<VehicleStatus> Garage = new List<VehicleStatus>();

        public VehicleStatus GetCurrentSelected()
        {
            return GetVehicle(CurrentSelectedID);
        }
        public VehicleStatus GetVehicle(string vehicleID)
        {
            if (Garage != null)
            {
                var ret = Garage.First(s => s.VehicleID == vehicleID);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        public bool IsVehicleExist(string vehicleID)
        {
            return Garage.First(s => s.VehicleID == vehicleID) != null;
        }
    }
}
