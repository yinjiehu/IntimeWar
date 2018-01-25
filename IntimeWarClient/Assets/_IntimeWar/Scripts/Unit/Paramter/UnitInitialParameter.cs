using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Shared.Models;
using IntimeWar;

namespace YJH.Unit
{
    public class UnitInitialParameter
    {
        Dictionary<string, float> _basicParameters = new Dictionary<string, float>();
        Dictionary<int, string> _skills = new Dictionary<int, string>();

        public float GetParameter(string type)
        {
            float v;
            if (_basicParameters.TryGetValue(type, out v))
            {
                return v;
            }
            return 0;
        }
        public void SetParameter(string type, float value)
        {
            if (_basicParameters.ContainsKey(type))
            {
                _basicParameters[type] = value;
            }
            else
            {
                _basicParameters.Add(type, value);
            }
        }


        public Dictionary<int, string> GetSkills()
        {
            return _skills;
        }


        public static UnitInitialParameter Create(string typeID)
        {
            var ret = new UnitInitialParameter();

            return ret;
        }

        public static UnitInitialParameter Create(PlayerStatus player)
        {
            var collection = GlobalCache.GetPlayerSettings();
            var settings = collection.Get(player.PlayerClassify);
            var ret = new UnitInitialParameter();
            ret._basicParameters.Add("BodyHp", settings.GetParameter("BodyHp"));
            ret._basicParameters.Add("Mobility", settings.GetParameter("Mobility"));
            int n = 0;
            foreach(var skill in settings.Skills)
            {
                ret._skills.Add(n, skill);
                n++;
            }
            return ret;
        }

        public ExitGames.Client.Photon.Hashtable ToPhotonHashtable()
        {
            var ret = new ExitGames.Client.Photon.Hashtable();
            {
                var parameter = new ExitGames.Client.Photon.Hashtable();
                ret.Add(0, parameter);
                foreach (var kv in _basicParameters)
                {
                    parameter.Add(kv.Key, kv.Value);
                }
                parameter = new ExitGames.Client.Photon.Hashtable();
                ret.Add(1, parameter);
                foreach (var sk in _skills)
                {
                    parameter.Add(sk.Key, sk.Value);
                }
            }

            return ret;
        }

        public static UnitInitialParameter FromPhotonHashtable(ExitGames.Client.Photon.Hashtable table)
        {
            var ret = new UnitInitialParameter();
            {
                var parameter = table[0] as ExitGames.Client.Photon.Hashtable;
                foreach (var kv in parameter)
                {
                    ret._basicParameters.Add((string)kv.Key, (float)kv.Value);
                }

                parameter = table[1] as ExitGames.Client.Photon.Hashtable;
                foreach(var sk in parameter)
                {
                    ret._skills.Add((int)sk.Key, (string)sk.Value);
                }
            }
            return ret;
        }
    }
}
