using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Kit.Utility.OpenXml;

namespace IntimeWar.Data
{
    [Serializable]
	public class PlayerSettingsCollection
    {
        public List<PlayerSettings> Collection;

        public PlayerSettings Get(string id, bool throwExceptionWhenNotFound = true)
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
            Collection = new List<PlayerSettings>();
            var parser = new OpenXmlParser();
            parser.LoadXml(xml);

            this.LoadFromXmlParser(parser);
        }


        public void LoadFromPath(string path)
        {
            Collection = new List<PlayerSettings>();

            var parser = new OpenXmlParser();
            parser.LoadFromPath(path);

            this.LoadFromXmlParser(parser);
        }

        private void LoadFromXmlParser(OpenXmlParser parser)
        {

        }
    }


    [Serializable]
    public class PlayerSettings
    {
        public string ID;
        public string PrefabName;
        public Dictionary<string, float> ExtraParameters;

        public List<string> Skills;

        public float GetParameter(string name, bool allowNotExist = true)
        {
            float ret = 0;
            if (ExtraParameters.TryGetValue(name, out ret))
                return ret;

            if (allowNotExist)
                return 0;

            throw new System.Exception(string.Format("PlayerSettings {0} parameter {1} is not exist!", ID, name));
        }
    }
}
