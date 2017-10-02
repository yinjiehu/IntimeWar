using MechSquadShared;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MechSquad
{
	public static class DemoNameDefine
	{
#if UNITY_EDITOR
		[UnityEditor.MenuItem("Tools/GenerateNameDefineFile", priority = 60)]
		public static void GenerateNameDefine()
		{
			var types = new List<System.Type>()
			{
				typeof(VehicleID),
				typeof(SlotID),
				typeof(ActiveAttachmentID),
			};

			var list = new List<string>();
			foreach (var t in types)
			{
				list.AddRange(GetStaticFieldsString(t));
			}

			var path = UnityEngine.Application.dataPath + "/_Prefabs/Data/NameDefine/NameDefineAuto.txt";
			System.IO.File.WriteAllLines(path, list.ToArray());

			UnityEditor.AssetDatabase.ImportAsset(path);
		}
#endif

		static string[] GetStaticFieldsString(System.Type type)
		{
			var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			return fields.Select(f => type.Name + "/" + (string)f.GetValue(null)).ToArray();
		}
	}

	public static class VehicleID
	{
		public const string Mech_01_Raven = "Mech_01_Raven";
		public const string Mech_01_Raven_M = "Mech_01_Raven_M";
		public const string Mech_01_Raven_S = "Mech_01_Raven_S";

		public const string Mech_02_Madcat = "Mech_02_Madcat";
		public const string Mech_02_Madcat_P = "Mech_02_Madcat_P";
		public const string Mech_02_Madcat_M = "Mech_02_Madcat_M";

		public const string Mech_03_Avatar = "Mech_03_Avatar";
		public const string Mech_03_Avatar_P = "Mech_03_Avatar_P";
		public const string Mech_03_Avatar_M = "Mech_03_Avatar_M";
	}

	public static class SlotID
	{
		public const string LHand = "LHand";
		public const string RHand = "RHand";
		public const string LShoulder = "LShoulder";
		public const string RShoulder = "RShoulder";
	}

	public static class ActiveAttachmentID
	{
		public const string AT_AC02_R2_M270 = "AT_AC02_R2_M270";
		/// <summary>
		/// laser
		/// </summary>
		public const string AT_AC10_R2_LAS19 = "AT_AC10_R2_LAS19";

		public const string AT_SG01_S2_SPAS28 = "AT_SG01_S2_SPAS28";

		public const string AT_MG01_R3_RK97 = "AT_MG01_R3_RK97";

		public const string AT_CN01_S3_CP105 = "AT_CN01_S3_CP105";
		public const string AT_CN02_S3_CH105 = "AT_CN02_S3_CH105";
		/// <summary>
		/// canon
		/// </summary>
		public const string AT_CN30_S4_CP185 = "AT_CN30_S4_CP185";
		/// <summary>
		/// railgun
		/// </summary>
		public const string AT_CN50_S4_RAX65 = "AT_CN50_S4_RAX65";
		/// <summary>
		/// plasma
		/// </summary>
		public const string AT_CN60_S3_EC10 = "AT_CN60_S3_EC10";
		
		public const string AT_GRL01_G2_SS39 = "AT_GRL01_G2_SS39";
		public const string AT_GRL02_G3_SS51 = "AT_GRL02_G3_SS51";

		public const string AT_ARTL01_A4_MX510 = "AT_ARTL01_A4_MX510";
	}

	public static class PassiveAttachmentID
	{
		public const string Ammo_Shell_20 = "Ammo_Shell_20";
		public const string Ammo_Shell_45 = "Ammo_Shell_45";
		public const string Ammo_Shell_105AP = "Ammo_Shell_105AP";
		public const string Ammo_Shell_105HEAT = "Ammo_Shell_105HEAT";
		public const string Ammo_Shell_185AP = "Ammo_Shell_185AP";
		public const string Ammo_Slug_28 = "Ammo_Slug_28";
		public const string Ammo_Grenade_67 = "Ammo_Grenade_67";
		public const string Ammo_Grenade_100 = "Ammo_Grenade_100";
		
		public const string Up_Armor = "Up_Armor";
		public const string Up_Body = "Up_Body";
		public const string Up_TurnSpeed = "Up_TurnSpeed";
		public const string Up_MaxSpeed = "Up_MaxSpeed";
		public const string Up_Sight = "Up_Sight";
	}
}
