﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_Impulse : Tapestry_EffectBuilder_Payload {

    public Vector3 dir;
    public float strength;
    public bool useLocalDirection;

    public Tapestry_EffectBuilder_Payload_Impulse()
    {
        dir = new Vector3(0, 1, 0);
        strength = 10.0f;
    }

    public override void Apply(Tapestry_Actor target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if(rb != null)
        {
            if(target.GetType() == typeof(Tapestry_Player))
            {
                Tapestry_Player p = target as Tapestry_Player;
                p.RestrictControls = true;
            }
            rb.velocity = dir * strength;
        }
    }

    #if UNITY_EDITOR
    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Direction");
        GUILayout.Space(4);
        GUILayout.Label("X");
        dir.x = EditorGUILayout.DelayedFloatField(dir.x, GUILayout.Width(64));
        GUILayout.Label("Y");
        dir.y = EditorGUILayout.DelayedFloatField(dir.y, GUILayout.Width(64));
        GUILayout.Label("Z");
        dir.z = EditorGUILayout.DelayedFloatField(dir.z, GUILayout.Width(64));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //GUILayout.Space(40);
        //useLocalDirection = EditorGUILayout.Toggle(useLocalDirection, GUILayout.Width(12));
        //GUILayout.Label("Use local direction?");
        //GUILayout.FlexibleSpace();
        //if(GUILayout.Button("Get Dir from Gizmo"))
        //{
        //
        //}
        //GUILayout.FlexibleSpace();
        //GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Strength");
        strength = EditorGUILayout.DelayedFloatField(strength, GUILayout.Width(42));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (exposeTimeControls)
        {
        }

        GUILayout.EndVertical();
    }
    #endif
}
