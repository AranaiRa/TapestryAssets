using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Door))]
public class TapestryInspector_Door : Editor
{

    public override void OnInspectorGUI()
    {
        Tapestry_Door d = target as Tapestry_Door;

        if (d.security == null)
            d.security = new Tapestry_Lock(false, 0, "");

        string lockedTooltip = "Is this container locked?";
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        d.security.isLocked = EditorGUILayout.Toggle(d.security.isLocked, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Locked?", lockedTooltip));
        GUILayout.EndHorizontal();


        if (d.security.isLocked)
        {
            GUILayout.BeginHorizontal("box");

            string 
                bypassableTooltip = "Can the player bypass this lock with "+Tapestry_Config.lockBypassSkill.ToString()+"?",
                levelTooltip = "How difficult this lock is to bypass.",
                keyTooltip = "Entities with a key with this ID can open this door when locked. After passing through the door, the Entity will re-lock it.";

            GUILayout.Label(new GUIContent("Bypassable?",bypassableTooltip));
            d.security.canBeBypassed = EditorGUILayout.Toggle(d.security.canBeBypassed, GUILayout.Width(12));
            GUILayout.FlexibleSpace();
            if (d.security.canBeBypassed)
            {
                GUILayout.Label(new GUIContent("Level", levelTooltip));
                d.security.LockLevel = EditorGUILayout.DelayedIntField(d.security.LockLevel, GUILayout.Width(30));
                GUILayout.FlexibleSpace();
            }
            GUILayout.Label(new GUIContent("Key", keyTooltip));
            d.security.keyID = EditorGUILayout.DelayedTextField(d.security.keyID, GUILayout.Width(100));

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();


        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Change Time:");
        d.openTime = EditorGUILayout.DelayedFloatField(d.openTime, GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Change Curve:");
        d.curve = EditorGUILayout.CurveField(d.curve, GUILayout.Width(150));
        GUILayout.EndHorizontal();


        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Open");
        GUILayout.FlexibleSpace();
        GUILayout.Label(d.GetOpenInspectorString());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Open Door"))
        {
            if (Application.isPlaying)
                d.Open();
            else
                d.Open(true);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Bake Open Pivot Transform"))
        {
            d.BakeOpenState();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();



        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Closed");
        GUILayout.FlexibleSpace();
        GUILayout.Label(d.GetClosedInspectorString());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Close Door"))
        {
            if (Application.isPlaying)
                d.Close();
            else
                d.Close(true);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Bake Closed Pivot Transform"))
        {
            d.BakeClosedState();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        /*GUILayout.BeginHorizontal();
        
        GUILayout.FlexibleSpace();
        GUILayout.Label(d.pos2.ToString());
        GUILayout.Label(d.rot2.ToString());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Bake Closed Transform"))
        {
            d.BakeClosedState();
        }
        GUILayout.FlexibleSpace();
        GUILayout.Label(d.pos1.ToString());
        GUILayout.Label(d.rot1.ToString());
        GUILayout.EndHorizontal();

        

        if(GUILayout.Button("Close"))
        {
            if (Application.isPlaying)
                d.Close();
            else
                d.Close(true);
        }*/
    }
}
