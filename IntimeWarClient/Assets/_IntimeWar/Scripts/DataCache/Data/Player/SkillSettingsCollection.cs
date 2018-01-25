using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Kit.Utility.OpenXml;

namespace IntimeWar.Data
{
    [Serializable]
	public class SkillSettingsCollection
    {
        public List<SkillSettings> Collection;

        public SkillSettings Get(string id, bool throwExceptionWhenNotFound = true)
        {
            var ret = Collection.First(s => s.ID == id);
            if (ret == null && throwExceptionWhenNotFound)
            {
                throw new System.Exception(string.Format("Can not find id [{0}]", id));
            }
            return ret;
        }
        public bool IsExsit(string id)
        {
            return Collection.First(s => s.ID == id) != null;
        }

        public void LoadFromXmlText(string xml)
        {
            Collection = new List<SkillSettings>();
            var parser = new OpenXmlParser();
            parser.LoadXml(xml);

            this.LoadFromXmlParser(parser);
        }


        public void LoadFromPath(string path)
        {
            Collection = new List<SkillSettings>();

            var parser = new OpenXmlParser();
            parser.LoadFromPath(path);

            this.LoadFromXmlParser(parser);
        }

        private void LoadFromXmlParser(OpenXmlParser parser)
        {

        }
    }


    [Serializable]
    public class SkillSettings
    {
        public string ID;
        public string SkillName;
        public string PrefabName;
        public string IconName;
        public string SkillDescription;
        public float ReloadSeconds;
        public float Damage;
    }
}
