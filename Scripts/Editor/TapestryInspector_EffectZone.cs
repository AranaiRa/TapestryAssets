using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_EffectZone))]
public class TapestryInspector_EffectZone : Editor {

    int pSel;
    bool startup = true;

    public override void OnInspectorGUI()
    {
        Tapestry_EffectZone e = target as Tapestry_EffectZone;
        
        if (ReferenceEquals(e.effect, null))
            e.effect = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");
        if (ReferenceEquals(e.effect.payload, null))
            e.effect.payload = (Tapestry_EffectBuilder_Payload)ScriptableObject.CreateInstance("Tapestry_EffectBuilder_Payload_Damage");

        if (startup)
        {
            pSel = ArrayUtility.IndexOf(Tapestry_Config.GetPayloadTypes().Values.ToArray(), e.effect.payload.GetType());
            startup = false;
        }

        pSel = e.effect.DrawInspector(pSel);

        DrawDefaultInspector();
    }
}
