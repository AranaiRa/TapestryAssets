using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Level))]
public class TapestryInspector_Level : Editor {

    public override void OnInspectorGUI()
    {
        //Tapestry_Level l = target as Tapestry_Level;
        
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("Day Length (DEBUG)");
        Tapestry_WorldClock.dayLength = EditorGUILayout.DelayedFloatField(Tapestry_WorldClock.dayLength);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("DEFAULT INSPECTOR BELOW");
        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();
    }
}
