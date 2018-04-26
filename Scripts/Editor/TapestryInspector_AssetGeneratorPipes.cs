using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_AssetGeneratorPipes))]
public class TapestryInspector_AssetGeneratorPipes : Editor
{

    public Tapestry_AssetGeneratorPipes_Segment
        current;
    public int toolbarActive = 0;
    public string[] toolbarNames = { "Pipe System", "Prefabs" };

    public override void OnInspectorGUI()
    {
        Tapestry_AssetGeneratorPipes agp = target as Tapestry_AssetGeneratorPipes;

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Pipe System")
                DrawSystemTab(agp);

            if (toolbarNames[toolbarActive] == "Prefabs")
                DrawPrefabsTab(agp);
        }
    }

    private void DrawSystemTab(Tapestry_AssetGeneratorPipes agp)
    {
        GUILayout.BeginVertical("box");

        GUIStyle section = new GUIStyle
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };

        GUILayout.BeginVertical("box");
        GUILayout.Label("Current Segment", section);

        GUILayout.BeginHorizontal();
        if (agp.GetCurrentSegment() == null)
        {
            GUILayout.Label("No segment placed.");
        }
        else
        {
            agp.GetCurrentSegment().transform.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", agp.GetCurrentSegment().transform.localEulerAngles);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove Segment", GUILayout.Width(250)))
            {
                agp.RemoveSegment();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Add New Segment", section);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Short Straight", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabShort);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Medium Straight", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabMedium);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Long Straight", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabLong);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("15° Bend", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabBend15);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("30° Bend", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabBend30);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("45° Bend", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabBend45);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("60° Bend", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabBend60);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("75° Bend", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabBend75);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("90° Bend", GUILayout.Width(110)))
        {
            agp.AddSegment(agp.prefabBend90);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Finalization", section);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Finalize Pipe System", GUILayout.Width(250)))
        {
            agp.BakeSystem();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndVertical();
    }

    private void DrawPrefabsTab(Tapestry_AssetGeneratorPipes agp)
    {
        GUILayout.BeginVertical("box");
        agp.prefabShort = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("Short Straight", agp.prefabShort, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabMedium = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("Medium Straight", agp.prefabMedium, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabLong = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("Long Straight", agp.prefabLong, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabBend15 = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("15° Bend", agp.prefabBend15, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabBend30 = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("30° Bend", agp.prefabBend30, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabBend45 = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("45° Bend", agp.prefabBend45, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabBend60 = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("60° Bend", agp.prefabBend60, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabBend75 = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("75° Bend", agp.prefabBend75, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        agp.prefabBend90 = (Tapestry_AssetGeneratorPipes_Segment)EditorGUILayout.ObjectField("90° Bend", agp.prefabBend90, typeof(Tapestry_AssetGeneratorPipes_Segment), true);
        GUILayout.EndVertical();
    }
}
