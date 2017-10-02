using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Haruna.Pool;

namespace MechSquad.Battle
{
	[Serializable]
	public struct HitFx
	{
		public FxHandler HitDirt;
		public FxHandler HitWater;
		public FxHandler HitMetal;
		public FxHandler HitStone;

		public FxHandler GetFX(BodyMaterialTypeEnum bodyMaterialType)
		{
			switch (bodyMaterialType)
			{
				case BodyMaterialTypeEnum.Metal:
					return HitMetal;
				case BodyMaterialTypeEnum.Stone:
					return HitStone;
				case BodyMaterialTypeEnum.Dirt:
					return HitDirt;
				case BodyMaterialTypeEnum.Water:
					return HitWater;
			}
			return null;
		}
	}
	[Serializable]
	public struct HitSE
	{
		public SePlayer HitDirt;
		public SePlayer HitWater;
		public SePlayer HitMetal;
		public SePlayer HitStone;

		public SePlayer GetSE(BodyMaterialTypeEnum bodyMaterialType)
		{
			switch (bodyMaterialType)
			{
				case BodyMaterialTypeEnum.Metal:
					return HitMetal;
				case BodyMaterialTypeEnum.Stone:
					return HitStone;
				case BodyMaterialTypeEnum.Dirt:
					return HitDirt;
				case BodyMaterialTypeEnum.Water:
					return HitWater;
			}
			return null;
		}
	}

	public abstract class CollisionBullet : CollisionTiggerProcessor, IBullet
	{
		[SerializeField]
		WeaponForceLevelEnum _weaponForceLevel;
		public WeaponForceLevelEnum WeaponForceLevel { get { return _weaponForceLevel; } }

		[SerializeField]
		PenetrationLevelEnum _penetrationLevel;
		public PenetrationLevelEnum PenetrationLevel { get { return _penetrationLevel; } }
		
		[SerializeField]
		protected HitFx _hitFx;
		[SerializeField]
		protected HitSE _hitSe;
		
		public virtual void Activate()
		{
			_unit.gameObject.SetActive(true);
		}

		public virtual void DestroyBullet()
		{
			var poolElement = _unit.GetComponent<PoolElement>();
			if (poolElement != null)
			{
				_unit.gameObject.SetActive(false);
				poolElement.RecycleThis();
			}
			else
			{
				_unit.OnUnitDestroy();
				Destroy(_unit.gameObject);
			}
		}
	}
}