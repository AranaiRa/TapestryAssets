using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_EffectZone))]
public class TapestryInspector_EffectZone : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_EffectZone e = target as Tapestry_EffectZone;

        if(e.effect.DrawInspector())
        {
            TapestryEditor_EffectBuilder.RegisterEffect(e.effect);
            TapestryEditor_EffectBuilder.ShowWindow();
        }

        base.OnInspectorGUI();
    }
}
