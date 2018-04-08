using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(testscript))]
public class testinspector : Editor {

    public override void OnInspectorGUI()
    {
        testscript ts = target as testscript;
        
        if (ts.e.DrawInspector())
        {
            Debug.Log(ts.e.ToString()+" | "+(ReferenceEquals(ts.e, null)));
            Debug.Log(ts.e.payload.ToString() + " | " + (ReferenceEquals(ts.e, null)));
            //TapestryEditor_EffectBuilder.RegisterEffect(ts.e);
            //TapestryEditor_EffectBuilder.ShowWindow();
        }
    }
}
