using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Tapestry_EffectBuilder_Payload_BreakPushLift : Tapestry_EffectBuilder_Payload {

	public Tapestry_EffectBuilder_Payload_BreakPushLift()
    {
        mustBeInstant = true;
    }

    public override void Apply(Tapestry_Actor target)
    {
        base.Apply(target);
    }

    #if UNITY_EDITOR
    public override void DrawInspector()
    {
        base.DrawInspector();
    }
    #endif
}
