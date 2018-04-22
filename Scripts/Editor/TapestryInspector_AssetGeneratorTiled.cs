using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_AssetGeneratorTiled))]
public class TapestryInspector_AssetGeneratorTiled : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_AssetGeneratorTiled tag = target as Tapestry_AssetGeneratorTiled;

        string
            objectTTTooltip = "What object to create the tiled array out of. This can be a scene object or a Prefab.",
            boundsTooltip = "How large the object to be tiled is, in meters, horizontally.",
            sizeTooltip = "How large the array is. The first value is along this object's X axis, and the second value is along this object's Z axis.",
            directionTooltip = "What directions new tiles are created in relative to this object.",
            randomRotTooltip = "If true, tiles will be assigned random rotations. Random rotations are always multiples of 90 degrees.",
            outlineTooltip = "If true, only tiles around the outer edge will be generated; interior tiles will be skipped entirely.";

        GUILayout.BeginHorizontal("box");
        GUILayout.Label(new GUIContent("Object to Tile", objectTTTooltip));
        tag.tile = (GameObject)EditorGUILayout.ObjectField(tag.tile, typeof(GameObject), true, GUILayout.Width(120));
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Bounds", boundsTooltip));
        tag.tileSize.x = EditorGUILayout.DelayedFloatField(tag.tileSize.x, GUILayout.Width(32));
        GUILayout.Label("×");
        tag.tileSize.y = EditorGUILayout.DelayedFloatField(tag.tileSize.y, GUILayout.Width(32));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Size of Array", sizeTooltip));
        tag.arraySize.x = EditorGUILayout.DelayedIntField(tag.arraySize.x, GUILayout.Width(32));
        GUILayout.Label("×");
        tag.arraySize.y = EditorGUILayout.DelayedIntField(tag.arraySize.y, GUILayout.Width(32));
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Array Direction", directionTooltip));
        tag.mode = (Tapestry_TileGeneratorModes)EditorGUILayout.EnumPopup(tag.mode, GUILayout.Width(80));

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        tag.randomlyRotateTiles = EditorGUILayout.Toggle(tag.randomlyRotateTiles, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Randomly Rotate?", randomRotTooltip));
        if (tag.randomlyRotateTiles)
        {
            GUILayout.Space(20);
            tag.rotateOnX = EditorGUILayout.Toggle(tag.rotateOnX, GUILayout.Width(12));
            GUILayout.Label("X");
            GUILayout.Space(12);
            tag.rotateOnY = EditorGUILayout.Toggle(tag.rotateOnY, GUILayout.Width(12));
            GUILayout.Label("Y");
            GUILayout.Space(12);
            tag.rotateOnZ = EditorGUILayout.Toggle(tag.rotateOnZ, GUILayout.Width(12));
            GUILayout.Label("Z");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        tag.outline = EditorGUILayout.Toggle(tag.outline, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Outline?", outlineTooltip));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        if (GUILayout.Button("Generate"))
        {
            tag.Clear();
            tag.Generate();
        }
        GUILayout.EndVertical();
    }
}
