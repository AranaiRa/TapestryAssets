using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(testscript))]
public class testinspector : Editor {

    public override void OnInspectorGUI()
    {
        testscript ts = target as testscript;

        ts.s.DrawInspector();
        ts.d.DrawInspector();
    }
}
