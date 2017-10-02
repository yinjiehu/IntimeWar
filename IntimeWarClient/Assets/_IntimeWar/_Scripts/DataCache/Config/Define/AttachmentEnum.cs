using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad
{
	public enum AttachmentControlTypeEnum
	{
		/// <summary>
		/// 连射
		/// </summary>
		Rapid,
		/// <summary>
		/// 单发
		/// </summary>
		Single,
		/// <summary>
		/// 抛射
		/// </summary>
		Grenade,
		/// <summary>
		/// 火炮
		/// </summary>
		Artillery,
		/// <summary>
		/// 制导
		/// </summary>
		Missile,
		/// <summary>
		/// 被动
		/// </summary>
		Passive,
	}

	public enum AttachmentPurposeTypeEnum
	{
		/// <summary>
		/// 能量
		/// </summary>
		Energy,
		/// <summary>
		/// 实弹
		/// </summary>
		Ballistic,
		/// <summary>
		/// 导弹
		/// </summary>
		Missile,
		/// <summary>
		/// 医疗
		/// </summary>
		Medic,
		/// <summary>
		/// 运动
		/// </summary>
		Motion,
		/// <summary>
		/// 辅助
		/// </summary>
		Support,
		/// <summary>
		/// 补给
		/// </summary>
		Supply,
	}

	public enum AttachmentCategoryTypeEnum
	{
		/// <summary>
		/// Auto Canon
		/// </summary>
		AutoCanon,
		/// <summary>
		/// Machine Gun.
		/// </summary>
		MachineGun,
		/// <summary>
		/// Shotgun.
		/// </summary>
		Shotgun,
		/// <summary>
		/// Shot Canon.
		/// </summary>
		ShotCanon,
		/// <summary>
		/// Canon.
		/// </summary>
		Canon,
		/// <summary>
		/// Railgun
		/// </summary>
		Railgun,

		/// <summary>
		/// Laser Canon
		/// </summary>
		Laser,
		/// <summary>
		/// Plasm Canon
		/// </summary>
		Plasma,

		/// <summary>
		/// Flame Thrower
		/// </summary>
		FlameThrower,

		/// <summary>
		/// Mortar.
		/// </summary>
		Mortar,

		/// <summary>
		/// Artillery.
		/// </summary>
		Artillery,

		/// <summary>
		/// Short ranged missile launcher.
		/// </summary>
		ShortRangedMissle,
		/// <summary>
		/// Medium ranged missile launcher
		/// </summary>
		MediumRangedMissle,

		/// <summary>
		/// Melee
		/// </summary>
		Melee,

		/// <summary>
		/// Support.
		/// </summary>
		Support,
	}

	public enum PassiveAttachmentTypeEnum
	{
		Ammo,
		ParameterUp,
	}
}
