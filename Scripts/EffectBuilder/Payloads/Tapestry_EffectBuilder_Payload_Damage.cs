using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_Damage : Tapestry_EffectBuilder_Payload
{
    public Tapestry_DamageType type;
    public float 
        amountMin,
        amountMax;

    public Tapestry_EffectBuilder_Payload_Damage()
    {
        amountMin = 10;
        amountMax = 40;
        type = Tapestry_DamageType.Crushing;
    }

    public override void Apply()
    {
        float amount = Random.Range(amountMin, amountMax);
        parent.target.DealDamage(type, amount);
        Debug.Log("Dealing " + amount + " raw " + type.ToString() + " damage to " + parent.target.name);
    }

    public override string ToString()
    {
        string export = "[<PAYLOAD:DAMAGE> ";

        if (amountMin != amountMax)
            export += amountMin + "-" + amountMax + " ";
        else
            export += amountMin + " ";
        
        export += type.ToString() + "]";

        return export;
    }

    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Min", GUILayout.Width(30));
        amountMin = EditorGUILayout.DelayedFloatField(amountMin, GUILayout.Width(42));
        if (amountMin < 0)
            amountMin = 0;
        GUILayout.FlexibleSpace();
        GUILayout.Label("Max", GUILayout.Width(30));
        amountMax = EditorGUILayout.DelayedFloatField(amountMax, GUILayout.Width(42));
        if (amountMax < amountMin)
            amountMax = amountMin;
        GUILayout.FlexibleSpace();
        GUILayout.Label("Type");
        type = (Tapestry_DamageType)EditorGUILayout.EnumPopup(type, GUILayout.Width(128));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
