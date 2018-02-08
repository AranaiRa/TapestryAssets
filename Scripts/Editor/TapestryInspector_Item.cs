using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Item))]
public class TapestryInspector_Item : Editor
{
    
    public override void OnInspectorGUI()
    {
        Tapestry_Item i = target as Tapestry_Item;

        DrawDefaultInspector();
    }
}