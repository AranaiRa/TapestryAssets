using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_Healing : Tapestry_EffectBuilder_Payload
{
    public float
        amountMin,
        amountMax,
        pulse;

    private float
        time;

    public Tapestry_EffectBuilder_Payload_Healing()
    {
        amountMin = 100;
        amountMax = 400;
        pulse = 0.5f;
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
            target.Heal(amount);
            if (exposeTimeControls)
                time = pulse;
        }
    }

    #if UNITY_EDITOR
    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Range");
        amountMin = EditorGUILayout.DelayedFloatField(amountMin, GUILayout.Width(42));
        if (amountMin < 0)
            amountMin = 0;
        GUILayout.Label("-");
        amountMax = EditorGUILayout.DelayedFloatField(amountMax, GUILayout.Width(42));
        if (amountMax < amountMin)
            amountMax = amountMin;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (exposeTimeControls)
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
    #endif
}
