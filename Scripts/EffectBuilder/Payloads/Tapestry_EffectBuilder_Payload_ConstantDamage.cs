using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_ConstantDamage : Tapestry_EffectBuilder_Payload {

    public Tapestry_DamageType type;
    public float
        amountMin,
        amountMax,
        interval;
    [SerializeField]
    private float time;

    public Tapestry_EffectBuilder_Payload_ConstantDamage()
    {
        amountMin = 1;
        amountMax = 4;
        type = Tapestry_DamageType.Toxic;
    }

    public override void Apply()
    {
        time -= Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor * parent.target.personalTimeFactor;
        if(time <= 0)
        {
            time = interval;
            float amount = Random.Range(amountMin, amountMax);
            parent.target.DealDamage(type, amount);
        }
    }

    public override string ToString()
    {
        string export = "[<PAYLOAD:CONSTANT_DAMAGE> ";

        if (amountMin != amountMax)
            export += amountMin + "-" + amountMax + " ";
        else
            export += amountMin + " ";

        export += type.ToString() + "@";
        export += interval + "sec intervals]";

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

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Pulse Every");
        interval = EditorGUILayout.DelayedFloatField(interval, GUILayout.Width(42));
        if (interval < 0.1f)
            interval = 0.1f;
        GUILayout.Label("Seconds");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
