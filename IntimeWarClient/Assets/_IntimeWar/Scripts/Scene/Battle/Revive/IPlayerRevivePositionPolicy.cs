using System;
using UnityEngine;

namespace MechSquad.Battle
{
	public interface IPlayerRevivePositionPolicy
	{
		Vector3 GetRevivePosition(BattleUnit unit);
	}
}