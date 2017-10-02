using UnityEngine;
using System;
using Haruna.Inspector;

namespace MechSquad.Battle
{
	public static class Team
	{
		public const byte None = 0;
		public const byte A = 1;
		public const byte B = 2;
		public const byte C = 3;
	}
	
	[Serializable]
	public class UnitInfo
	{
		UnitInfo _spawnFrom;
		public UnitInfo SpawnFrom { set { _spawnFrom = value; } get { return _spawnFrom; } }
		public bool IsSpawnFromUnit { get { return _spawnFrom != null; } }

		[HarunaInspect(HideInRunningMode = true)]
		[SerializeField]
		string _typeID;
		public string TypeID { set { _typeID = value; } get { return _typeID; } }

		[HarunaInspect(HideInEditorMode = true)]
		[SerializeField]
		int _seqNo;
		public int SeqNo { set { _seqNo = value; } get { return _seqNo; } }

		[HarunaInspect(HideInEditorMode = true)]
		[SerializeField]
		int _actorID;
		public int ActorID { set { _seqNo = value; } get { return _seqNo; } }

		[SerializeField]
		string _tag;
		public string Tag { set { _tag = value; } get { return _tag; } }

		[SerializeField]
		byte _team;
		public byte Team { set { _team = value; } get { return _team; } }

		[SerializeField]
		float _level;
		public float Level { set { _level = value; } get { return _level; } }

		BattleUnit _unit;
		public BattleUnit Unit { set { _unit = value; } get { return _unit; } }
	}	
}