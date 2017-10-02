using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MechSquad
{
	public class BattleAllPlayers
	{
		List<BattlePlayerInfo> _players = new List<BattlePlayerInfo>();

		public BattlePlayerInfo GetByUid(string uid)
		{
			for (var i = 0; i < _players.Count; i++)
			{
				if (_players[i].Uid == uid)
					return _players[i];
			}

			throw new UnityException(string.Format("Can not find battle player by uid {0}", uid));
		}
	}
	
	public class BattlePlayerInfo
	{
		bool _isRobot;
		public bool IsRobot { set { _isRobot = value; } get { return _isRobot; } }

		string _uid;
		public string Uid { set { _uid = value; } get { return _uid; } }

		byte _team;
		public byte Team
		{
			set { _team = value; }
			get
			{
				if (_isRobot)
					return _team;
				else
				{
					var p = PhotonHelper.GetPlayerByUserID(_uid);
					return p.GetUnitTeam();
				}
			}
		}

		public PhotonPlayer GetPhotonPlayer()
		{
			return PhotonHelper.GetPlayerByUserID(_uid);
		}
	}
}
