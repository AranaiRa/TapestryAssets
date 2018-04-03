using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public class TapestryEditor_EffectBuilder : EditorWindow
{
    Dictionary<string, Type>
        shapes,
        payloads;
    public Vector2 scrollPos;

    [MenuItem("Tapestry/Effect Builder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_EffectBuilder), true, "Tapestry Effect Builder", true);
    }

    private void OnGUI()
    {
        HandleShapeRegistry();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Delivery");
        GUILayout.FlexibleSpace();
        EditorGUILayout.Popup(0, shapes.Keys.ToArray());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("None Selected");
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Duration");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Payload");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("None Selected");
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void HandleShapeRegistry()
    {
        shapes = new Dictionary<string, Type>();
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if(type.GetInterface("ITapestry_EffectBuilder_Shape") != null)
                {
                    shapes.Add(type.Name.Substring(29,type.Name.Length-29), type);
                }
            }
        }
    }
}