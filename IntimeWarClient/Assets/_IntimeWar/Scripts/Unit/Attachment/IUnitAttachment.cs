using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YJH.Unit
{
    public interface IUnitAttachment
    {
        string SkillID { get; }
        string SlotID { get; }
    }
}