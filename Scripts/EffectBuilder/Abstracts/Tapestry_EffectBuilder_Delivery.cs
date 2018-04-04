using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tapestry_EffectBuilder_Delivery {

    public Tapestry_Effect parent;

    public virtual List<Tapestry_Actor> GetAffectedTargets()
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
