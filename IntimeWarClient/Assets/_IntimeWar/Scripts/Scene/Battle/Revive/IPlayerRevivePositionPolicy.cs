using System;
using UnityEngine;

namespace YJH.Unit
{
	public interface IPlayerRevivePositionPolicy
	{
		Vector3 GetRevivePosition(BattleUnit unit);
	}
}