using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tapestry_EffectBuilder_Payload_BreakPushLift : Tapestry_EffectBuilder_Payload {

	public Tapestry_EffectBuilder_Payload_BreakPushLift()
    {
        mustBeInstant = true;
    }

    public override void Apply(Tapestry_Actor target)
    {
        base.Apply(target);
    }

    public override void DrawInspector()
    {
        base.DrawInspector();
    }
}
