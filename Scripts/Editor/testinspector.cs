using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(testscript))]
public class testinspector : Editor {

    int pSel;

    public override void OnInspectorGUI()
    {
        testscript ts = target as testscript;

        pSel = ts.e.DrawInspector(pSel);

        GUILayout.Label("has payload? " + (ts.e.payload != null));
        GUILayout.Label("payload type? " + ts.e.payload.GetType().ToString());

        DrawDefaultInspector();
    }
}
