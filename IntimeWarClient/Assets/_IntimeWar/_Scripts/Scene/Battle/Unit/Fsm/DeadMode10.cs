using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MechSquad.Battle
{
	public class DeadMode10 : FsmStateAbility
	{
		BattleUnit _followUnit;

		bool _enable;
		float _interval = 2f;
		float _elapsedTime;

		public override void OnEnter()
		{
			base.OnEnter();

			if (PhotonHelper.IsModeInTeamDeathMatach(PhotonNetwork.room))
			{
				if (_unit.IsPlayerForThisClient)
				{
					_enable = true;
				}
			}
			else
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_enable)
			{
				_elapsedTime += Time.deltaTime;
				if(_elapsedTime > _interval)
				{
					_elapsedTime = 0;

					if (_followUnit == null || _followUnit.IsDead)
					{
						DecideFollowUnit();
					}
				}
			}
		}

		void DecideFollowUnit()
		{
			var allies = UnitManager.Instance.GetSameTeamUnits(_unit.Team).ToList();

			if (allies.Count() != 0)
				_followUnit = allies.FirstOrDefault(u => !u.IsDead);

			if (_followUnit != null)
				Camera.main.GetComponent<Deftly.DeftlyCamera>().ResetTarget(new GameObject[] { _followUnit.Model.gameObject });
		}
	}
}