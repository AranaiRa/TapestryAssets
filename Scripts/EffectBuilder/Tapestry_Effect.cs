using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_Effect : ScriptableObject {

    public string displayName;
    public Sprite sprite;
    public bool 
        hideEffectDisplay = false,
        readyForRemoval = false;
    public Tapestry_EffectBuilder_Duration duration;
    public Tapestry_EffectBuilder_Payload payload;
    public float
        decayTime = 30f;

    private float
        time;

	public Tapestry_Effect()
    {
        displayName = "Unnamed Effect";
        duration = Tapestry_EffectBuilder_Duration.Instant;
    }

    public void Apply(Tapestry_Actor target)
    {
        payload.Apply(target);

        if (duration == Tapestry_EffectBuilder_Duration.Instant)
            readyForRemoval = true;
        else if(duration == Tapestry_EffectBuilder_Duration.Timed)
        {
            time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor * target.personalTimeFactor;
            if (time >= decayTime)
                readyForRemoval = true;
        }
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
        GUILayout.BeginHorizontal();
        GUILayout.Label("Effect", title);
        displayName = EditorGUILayout.DelayedTextField(displayName);
        GUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Duration", title);
        GUILayout.FlexibleSpace();
        if(payload.mustBeInstant)
        {
            duration = Tapestry_EffectBuilder_Duration.Instant;
            EditorGUILayout.Popup(0,new string[]{"Instant"});
        }
        else
            duration = (Tapestry_EffectBuilder_Duration)EditorGUILayout.EnumPopup(duration);
        EditorGUILayout.EndHorizontal();

        if (duration == Tapestry_EffectBuilder_Duration.Timed)
        {
            payload.exposeTimeControls = true;
            EditorGUILayout.BeginHorizontal("box");

            GUILayout.Space(40);
            decayTime = EditorGUILayout.DelayedFloatField(decayTime, GUILayout.Width(42));
            GUILayout.Label("Seconds");
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }
        else if (duration == Tapestry_EffectBuilder_Duration.Permanent)
            payload.exposeTimeControls = true;
        else
            payload.exposeTimeControls = false;

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Payload", title);
        GUILayout.FlexibleSpace();
        pSel = EditorGUILayout.Popup(pSel, payloads.Keys.ToArray());
        EditorGUILayout.EndHorizontal();
        
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
    Timed,
    /*UntilEventRegistered,*/
    Permanent
}