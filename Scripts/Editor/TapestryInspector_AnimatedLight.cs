using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_AnimatedLight))]
public class TapestryInspector_AnimatedLight : Editor
{    
    public override void OnInspectorGUI()
    {
        Tapestry_AnimatedLight l = target as Tapestry_AnimatedLight;

        GUILayout.BeginVertical("box");
        if(GUILayout.Button("On"))
        {
            l.TurnOn();
        }
        if(GUILayout.Button("Off"))
        {
            l.TurnOff();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("DEFAULT BELOW");
        GUILayout.EndVertical();

        DrawDefaultInspector();
    }
}
