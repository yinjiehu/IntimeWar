using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad
{
	public static class TextForVehicle
	{
		public const string VehicleCodePrefix = "VehicleCode_";
		public const string VehicleNameSimplePrefix = "VehicleNameSimple_";
		public const string VehicleNameFullPrefix = "VehicleNameFull_";
		public const string VehicleIntroducePrefix = "VehicleIntroduce_";

		public const string Parameter_BodyHP = "VehicleParameter_BodyHP";
		public const string Parameter_ArmorHP = "VehicleParameter_ArmorHP";
		public const string Parameter_MaxSpeed = "VehicleParameter_MaxSpeed";
		public const string Parameter_TurnSpeed = "VehicleParameter_TurnSpeed";
		public const string Parameter_Sight = "VehicleParameter_Sight";


		public static string GetNameFull(string vehicleID)
		{
			return Texts.Get(VehicleNameFullPrefix + vehicleID);
		}
		public static string GetNameSimple(string vechileID)
		{
			return Texts.Get(VehicleNameSimplePrefix + vechileID);
		}
		public static string GetIntroduce(string vechileID)
		{
			return Texts.Get(VehicleIntroducePrefix + vechileID);
		}

		public static void DemoCreate()
		{
			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_01_Raven, "PF-16 “渡鸦”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_01_Raven, "PF-16 “渡鸦” 装甲步兵");
			Texts.Add(VehicleIntroducePrefix + VehicleID.Mech_01_Raven, "PF-16是蓝翔重工的最新产品，主要用于快速侦察与扫荡。和PE系列相比，PF系列的特点是极大强化了雷达的效率，但相应牺牲了火力和续航能力。");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_01_Raven_M, "PF-16M “渡鸦”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_01_Raven_M, "PF-16M “渡鸦” 装甲侦察步兵");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_01_Raven_S, "PF-16S “渡鸦”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_01_Raven_S, "PF-16S “渡鸦” 装甲侦察步兵");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_02_Madcat, "ZX-79 “狂猫”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_02_Madcat, "ZX-79 “狂猫” 装甲步兵");
			Texts.Add(VehicleIntroducePrefix + VehicleID.Mech_02_Madcat, "ZX-79在3年前由蓝翔重工开发并投入测试，采用了第三代挖掘机技术，动力相比前一代的RX-77有了大幅增强，载重能力也因此提高，可以装备一个中型外挂组件意味着它可以对一般轻型侦察机体的决定性杀伤。");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_02_Madcat_M, "ZX-79M “狂猫”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_02_Madcat_M, "ZX-79M “狂猫” 装甲步兵");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_02_Madcat_P, "ZX-79P “狂猫”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_02_Madcat_P, "ZX-79P “狂猫” 装甲步兵");
			
			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_03_Avatar, "A5 “阿凡达”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_03_Avatar, "A5 “阿凡达” 装甲步兵");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_03_Avatar_P, "A5P “阿凡达”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_03_Avatar_P, "A5P “阿凡达” 装甲步兵");

			Texts.Add(VehicleNameSimplePrefix + VehicleID.Mech_03_Avatar_M, "A5M “阿凡达”");
			Texts.Add(VehicleNameFullPrefix + VehicleID.Mech_03_Avatar_M, "A5M “阿凡达” 装甲步兵");

			Texts.Add(Parameter_BodyHP, "结构强度");
			Texts.Add(Parameter_ArmorHP, "装甲强度");
			Texts.Add(Parameter_MaxSpeed, "移动速度");
			Texts.Add(Parameter_TurnSpeed, "炮塔转速");
			Texts.Add(Parameter_Sight, "视野范围");
		}
	}
}
