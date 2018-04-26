using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_InspectorHelper))]
public class TapestryInspector_InspectorHelper : Editor
{
    public override void OnInspectorGUI()
    {
        Tapestry_InspectorHelper h = target as Tapestry_InspectorHelper;

        Texture2D bg1 = new Texture2D(1, 1);
        bg1.SetPixels(new Color[] { new Color(1,1,0) });
        GUIStyle style = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 24,
            fontStyle = FontStyle.Bold
        };
        style.normal.background = bg1;
        
        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal("box", GUILayout.Width(32), GUILayout.Height(32));
        GUILayout.Label("!", style);
        GUILayout.EndHorizontal();

        Texture2D bg2 = new Texture2D(1, 1);
        bg2.SetPixels(new Color[] { new Color(1, 1, 0.75f) });
        style = new GUIStyle
        {
            wordWrap = true,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 11,
            fontStyle = FontStyle.Normal
        };
        style.normal.background = bg2;
        style.padding = new RectOffset(6, 6, 6, 6);

        GUILayout.BeginHorizontal("box");
        GUILayout.Label(h.helpMessage, style);
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("OK", GUILayout.Width(100)))
        {
            DestroyImmediate(h);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        style.normal.background = null;
    }
}
