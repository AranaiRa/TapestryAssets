using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_EffectBuilder_Delivery_Self : Tapestry_EffectBuilder_Delivery {
    
    public Tapestry_EffectBuilder_Delivery_Self()
    {

    }

    public override List<Tapestry_Actor> GetAffectedTargets()
    {
        List<Tapestry_Actor> targets = new List<Tapestry_Actor>();
        targets.Add(parent.initiator.GetComponentInParent<Tapestry_Actor>());
        return targets;
    }

    public override string ToString()
    {
        string export = "<b>SELF</b>: Affects only the initiator";

        return export;
    }

    public override void DrawInspector()
    {
        base.DrawInspector();
    }
}
