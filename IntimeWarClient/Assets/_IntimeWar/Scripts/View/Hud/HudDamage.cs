using IntimeWar.Battle;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WhiteCat.Tween;
using YJH.Unit;
using Haruna.Utility;

namespace IntimeWar.View
{
    public class HudDamage : HudInstance
    {
        BattleUnit _observerUnit;
        HudInstance _hudInstance;

        [SerializeField]
        Text _numberToDisplay;

        Vector3 _originOffset;
        [SerializeField]
        Vector2 _randomOffsetVar;

        [SerializeField]
        UnityEvent _onShowEvent;

        private void Awake()
        {
            _originOffset = Offset;
        }

        public void ShowNumber(Transform follow, float number)
        {
            WorldCamera = Camera.main;
            UICamera = Util.GetUICamera();
            FollowTransform = follow;

            Offset = _originOffset +
                new Vector3(UnityEngine.Random.Range(-1f, 1f) * _randomOffsetVar.x,
                            UnityEngine.Random.Range(-1f, 1f) * _randomOffsetVar.y,
                            0);

            SynchronizePosition();

            _numberToDisplay.text = number.ToString();

            gameObject.SetActive(true);
            _onShowEvent.Invoke();
        }
    }
}