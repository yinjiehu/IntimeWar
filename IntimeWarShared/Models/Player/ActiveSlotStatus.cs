using System.Collections.Generic;

namespace Shared
{
    [System.Serializable]
    public class ActiveSlotStatus
    {
        public string VehicleID;
        public string SlotID;

        public bool Attched { get { return !string.IsNullOrEmpty(AttachmentID); } }
        public string AttachmentID;

        public ActiveSlotStatus()
        {
        }
    }
}
