using System;
using System.Linq;
using UnityEngine;

namespace MechSquad.Battle
{
	public class TestPlayerRevivePositionPolicy : MonoBehaviour, IPlayerRevivePositionPolicy
	{
		[SerializeField]
		Transform _originSpawnPositionA;
		[SerializeField]
		Transform _originSpawnPositionB;

		[SerializeField]
		float _randomDistanceMin = 30;
		[SerializeField]
		float _randomDistanceMax = 60;

		public Vector3 GetRevivePosition(BattleUnit unit)
		{
			var sameTeamUnits = UnitManager.Instance.GetSameTeamUnits(unit.Team).ToList();
			sameTeamUnits.Remove(unit);

			Vector3 centerPosition;
			if (sameTeamUnits.Count == 0)
			{
				centerPosition = unit.Team == Team.A ? _originSpawnPositionA.position : _originSpawnPositionB.position;
			}
			else
			{
				centerPosition = sameTeamUnits.RandomGet().Model.position;
			}
			centerPosition.y = 0;

			var randomAngle = UnityEngine.Random.Range(0, 360f);
			var positionVar = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward
				* UnityEngine.Random.Range(_randomDistanceMin, _randomDistanceMax);
			var position = centerPosition + positionVar;
			var rotation = Quaternion.Euler(0, -randomAngle, 0);

			position = MapEdge.GetCorrectedPosition(position);

			return position;
		}
	}
}