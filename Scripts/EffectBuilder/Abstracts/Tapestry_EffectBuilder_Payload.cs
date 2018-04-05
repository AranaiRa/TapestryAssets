using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tapestry_EffectBuilder_Payload {

    public Tapestry_Effect parent;
    public bool IsStackable;
    public bool AffectsEntitiesOnly;
    public bool AffectsPropsOnly;

    public virtual void Apply()
    {
        throw new System.NotImplementedException();
    }

    public virtual void DrawInspector()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("This inspector is not yet written.");
        GUILayout.EndVertical();
    }
}
