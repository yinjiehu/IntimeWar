using UnityEngine;
using System;


namespace MechSquad
{
	public enum LogLevelEnum
	{
		Error = 0,
		Warning = 1,
		Info = 2,
		Debug = 3,
		Trace = 4
	}

	[CreateAssetMenu(fileName = "MechSquadPreference", menuName = "MechSquad")]
	public class MechSquadPreference : ScriptableObject
	{
		static MechSquadPreference _instance;
		public static MechSquadPreference Instance { get { return _instance; } }
		
		[SerializeField]
		LogLevelEnum _mechSquadLogLevel;
		public static LogLevelEnum LogLevel { get { return Instance._mechSquadLogLevel; } }
        public static bool CheckLogLevel(LogLevelEnum level) { return (int)LogLevel >= (int)level; }
		
		[RuntimeInitializeOnLoadMethod]
		private static void InitMechSquadPreference()
		{
			_instance = Resources.Load<MechSquadPreference>("MechSquadPreference");
		}
		
		[SerializeField]
		[Haruna.Inspector.ListByNameDefine("PhotonServerAddress")]
		string _photonServerAddress;
		public static string PhotonServerAddress { get { return Instance._photonServerAddress; } }
		
#if UNITY_EDITOR
		[UnityEditor.MenuItem("Tools/MechSquadPreference", priority = 10)]
		static void SelectPreference()
		{
			UnityEditor.Selection.activeObject = Resources.Load<MechSquadPreference>("MechSquadPreference");
		}
#endif

	}
}
