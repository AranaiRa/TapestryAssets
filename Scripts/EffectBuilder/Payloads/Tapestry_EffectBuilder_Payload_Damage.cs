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
        amountMax,
        pulse;

    private float
        time;

    public Tapestry_EffectBuilder_Payload_Damage()
    {
        amountMin = 10;
        amountMax = 40;
        pulse = 0.5f;
        type = Tapestry_DamageType.Crushing;
    }

    public override void Apply(Tapestry_Actor target)
    {
        bool execute = false;
        if (exposeTimeControls)
        {
            time -= Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor * target.personalTimeFactor;
            if (time <= 0)
                execute = true;
        }
        else
            execute = true;
        if (execute)
        {
            float amount = Random.Range(amountMin, amountMax);
            target.DealDamage(type, amount);
            if (exposeTimeControls)
                time = pulse;
        }
    }

    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Damage Spread");
        amountMin = EditorGUILayout.DelayedFloatField(amountMin, GUILayout.Width(36));
        if (amountMin < 0)
            amountMin = 0;
        GUILayout.Label("-");
        amountMax = EditorGUILayout.DelayedFloatField(amountMax, GUILayout.Width(36));
        if (amountMax < amountMin)
            amountMax = amountMin;
        GUILayout.FlexibleSpace();
        GUILayout.Label("Type");
        type = (Tapestry_DamageType)EditorGUILayout.EnumPopup(type, GUILayout.Width(64));
        GUILayout.EndHorizontal();

        if(exposeTimeControls)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            GUILayout.Label("Pulse Every ");
            pulse = EditorGUILayout.DelayedFloatField(pulse, GUILayout.Width(42));
            GUILayout.Label("Seconds");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
