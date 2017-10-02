using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace  MechSquad.Battle
{
	public enum AimingModeEnum
	{
		Manual,
		/// <summary>
		/// semi-auto means will aiming with support system
		/// </summary>
		SemiAuto,
		Auto
	}
	public interface IMainFireControlSystem
	{
		void SetAimingDirection(Vector3 direction);

		AimingModeEnum CurrentAimingMode { get; }
		void SwitchAimingMode(AimingModeEnum mode);

		BattleUnit SwitchTarget();
		Vector3 GetAimingDirection();
		BattleUnit GetAimingTarget();

		Transform TurretRoot { get; }
		Vector3 CurrentTurretDirection { get; }
		void SetAsCameraFollow();
	}
}