using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace YJH.Unit
{
	public interface IUnitAbility
	{
        string AbilityID { get; }

        void SetupInstance(BattleUnit u);

        void Init();
        void LateInit();

        void OnInitSynchronization(object data);
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