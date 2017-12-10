﻿using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MechSquad
{
    public class UnitInitialParameter
    {
        Dictionary<string, float> _basicParameters = new Dictionary<string, float>();

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


        public static UnitInitialParameter Create(string typeID)
        {
            var ret = new UnitInitialParameter();

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
            }
            return ret;
        }
    }
}