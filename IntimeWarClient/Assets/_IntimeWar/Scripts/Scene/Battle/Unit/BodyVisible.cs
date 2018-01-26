using IntimeWar.View;
using UnityEngine;
using View;
using YJH.Unit;

namespace IntimeWar.Unit
{
    public class BodyVisible : Ability
    {
        public override void Init()
        {
            base.Init();
            OnBodyValueChange(false);
            _unit.STS.BodyVisible.EvOnValueChange += OnBodyValueChange;
        }

        void OnBodyValueChange(bool visible)
        {
            GetComponent<SpriteRenderer>().enabled = visible;
        }
    }
}