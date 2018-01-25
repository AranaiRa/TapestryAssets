using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Actor))]
public class TapestryInspector_Actor : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_Actor t = target as Tapestry_Actor;

        GUILayout.Label("Actor is not a usable component!\nPlease use Entity, Prop, or a component that inherits from one of them.");
        GUILayout.Label("Replace With...");
        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Entity"))
            {
                t.gameObject.AddComponent<Tapestry_Entity>();
                DestroyImmediate(t);
            }
            if (GUILayout.Button("Prop"))
            {
                t.gameObject.AddComponent<Tapestry_Prop>();
                DestroyImmediate(t);
            }
        GUILayout.EndHorizontal();
    }
}
