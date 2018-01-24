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
		/// 弹道
		/// </summary>
		Ballistic,
		/// <summary>
		/// 制导
		/// </summary>
		Missile,
		/// <summary>
		/// 维修
		/// </summary>
		Medic,
		/// <summary>
		/// 侦查
		/// </summary>
		Detect,
		/// <summary>
		/// 干扰
		/// </summary>
		Disturb,
		/// <summary>
		/// 机动
		/// </summary>
		Motion,
		/// <summary>
		/// 防御
		/// </summary>
		Defense,
		/// <summary>
		/// 隐匿
		/// </summary>
		Hiding
	}

	public enum AttachmentCategoryTypeEnum
	{
		/// <summary>
		/// 自动加农.
		/// </summary>
		AutoCanon,
		/// <summary>
		/// 多管机炮.
		/// </summary>
		MachineGun,
		/// <summary>
		/// 霰弹枪.
		/// </summary>
		Shotgun,
		/// <summary>
		/// 霰弹炮.
		/// </summary>
		ShotCanon,
		/// <summary>
		/// 加农炮.
		/// </summary>
		Canon,
		/// <summary>
		/// 电磁炮
		/// </summary>
		Railgun,

		/// <summary>
		/// 镭射.
		/// </summary>
		Laser,
		/// <summary>
		/// 等离子炮.
		/// </summary>
		Plasma,
		/// <summary>
		/// 喷火.
		/// </summary>
		FlameThrower,

		/// <summary>
		/// 迫击炮.
		/// </summary>
		Mortar,
		/// <summary>
		/// 火炮.
		/// </summary>
		Artillery,
		/// <summary>
		/// 短程飞弹.
		/// </summary>
		ShortRangedMissle,
		/// <summary>
		/// 中程飞弹
		/// </summary>
		MediumRangedMissle,
		/// <summary>
		/// 近战
		/// </summary>
		Melee,
        /// <summary>
        /// 基础维修
        /// </summary>
        FastMedic,
        /// <summary>
        /// 定位壁垒
        /// </summary>
        PieceStaticSheild,
        /// <summary>
        /// 光学隐形
        /// </summary>
        OpticalHiding,
        /// <summary>
        /// 侦查标识
        /// </summary>
        StaticScout,
        /// <summary>
        /// Support.
        /// </summary>
        Support,
	}

	public enum PassiveAttachmentTypeEnum
	{
		Ammo,
		ParameterUp,
        Special,
	}
}
