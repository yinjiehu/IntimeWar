using System.Collections;
using UnityEngine;
using System;
using IntimeWar.Data;
using IntimeWar;

namespace Demo
{
	public class DemoSkillSettings
    {
        public static void InitSettings()
        {
            var collection = new SkillSettingsCollection();
            collection.Collection = new System.Collections.Generic.List<SkillSettings>()
            {
                new SkillSettings()
                {
                    ID = "Skill_1_1",
                    SkillName = "变身炽天使",
                    PrefabName = "Skill_1_1",
                    IconName = "Attack_1",
                    SkillDescription = "变身炽天使战斗",
                    ReloadSeconds = 0.5f,
                    Damage = 30,
                },
                new SkillSettings()
                {
                    ID = "Skill_1_2",
                    SkillName = "变身炽天使",
                    PrefabName = "Skill_1_2",
                    IconName = "Attack_2",
                    SkillDescription = "变身炽天使战斗",
                    ReloadSeconds = 8,
                    Damage = 30,
                },
                new SkillSettings()
                {
                    ID = "Skill_1_3",
                    SkillName = "变身炽天使",
                    PrefabName = "Skill_1_3",
                    IconName = "Attack_3",
                    SkillDescription = "变身炽天使战斗",
                    ReloadSeconds = 8,
                    Damage = 30,
                },
                new SkillSettings()
                {
                    ID = "Skill_1_4",
                    SkillName = "变身炽天使",
                    PrefabName = "Skill_1_4",
                    IconName = "Attack_4",
                    SkillDescription = "变身炽天使战斗",
                    ReloadSeconds = 8,
                    Damage = 30,
                }
            };

            GlobalCache.Set(GlobalCacheKey.SkillSettingsCollection, collection);
        }

	}
}
