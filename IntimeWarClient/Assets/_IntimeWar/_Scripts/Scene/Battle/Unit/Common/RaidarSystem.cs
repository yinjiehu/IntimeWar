using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class RaidarSystem : Ability
	{
		[SerializeField]
		float _range = 70;
		[SerializeField]
		bool _showRadiusOnGizmo;

		float _updateInterval = 0.2f;
		float _elapsedTime;
		
		Dictionary<int, BattleUnit> _enemyInfo = new Dictionary<int, BattleUnit>();
		Dictionary<int, BattleUnit> _sharedEnemyInfo = new Dictionary<int, BattleUnit>();
		
		[SerializeField]
		bool _raidarEqualSight;
		SquadSight _squadSight;

		public override void Init()
		{
			base.Init();
			
			_squadSight = _unit.GetAbility<SquadSight>();
			_range = _unit.InitialParameter.GetParameter(ConstParameter.Sight);
			//if (_raidarEqualSight)
			//	_squadSight.GetComponent<FoW.FogOfWarUnit>().radius = _range;

			_unit.STS.SightPercentModify.EvOnValueChange += (value) =>
			{
				//if (_raidarEqualSight)
				//	_squadSight.GetComponent<FoW.FogOfWarUnit>().radius = _range * (1 + value);
			};
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			_elapsedTime += Time.deltaTime;
			if (_elapsedTime > _updateInterval)
			{
				_elapsedTime = 0;
				if(_unit.Team == PhotonNetwork.player.GetUnitTeam())
					UpdateRaidarSpotting();

				if (_unit.IsPlayerForThisClient)
					UpdateSharedInfo();
			}
		}

		void UpdateRaidarSpotting()
		{
			_enemyInfo.Clear();

			var teamedUnits = UnitManager.Instance.TeamedUnits;
			using (var itr = teamedUnits.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					if (!itr.Current.STS.DisplayInRaidar)
						continue;

					if (itr.Current.Team != _unit.Team)
					{
						if (_raidarEqualSight)
						{
							var targetFogSight = itr.Current.GetAbility<InvisibleInFog>();
							if (targetFogSight == null || !targetFogSight.InvisibleInEnemyRaidar)
							{
								_enemyInfo.Add(itr.Current.SeqNo, itr.Current);
							}
						}
						else
						{
							var dis = Vector3.Distance(_unit.Model.position, itr.Current.Model.position);
							if (dis < _range)
							{
								_enemyInfo.Add(itr.Current.SeqNo, itr.Current);
							}
						}
					}
				}
			}
		}

		void UpdateSharedInfo()
		{
			_sharedEnemyInfo.Clear();

			var units = UnitManager.Instance.TeamedUnits;
			foreach (var u in units)
			{
				if (u.STS.ProvideSquadSight && u.Team == _unit.Team)
				{
					var raidar = u.GetAbility<RaidarSystem>();
					var enemys = raidar.GetEnemyTargets();

					foreach (var e in enemys)
					{
						if (!_sharedEnemyInfo.ContainsKey(e.Key))
							_sharedEnemyInfo.Add(e.Key, e.Value);
					}
				}
			}
		}

		public Dictionary<int, BattleUnit> GetEnemyTargets()
		{
			return _enemyInfo;
		}

		public Dictionary<int, BattleUnit> GetSharedTargets()
		{
			return _sharedEnemyInfo;
		}
		
		private void OnDrawGizmosSelected()
		{
			if (_showRadiusOnGizmo)
			{
				Gizmos.color = new Color(1, 0, 0, 0.2f);
				Gizmos.DrawSphere(transform.position, _range);
			}
		}
	}
}