using Haruna.Utility;
using IntimeWar.Battle;
using System;
using UnityEngine;
using UnityEngine.UI;
using WhiteCat.Tween;
using YJH.Unit;

namespace IntimeWar.View
{
    public class HudHp : MonoBehaviour
    {
        [SerializeField]
        bool _alwaysDipslay;

        public enum StateEnum
        {
            Shown,
            Hiding,
            Hided,
        }
        StateEnum _state = StateEnum.Hided;

        [SerializeField]
        float _autoHideSec;
        float _elapsedTime;

        HudInstance _hudInstance;
        BattleUnit _observerUnit;
        UnitBody _body;


        [SerializeField]
        Image _bodyHpBack;
        [SerializeField]
        Image _bodyHpFront;

        [SerializeField]
        Color _colorSelf;
        [SerializeField]
        Color _colorAlly;
        [SerializeField]
        Color _colorEnemy;

        float _previousBodyHp;
        float _targetBodyHpAmount;

        [SerializeField]
        float _frontDownSpeed;
        [SerializeField]
        float _backDownSpeed;
        [SerializeField]
        float _upSpeed;

        [SerializeField]
        CanvasGroup _canvasGroup;
        [SerializeField]
        Tweener _hideTweener;

        void Awake()
        {
            _hudInstance = GetComponent<HudInstance>();

            if (_alwaysDipslay)
                _canvasGroup.alpha = 1;
            else
                _canvasGroup.alpha = 0;
        }

        public void SetUnitAndStartFollow(BattleUnit unit, Transform follow)
        {
            _observerUnit = unit;
            _body = unit.Body;

            _previousBodyHp = _body.CurrentHp;
            _targetBodyHpAmount = _previousBodyHp / _body.MaxHp;


            if (unit.IsPlayerForThisClient)
            {
                _bodyHpFront.color = _colorSelf;
            }
            else if (unit.Team == PhotonNetwork.player.GetUnitTeam())
            {
                _bodyHpFront.color = _colorAlly;
            }
            else
            {
                _bodyHpFront.color = _colorEnemy;
            }

            _hudInstance.WorldCamera = Camera.main;
            _hudInstance.UICamera = Util.GetUICamera();

            _hudInstance.FollowTransform = follow;
            _hudInstance.SynchronizePosition();

            if (_alwaysDipslay)
                _canvasGroup.alpha = 1;
            else
                _canvasGroup.alpha = 0;
        }

        private void Update()
        {
            UpdateBodyHp();
        }

        void UpdateBodyHp()
        {
            var currentHp = _body.CurrentHp;
            if (_previousBodyHp != currentHp)
            {
                var maxHp = _body.MaxHp;
                _previousBodyHp = currentHp;
                _targetBodyHpAmount = currentHp / maxHp;
                //Show();
            }

            UpdateHpFillAmount(_bodyHpFront, _bodyHpBack, _targetBodyHpAmount);
        }

        void UpdateHpFillAmount(Image front, Image back, float targetAmount)
        {
            {
                var currentAmountFront = front.fillAmount;
                if (currentAmountFront != targetAmount)
                {
                    var delta = targetAmount - currentAmountFront;
                    var speed = delta > 0 ? _upSpeed : _frontDownSpeed * -1;

                    front.fillAmount = Mathf.Lerp(currentAmountFront, targetAmount, Time.deltaTime / (delta / speed));
                }
            }

            {
                var currentAmountFront = front.fillAmount;
                var currentAmountBack = back.fillAmount;
                if (currentAmountBack != currentAmountFront)
                {
                    var delta = currentAmountFront - currentAmountBack;
                    if (delta < 0)
                    {
                        var speed = _backDownSpeed * -1;
                        back.fillAmount = Mathf.Lerp(currentAmountBack, currentAmountFront, Time.deltaTime / (delta / speed));
                    }
                    else
                    {
                        back.fillAmount = front.fillAmount;
                    }
                }
            }
        }

        void UpdateAutoHide()
        {
            if (!_alwaysDipslay && _state == StateEnum.Shown)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime > _autoHideSec)
                {
                    _state = StateEnum.Hiding;
                    _hideTweener.PlayForward();
                }
            }
        }


        public void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1 : 0;
        }
    }
}