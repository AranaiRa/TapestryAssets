using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_EffectZone))]
public class TapestryInspector_EffectZone : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_EffectZone e = target as Tapestry_EffectZone;
        if (e && ReferenceEquals(e.effect, null))
        {
            e.effect = new Tapestry_Effect();
            Debug.Log("checking for whether this thing is nullshit");
        }

        if(e.effect.DrawInspector())
        {
            Debug.Log((e.effect == null));
            TapestryEditor_EffectBuilder.RegisterEffect(e.effect, e);
            TapestryEditor_EffectBuilder.ShowWindow();
        }

        base.OnInspectorGUI();
    }
}
