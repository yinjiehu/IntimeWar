using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad
{
	public static class TextForAttachment
	{
		public const string AttNameFullPrefix = "AttachmentNameFull_";
		public const string AttNameSimplePrefix = "AttachmentNameSimple_";
		public const string AttIntroduceSimplePrefix = "AttachmentIntroduceSimpleFull_";
		public const string AttIntroduceFullPrefix = "AttachmentIntroduceFull_";

		public const string AttPurposePrefix = "AttachmentPurpose_";
		public const string AttCategoryPrefix = "AttachmentCategory_";

		public const string Parameter_PowerToUnit = "AttachmentParameter_PowerToUnit";
		public const string Parameter_PowerToEnv = "AttachmentParameter_PowerToEnv";
		public const string Parameter_MaxRange = "AttachmentParameter_MaxRange";
		public const string Parameter_ReloadSeconds = "AttachmentParameter_ReloadSeconds";
		public const string Parameter_CartridgeCapacity = "AttachmentParameter_CartridgeCapacity";
		public const string Parameter_ShootRate = "AttachmentParameter_ShootRate";
		public const string Parameter_MinRange = "AttachmentParameter_MinRange";
		public const string Parameter_WarmingUp = "AttachmentParameter_WarmingUp";
		public const string Parameter_ExplosiveRadius = "AttachmentParameter_ExplosiveRadius";
		public const string Parameter_ArmorPenetration = "AttachmentParameter_ArmorPenetration";
		public const string Parameter_Accuracy = "AttachmentParameter_Accuracy";

		public static string GetNameFull(string attachmentID)
		{
			return Texts.Get(AttNameFullPrefix + attachmentID);
		}
		public static string GetNameSimple(string attachmentID)
		{
			return Texts.Get(AttNameSimplePrefix + attachmentID);
		}
		public static string GetIntroduceSimple(string attachmentID)
		{
			return Texts.Get(AttIntroduceSimplePrefix + attachmentID);
		}
		public static string GetIntroduceFull(string attachmentID)
		{
			return Texts.Get(AttIntroduceFullPrefix + attachmentID);
		}
		public static string GetPurposeDescription(AttachmentPurposeTypeEnum purposeType)
		{
			return Texts.Get(AttPurposePrefix + purposeType.ToString());
		}
		public static string GetPurposeDescription(string purposeTypeStr)
		{
			return Texts.Get(AttPurposePrefix + purposeTypeStr);
		}
		public static string GetCategoryDesciption(string categoryStr)
		{
			return Texts.Get(AttCategoryPrefix + categoryStr);
		}

		public static void DemoCreate()
		{
			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_AC02_R2_M270, "M270 “解放者”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_AC02_R2_M270, "M270");
			Texts.Add(AttIntroduceFullPrefix + ActiveAttachmentID.AT_AC02_R2_M270, "由史密斯重工开发的M270是在M249基础上发展起来的。为了让小型PT也能使用，牺牲了射速而大大提升了稳定性，同时载弹量也足够进行10秒持续射击，单发子弹的动能达到了346每平方焦耳。小型化的结果是一部分士兵甚至徒手持用，称自己再现了黄金时代的某电影明星，但这是说明书所严格禁止的事情。相当的士兵因此肩膀被震碎甚至肋骨刺入肺部。但这足以体现士兵们对这款武器的喜爱。");

			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_SG01_S2_SPAS28, "SPAS-28 “撕裂者”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_SG01_S2_SPAS28, "SPAS-28");

			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_MG01_R3_RK97, "RK-97 “耳语者”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_MG01_R3_RK97, "RK97");

			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_CN01_S3_CP105, "CP-105 “麻雀”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_CN01_S3_CP105, "CP105");

			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_CN02_S3_CH105, "CH-105 “乌鸦”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_CN02_S3_CH105, "CH105");

			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_CN30_S4_CP185, "CP-185 “猫头鹰”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_CN30_S4_CP185, "CP185");
			
			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_GRL01_G2_SS39, "SS-39 “巧克力”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_GRL01_G2_SS39, "SS-39");

			Texts.Add(AttNameFullPrefix + ActiveAttachmentID.AT_GRL02_G3_SS51, "SS-51 “棉花糖”");
			Texts.Add(AttNameSimplePrefix + ActiveAttachmentID.AT_GRL02_G3_SS51, "SS-51");


			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Shell_20, "20mm通常弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Shell_20, "20mm");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Shell_20, "");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Shell_45, "45mm通常弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Shell_45, "45mm");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Shell_45, "");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Shell_105AP, "105mm穿甲弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Shell_105AP, "105mm AP");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Shell_105AP, "");
			
			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Shell_105HEAT, "105mm破甲弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Shell_105HEAT, "105mm HEAT");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Shell_105HEAT, "金属射流");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Shell_185AP, "185mm穿甲弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Shell_185AP, "185mm AP");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Shell_185AP, "尾翼稳定脱壳穿甲弹");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Slug_28, "28ga霰弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Slug_28, "28ga");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Slug_28, "");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Grenade_67, "67mm榴弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Grenade_67, "67mmHE");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Grenade_67, "");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Ammo_Grenade_100, "100mm榴弹");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Ammo_Grenade_100, "100mmHE");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Ammo_Grenade_100, "");

			//
			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Up_Armor, "EX01 I型装甲片");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Up_Armor, "EX01");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Up_Armor, "装甲强度 +10%");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Up_MaxSpeed, "C08 功率放大器");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Up_MaxSpeed, "C08");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Up_MaxSpeed, "移动速度 +3%");

			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Up_TurnSpeed, "TB29 辅助瞄准系统");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Up_TurnSpeed, "TB29");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Up_TurnSpeed, "炮塔转速 +5%");
			
			Texts.Add(AttNameFullPrefix + PassiveAttachmentID.Up_Sight, "S63 雷达增强系统");
			Texts.Add(AttNameSimplePrefix + PassiveAttachmentID.Up_Sight, "S63");
			Texts.Add(AttIntroduceSimplePrefix + PassiveAttachmentID.Up_Sight, "视野范围 +3");

			//attachment purpose type
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Ballistic.ToString(), "实弹");
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Energy.ToString(), "能量");
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Missile.ToString(), "制导");
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Motion.ToString(), "运动");
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Medic.ToString(), "医疗");
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Support.ToString(), "辅助");
			Texts.Add(AttPurposePrefix + AttachmentPurposeTypeEnum.Supply.ToString(), "补给");

			//attachment category type
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.AutoCanon.ToString(), "自动加农");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.MachineGun.ToString(), "火神炮");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Shotgun.ToString(), "霰弹枪");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.ShotCanon.ToString(), "霰弹炮");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Canon.ToString(), "加农炮");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Railgun.ToString(), "电磁炮");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Laser.ToString(), "镭射加农");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Plasma.ToString(), "离子加农");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.FlameThrower.ToString(), "火焰喷射器");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Mortar.ToString(), "迫击炮");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Artillery.ToString(), "火炮");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.ShortRangedMissle.ToString(), "短程导弹");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.MediumRangedMissle.ToString(), "中程导弹");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Melee.ToString(), "格斗");
			Texts.Add(AttCategoryPrefix + AttachmentCategoryTypeEnum.Support.ToString(), "辅助");


			Texts.Add(Parameter_PowerToUnit, "单位伤害");
			Texts.Add(Parameter_PowerToEnv, "环境伤害");
			Texts.Add(Parameter_MaxRange, "最大射程");
			Texts.Add(Parameter_ReloadSeconds, "装填时间");
			Texts.Add(Parameter_CartridgeCapacity, "弹夹容量");
			Texts.Add(Parameter_ShootRate, "连射速率");
			Texts.Add(Parameter_MinRange, "最小射程");
			Texts.Add(Parameter_WarmingUp, "攻击预热");
			Texts.Add(Parameter_ExplosiveRadius, "伤害半径");
			Texts.Add(Parameter_ArmorPenetration, "装甲穿透");
			Texts.Add(Parameter_Accuracy, "射击精度");
		}
	}
}
