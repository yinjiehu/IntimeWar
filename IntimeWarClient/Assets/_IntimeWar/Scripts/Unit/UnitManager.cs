using UnityEngine;
using System;
using Haruna.Inspector;
using System.Collections.Generic;
using System.Linq;

namespace YJH.Unit
{
	public class UnitManager : MonoBehaviour
	{
		static UnitManager _instance;

		public static UnitManager Instance
		{
			get
			{
				if (_instance == null)
				{
					var go = new GameObject("UnitManager");
					_instance = go.AddComponent<UnitManager>();
				}

				return _instance;
			}
		}

		HashSet<BattleUnit> _allUnitList = new HashSet<BattleUnit>();
		public HashSet<BattleUnit> AllUnits { get { return _allUnitList; } }
		HashSet<BattleUnit> _unitsInAllTeam = new HashSet<BattleUnit>();
		public HashSet<BattleUnit> TeamedUnits { get { return _unitsInAllTeam; } }
		HashSet<BattleUnit> _unitsInTeamNone = new HashSet<BattleUnit>();
		public HashSet<BattleUnit> TeamNoneUnits { get { return _unitsInTeamNone; } }

		List<BattleUnit> _playerUnits = new List<BattleUnit>();
		public List<BattleUnit> AllPlayerUnit { get { return _playerUnits; } }

		BattleUnit _thisClientPlayerUnit;
		public static BattleUnit ThisClientPlayerUnit { get { return Instance._thisClientPlayerUnit; } }
		
		public void SetThisClientPlayerUnit(BattleUnit unit)
		{
			_thisClientPlayerUnit = unit;
		}

		public IEnumerable<BattleUnit> GetSameTeamUnits(byte team)
		{
			return _unitsInAllTeam.Where(u => u.Team == team);
		}
		
		public void AddPlayerUnit(BattleUnit unit)
		{
			AddUnit(unit);
			_playerUnits.Add(unit);
		}
		public BattleUnit GetPlayerUnitByActorID(int actorID)
		{
			for(var i = 0; i < _playerUnits.Count; i++)
			{
				if (_playerUnits[i].ActorID == actorID)
					return _playerUnits[i];
			}
			return null;
		}

        public BattleUnit GetNearestPlayerUnit(int seqNo, byte team, Vector3 position, float range = float.MaxValue)
        {
            BattleUnit unit = null;
            float distance = range;
            foreach (var p in _playerUnits)
            {
                if (p.IsDead)
                    continue;
                var temp = Vector3.Distance(p.Model.position, position);
                if (temp < distance && p.Info.SeqNo != seqNo && p.Team == team)
                {
                    unit = p;
                    distance = temp;
                }
            }
            return unit;
        }

        public BattleUnit GetNearestUnit(int seqNo, byte team, Vector3 position, float range = float.MaxValue)
        {
            BattleUnit unit = null;
            float distance = range;
            foreach (var p in _allUnitList)
            {
                if (p.IsDead)
                    continue;
                var temp = Vector3.Distance(p.Model.position, position);
                if (temp < distance && p.Info.SeqNo != seqNo && p.Team == team)
                {
                    unit = p;
                    distance = temp;
                }
            }
            return unit;
        }

        public void AddUnit(BattleUnit unit)
		{
			if(unit.NoSequenceUnit)
			{
				Debug.LogErrorFormat(unit, "Unit is not sequence unit. should not add to unitmanager");
			}

			_allUnitList.Add(unit);
			if (unit.Team == Team.None)
				_unitsInTeamNone.Add(unit);
			else
				_unitsInAllTeam.Add(unit);

		}

		public void DestroyUnitItsSelf(BattleUnit unit, bool destroyGameObject = true)
		{
			DestroyUnit(unit, destroyGameObject);
		}

		public void DestroyUnitByAttack(BattleUnit unit, bool destroyGameObject = true)
		{
			DestroyUnit(unit, destroyGameObject);
		}

		void DestroyUnit(BattleUnit unit, bool destroyGameObject)
		{
			_allUnitList.Remove(unit);
			if (unit.Team == Team.None)
				_unitsInTeamNone.Remove(unit);
			else
				_unitsInAllTeam.Remove(unit);

			if (destroyGameObject)
			{
				unit.OnUnitDestroy();
				Destroy(unit.gameObject);
			}
		}
	}
}