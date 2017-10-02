using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Haruna.Pool;

namespace MechSquad.Battle
{
	public interface IBullet
	{
		WeaponForceLevelEnum WeaponForceLevel { get; }
		void Activate();
		void DestroyBullet();
	}

	public enum WeaponForceLevelEnum
	{
		Light,
		Heavy,
	}

	public enum PenetrationLevelEnum
	{
		Weak,
		Medium,
		Strong,
	}

}