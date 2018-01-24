using System;
using System.Linq;
using UnityEngine;

namespace YJH.Unit
{
	public class TestPlayerRevivePositionPolicy : MonoBehaviour, IPlayerRevivePositionPolicy
	{
		[SerializeField]
		Transform _originSpawnPositionA;
		[SerializeField]
		Transform _originSpawnPositionB;

		public Vector3 GetRevivePosition(BattleUnit unit)
		{
			return unit.Team == Team.A ? _originSpawnPositionA.position : _originSpawnPositionB.position;
        }
	}
}