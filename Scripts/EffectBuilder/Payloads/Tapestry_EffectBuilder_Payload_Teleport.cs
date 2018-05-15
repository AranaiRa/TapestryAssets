using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_Teleport : Tapestry_EffectBuilder_Payload
{
    public Vector3
        newPos;
    public float
        newRot;
    public bool
        localPosOffset = false,
        changeRot = false;

    public Tapestry_EffectBuilder_Payload_Teleport()
    {
        mustBeInstant = true;
        newPos = new Vector3();
        newRot = 0;
    }

    public override void Apply(Tapestry_Actor target)
    {
        if (localPosOffset)
            target.transform.position = target.transform.position + newPos;
        else
            target.transform.position = newPos;

        if (changeRot)
            target.transform.rotation = Quaternion.Euler(0, newRot, 0);
    }

    #if UNITY_EDITOR
    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Position");
        GUILayout.Space(4);
        GUILayout.Label("X");
        newPos.x = EditorGUILayout.DelayedFloatField(newPos.x, GUILayout.Width(64));
        GUILayout.Label("Y");
        newPos.y = EditorGUILayout.DelayedFloatField(newPos.y, GUILayout.Width(64));
        GUILayout.Label("Z");
        newPos.z = EditorGUILayout.DelayedFloatField(newPos.z, GUILayout.Width(64));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        localPosOffset = !EditorGUILayout.Toggle(!localPosOffset, GUILayout.Width(12));
        GUILayout.Label("Use Global Coordinates?");
        GUILayout.FlexibleSpace();
        localPosOffset = EditorGUILayout.Toggle(localPosOffset, GUILayout.Width(12));
        GUILayout.Label("Use Local Offset?");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        changeRot = EditorGUILayout.Toggle(changeRot, GUILayout.Width(12));
        GUILayout.Label("Change Facing?");
        GUILayout.FlexibleSpace();
        if (changeRot)
        {
            GUILayout.Label("New Y Rotation");
            newRot = EditorGUILayout.DelayedFloatField(newRot, GUILayout.Width(32));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    #endif
}
