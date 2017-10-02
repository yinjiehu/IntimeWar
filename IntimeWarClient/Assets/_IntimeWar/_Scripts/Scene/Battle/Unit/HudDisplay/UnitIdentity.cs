using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;
using UnityEngine.UI;

namespace MechSquad.Battle
{
	public class UnitIdentity : Ability
	{
		[SerializeField]
		GameObject _selfCircle;
		[SerializeField]
		GameObject _allyCircle;
		[SerializeField]
		GameObject _enemyCircle;

		[SerializeField]
		GameObject _moveDirection;

		[SerializeField]
		HudInstance _nickNameDisplay;
		HudInstance _nickNameHudInstance;

		[SerializeField]
		Color _colorSelf;
		[SerializeField]
		Color _colorAlly;
		[SerializeField]
		Color _colorEnemy;

		float _elapsedTime;
		float _updateInterval = 0.1f;

		string _nickName;
		public string NickName { get { return _nickName; } }

		public override void LateInit()
		{
			base.LateInit();

			_nickName = "Offline";

			if (!PhotonNetwork.offlineMode)
			{
				if (_unit.View.isSceneView)
				{
					_nickName = "未知势力";
				}
				else
				{
					var p = PhotonHelper.GetPlayer(_unit.View.CreatorActorNr);
					_nickName = p.NickName;
				}
			}

			_nickNameHudInstance = ViewManager.Instance.GetView<HudView>().CreateFromPrefab(_nickNameDisplay);
			_nickNameHudInstance.WorldCamera = Camera.main;
			_nickNameHudInstance.UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
			_nickNameHudInstance.FollowTransform = _unit.GetAbility<UnitPositionDefine>().Top;
			_nickNameHudInstance.SynchronizePosition();
			if (_unit.IsPlayerForThisClient)
				_nickNameHudInstance.GetComponent<Text>().color = _colorSelf;
			else if (_unit.Team == PhotonNetwork.player.GetUnitTeam())
				_nickNameHudInstance.GetComponent<Text>().color = _colorAlly;
			else
				_nickNameHudInstance.GetComponent<Text>().color = _colorEnemy;
			_nickNameHudInstance.GetComponent<Text>().text = _nickName;

			OnBodyVisibleChange(_unit.STS.BodyVisible.GetValue());

            _unit.STS.BodyVisible.EvOnValueChange += OnBodyVisibleChange;
		}

		private void OnBodyVisibleChange(bool visible)
		{
			if (visible)
			{
				if (_unit.IsPlayerForThisClient)
				{
					_selfCircle.SetActive(true);
                    _moveDirection.SetActive(true);
                    _allyCircle.SetActive(false);
					_enemyCircle.SetActive(false);
				}
				else
				{
					var sameTeam = PhotonNetwork.player.GetUnitTeam() == _unit.Team;
					_selfCircle.SetActive(false);
                    _moveDirection.SetActive(false);
                    _allyCircle.SetActive(sameTeam);
					_enemyCircle.SetActive(!sameTeam);
                }
                _nickNameHudInstance.gameObject.SetActive(true);
				_nickNameHudInstance.SynchronizePosition();
			}
			else
			{
				_selfCircle.SetActive(false);
				_allyCircle.SetActive(false);
				_enemyCircle.SetActive(false);
				_moveDirection.SetActive(false);

				_nickNameHudInstance.gameObject.SetActive(false);
			}
		}
		
		public override void BeforeDestroy()
		{
			if (_nickNameHudInstance != null)
			{
				_nickNameHudInstance.RecycleInstance();
				_nickNameHudInstance.gameObject.SetActive(false);
			}
		}
	}
}