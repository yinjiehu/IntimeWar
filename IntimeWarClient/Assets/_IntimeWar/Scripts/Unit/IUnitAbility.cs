using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MechSquad.Battle
{
	public interface IUnitAbility
	{
		string Name { get; }
		bool IsSyncAbility { get; }

		void SetupInstance(BattleUnit u);

		void Init();

		void LateInit();
	}

	public interface IUnitAbilityUpdate
	{
		void OnUpdate();
	}

	public interface IUnitAbilityDestroy
	{
		void BeforeDestroy();
	}
}