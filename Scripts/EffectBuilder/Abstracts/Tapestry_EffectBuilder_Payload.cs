using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_EffectBuilder_Payload : ScriptableObject {
    
    public bool AffectsPropsOnly;
    public bool
        isStackable = false,
        exposeTimeControls = false;

    public virtual void Apply(Tapestry_Actor target)
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
