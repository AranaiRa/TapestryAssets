using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_DamageProfile {
    
    /// <summary>
    /// X corresponds to Resistance (-%) value. Y corresponds to Mitigation (-X, after Resistance) value.
    /// </summary>
    Dictionary<Tapestry_DamageType, Tapestry_DamageTypeIndex> dict = new Dictionary<Tapestry_DamageType, Tapestry_DamageTypeIndex>();

    public Tapestry_DamageProfile()
    {
        foreach (Tapestry_DamageType val in Enum.GetValues(typeof(Tapestry_DamageType)))
        {
            dict.Add(val, new Tapestry_DamageTypeIndex(0,0));
        }
    }

    public void SetProfile(Tapestry_DamageType type, float res, float mit)
    {
        dict[type] = new Tapestry_DamageTypeIndex(res, mit);
    }

    public void SetRes(Tapestry_DamageType type, float res)
    {
        dict[type] = new Tapestry_DamageTypeIndex(res, dict[type].Mitigation);
    }

    public void SetMit(Tapestry_DamageType type, float mit)
    {
        dict[type] = new Tapestry_DamageTypeIndex(dict[type].Resistance, mit);
    }

    public float GetRes(Tapestry_DamageType type)
    {
        return dict[type].Resistance;
    }

    public float GetMit(Tapestry_DamageType type)
    {
        return dict[type].Mitigation;
    }
}
