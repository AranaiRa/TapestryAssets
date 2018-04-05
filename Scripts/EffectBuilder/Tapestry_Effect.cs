using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_Effect {

    public string name;
    public Sprite sprite;
    public bool 
        hideEffectDisplay = false;
    public Tapestry_EffectBuilder_Delivery delivery;
    public Tapestry_EffectBuilder_Duration duration;
    public Tapestry_EffectBuilder_Payload payload;
    public Transform initiator;
    public Tapestry_Actor target;

    [SerializeField]
    private float
        time;

	public Tapestry_Effect(Transform initiator)
    {
        this.initiator = initiator;
        delivery = new Tapestry_EffectBuilder_Delivery_Self();
        duration = Tapestry_EffectBuilder_Duration.Instant;
        payload = new Tapestry_EffectBuilder_Payload_Damage();
    }

    public Tapestry_Effect Clone(Transform newInitiator)
    {
        Tapestry_Effect export = (Tapestry_Effect)this.MemberwiseClone();
        export.initiator = newInitiator;
        return export;
    }

    public bool DrawInspector()
    {
        bool buttonActive = false;

        GUIStyle wrapped = new GUIStyle();
        wrapped.richText = true;
        wrapped.wordWrap = true;

        GUIStyle title = new GUIStyle();
        title.fontStyle = FontStyle.Bold;
        title.fontSize = 16;
        title.alignment = TextAnchor.MiddleLeft;

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Effect", title);
        GUILayout.FlexibleSpace();
        buttonActive = GUILayout.Button("Builder", GUILayout.Width(60));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("<b>DURATION</b>: " + duration.ToString(), wrapped);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        if (delivery == null)
            GUILayout.Label("<b>DELIVERY</b>: Null", wrapped);
        else
            GUILayout.Label(delivery.ToString(), wrapped);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        if (payload == null)
            GUILayout.Label("<b>PAYLOAD</b>: Null", wrapped);
        else
            GUILayout.Label(payload.ToString(), wrapped);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        return buttonActive;
    }
}

public enum Tapestry_EffectBuilder_Duration
{
    Instant,
    ActualTime, WorldTime,
    UntilEventRegistered, Permanent
}