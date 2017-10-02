using MechSquad.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Haruna.Pool;
using WhiteCat.Tween;

namespace MechSquad.View
{
	public class RaidarElement : MonoBehaviour
	{
		public BattleUnit Unit { set; get; }

		bool _isAlly;

		[SerializeField]
		Image _iconAlly;
		[SerializeField]
		Image _iconAllyEdge;

		[SerializeField]
		Image _iconEnemy;
		[SerializeField]
		Image _iconEnemyEdge;
		[SerializeField]
		Image _locker;
		[SerializeField]
		Tweener _lockerTween;

		public void SetAsAlly()
		{
			_isAlly = true;

			_iconEnemy.enabled = false;
			_iconEnemyEdge.enabled = false;
			_locker.enabled = false;
		}
		public void SetAsEnemy()
		{
			_isAlly = false;

			_iconAlly.enabled = false;
			_iconAllyEdge.enabled = false;
			_locker.enabled = false;
		}

		public void SetIn()
		{
			if (_isAlly)
			{
				_iconAlly.enabled = true;
				_iconAllyEdge.enabled = false;
			}
			else
			{
				_iconEnemy.enabled = true;
				_iconEnemyEdge.enabled = false;
			}
		}
		public void SetOut()
		{
			if (_isAlly)
			{
				_iconAlly.enabled = false;
				_iconAllyEdge.enabled = true;
			}
			else
			{
				_iconEnemy.enabled = false;
				_iconEnemyEdge.enabled = true;
			}
		}
		public void Hide()
		{
			_iconEnemy.enabled = false;
			_iconEnemyEdge.enabled = false;
			_locker.enabled = false;
		}
		
		public void SetLock(bool on)
		{
			if (on)
			{
				if (!_locker.enabled)
				{
					_locker.enabled = true;
					//_lockerTween.PlayForward();
				}
			}
			else
			{
				_locker.enabled = false;
			}
		}
	}
}