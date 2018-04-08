using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_Effect {

    public string name;
    public Sprite sprite;
    public bool 
        hideEffectDisplay = false;
    public Tapestry_EffectBuilder_Duration duration;
    public Tapestry_EffectBuilder_Payload payload;
    
    private float
        time;

	public Tapestry_Effect()
    {
        name = "Effect";
        duration = Tapestry_EffectBuilder_Duration.Instant;
    }

    public void Apply(Tapestry_Actor target)
    {
        payload.Apply(target);
    }

    public Tapestry_Effect Clone()
    {
        Tapestry_Effect export = (Tapestry_Effect)this.MemberwiseClone();
        return export;
    }

    public int DrawInspector(int pSel)
    {
        if (ReferenceEquals(payload, null))
            payload = (Tapestry_EffectBuilder_Payload_Damage)ScriptableObject.CreateInstance("Tapestry_EffectBuilder_Payload_Damage");
        Dictionary<string, Type> payloads = Tapestry_Config.GetPayloadTypes();

        GUIStyle title = new GUIStyle();
        title.fontStyle = FontStyle.Bold;
        title.fontSize = 14;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Duration", title);
        GUILayout.FlexibleSpace();
        duration = (Tapestry_EffectBuilder_Duration)EditorGUILayout.EnumPopup(duration);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        if (duration != Tapestry_EffectBuilder_Duration.Instant)
            payload.exposeTimeControls = true;

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Payload", title);
        GUILayout.FlexibleSpace();
        pSel = EditorGUILayout.Popup(pSel, payloads.Keys.ToArray());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical("box");
        Type pType = payloads[payloads.Keys.ToArray()[pSel]];
        if (ReferenceEquals(payload, null) || payload.GetType() != pType)
            payload = (Tapestry_EffectBuilder_Payload)ScriptableObject.CreateInstance(pType.ToString());
        payload.DrawInspector();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        return pSel;
    }
}

public enum Tapestry_EffectBuilder_Duration
{
    Instant,
    ActualTime, WorldTime,
    UntilEventRegistered, Permanent
}