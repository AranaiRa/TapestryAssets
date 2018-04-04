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

    [MenuItem("Tapestry/Effect Builder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_EffectBuilder), true, "Tapestry Effect Builder", true);
        Debug.Log("running setup");
        effect = new Tapestry_Effect(null);
        HandleEffectBuilderClassRegistry();
    }

    private void OnGUI()
    {
        if (effect == null)
            effect = new Tapestry_Effect(null);
        if (deliveries == null || payloads == null)
            HandleEffectBuilderClassRegistry();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        
        GUIStyle title = GUIStyle.none;
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

        EditorGUILayout.BeginVertical("box");
        Type dType = deliveries[deliveries.Keys.ToArray()[dSel]];
        if (effect.delivery == null)
            effect.delivery = (Tapestry_EffectBuilder_Delivery)Activator.CreateInstance(dType);
        if (effect.delivery.GetType() != dType)
            effect.delivery = (Tapestry_EffectBuilder_Delivery)Activator.CreateInstance(dType);
        effect.delivery.DrawInspector();
        EditorGUILayout.EndVertical();

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
        if(effect.payload == null)
            effect.payload = (Tapestry_EffectBuilder_Payload)Activator.CreateInstance(pType);
        if (effect.payload.GetType() != pType)
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