using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YJH.Unit
{
    public abstract class UnitAttachment : Ability, IUnitAttachment
    {
        string _attachmentID;
        public virtual string AttachmentID { set { _attachmentID = value; } get { return _attachmentID; } }

        string _attachToSlotID;
        public virtual string AttachToSlotID { set { _attachToSlotID = value; } get { return _attachToSlotID; } }

        public override string AbilityID
        {
            get
            {
                return string.Format("{0}_{1}", AttachToSlotID, _attachmentID);
            }
        }
    }
}