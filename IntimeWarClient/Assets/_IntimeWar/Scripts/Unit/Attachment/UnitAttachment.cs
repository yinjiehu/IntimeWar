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
        string _skillID;
        public virtual string SkillID { set { _skillID = value; } get { return _skillID; } }

        string _slotID;
        public virtual string SlotID { set { _slotID = value; } get { return _slotID; } }

        public override string AbilityID
        {
            get
            {
                return string.Format("{0}_{1}", SlotID, _skillID);
            }
        }
    }
}