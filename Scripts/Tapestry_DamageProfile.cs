using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
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

    public void DrawInspector()
    {
        GUILayout.BeginVertical("box");
                
        string resTooltip = "Resistance: All incoming damage of this type is reduced by the listed value (EG: 0.5 will reduce damage by 50%, -0.5 will increase it by 50%).";
        string mitTooltip = "Mitigation: Damage taken subtracts this amount after Resistance is applied.";

        foreach (var v in Enum.GetValues(typeof(Tapestry_DamageType)))
        {
            Tapestry_DamageType val = (Tapestry_DamageType)v;

            GUILayout.BeginHorizontal();

            GUILayout.Label(val.ToString(), GUILayout.Width(70));

            GUILayout.FlexibleSpace();
                    
            GUILayout.Label(new GUIContent("RES", resTooltip), GUILayout.Width(30));
            SetRes(val, EditorGUILayout.FloatField(GetRes(val), GUILayout.Width(40)));

            GUILayout.FlexibleSpace();
                    
            GUILayout.Label(new GUIContent("MIT", mitTooltip), GUILayout.Width(30));
            SetMit(val, EditorGUILayout.FloatField(GetMit(val), GUILayout.Width(40)));

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
