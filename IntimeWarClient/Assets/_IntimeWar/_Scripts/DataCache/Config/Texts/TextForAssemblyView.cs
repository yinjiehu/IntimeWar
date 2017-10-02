using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechSquad
{
	public static class TextForAssemblyView
	{
		public const string SlotDisabled = "AssemblyView_SlotDisabled";
		public const string ClickSlot = "AssemblyView_ClickSlot";

		public const string BasicValue = "AssemblyView_BasicValue";
		public const string NotLoadedSlot = "AssemblyView_NotLoadedSlot";

		public const string SlotNamePrefix = "AssemblyView_SlotName_";

		public static void DemoCreate()
		{
			Texts.Add(SlotDisabled, "不可用");
			Texts.Add(ClickSlot, "点击以装载附件组件");
			Texts.Add(BasicValue, "基础值");
			Texts.Add(NotLoadedSlot, "未使用装备槽");


			Texts.Add(SlotNamePrefix + SlotID.LHand, "左手臂");
			Texts.Add(SlotNamePrefix + SlotID.RHand, "右手臂");
			Texts.Add(SlotNamePrefix + SlotID.LShoulder, "左侧躯干");
			Texts.Add(SlotNamePrefix + SlotID.RShoulder, "右侧躯干");
		}
	}
}
