using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace IntimeWar.View
{
    public class AttachmentView : BaseView
    {
        [SerializeField]
        GameObject[] _buttons;

        public GameObject[] GetButtons()
        {
            return _buttons;
        }

    }
}
