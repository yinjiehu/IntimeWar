using System.Collections;
using UnityEngine;
using System;
using IntimeWar.Data;
using IntimeWar;

namespace Demo
{
	public class DemoPlayerSettings
	{
        public static void InitSettings()
        {
            var collection = new PlayerSettingsCollection();
            collection.Collection = new System.Collections.Generic.List<PlayerSettings>()
            {
                new PlayerSettings()
                {
                    ID = "Player_1_1",
                    PrefabName = "Player_1_1",
                    ExtraParameters = new System.Collections.Generic.Dictionary<string, float>()
                    {
                        { "BodyHp", 500 },
                        {  "Mobility", 3 },
                    },
                    Skills = new System.Collections.Generic.List<string>()
                    {
                        "Skill_1_1",
                        "Skill_1_2",
                        "Skill_1_3",
                        "Skill_1_4",
                    }
                }
            };

            GlobalCache.Set(GlobalCacheKey.PlayerSettingsCollection, collection);
        }

	}
}
