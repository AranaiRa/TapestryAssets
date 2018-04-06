using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public class TapestryEditor_EffectBuilder : EditorWindow
{
    static Tapestry_Effect effect;
    int
        dSel,
        durSel,
        pSel;
    static Dictionary<string, Type>
        deliveries,
        payloads;
    public Vector2 scrollPos;
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_EffectBuilder), true, "Tapestry Effect Builder", true);
        HandleEffectBuilderClassRegistry();
    }

    public static void RegisterEffect(Tapestry_Effect e)
    {
        effect = null;
        effect = e;
    }

    public static Tapestry_Effect GetEffect()
    {
        return effect;
    }

    private void OnGUI()
    {
        if (effect == null)
            effect = new Tapestry_Effect();
        if (effect.payload == null)
            effect.payload = new Tapestry_EffectBuilder_Payload_Damage();
        if (deliveries == null || payloads == null)
            HandleEffectBuilderClassRegistry();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        
        pSel = Array.IndexOf(payloads.Values.ToArray(), effect.payload.GetType());

        GUIStyle title = new GUIStyle();
        title.fontStyle = FontStyle.Bold;
        title.fontSize = 14;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Duration", title);
        GUILayout.FlexibleSpace();
        effect.duration = (Tapestry_EffectBuilder_Duration)EditorGUILayout.EnumPopup(effect.duration);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Delivery", title);
        GUILayout.FlexibleSpace();
        dSel = EditorGUILayout.Popup(dSel, deliveries.Keys.ToArray());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Payload", title);
        GUILayout.FlexibleSpace();
        pSel = EditorGUILayout.Popup(pSel, payloads.Keys.ToArray());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical("box");
        //GUILayout.Label("None Selected");
        Type pType = payloads[payloads.Keys.ToArray()[pSel]];
        if(effect.payload == null || effect.payload.GetType() != pType)
            effect.payload = (Tapestry_EffectBuilder_Payload)Activator.CreateInstance(pType);
        effect.payload.DrawInspector();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private static void HandleEffectBuilderClassRegistry()
    {
        deliveries = new Dictionary<string, Type>();
        payloads = new Dictionary<string, Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.BaseType == typeof(Tapestry_EffectBuilder_Delivery))
                {
                    deliveries.Add(type.Name.Substring(32, type.Name.Length - 32), type);
                }
                if (type.BaseType == typeof(Tapestry_EffectBuilder_Payload))
                {
                    payloads.Add(type.Name.Substring(31, type.Name.Length - 31), type);
                }
            }
        }
    }
}