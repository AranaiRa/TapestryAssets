﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_SwitchContact))]
public class TapestryInspector_SwitchContact : TapestryInspector_Switch {
    
    string keywordToAdd;

    public override void OnInspectorGUI()
    {
        toolbarNames = new string[]{ "States", "Target", "Filters" };

        Tapestry_SwitchContact s = target as Tapestry_SwitchContact;

        if (s.curve == null)
            s.curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            changeTimeTooltip = "The amount of time, in seconds, it takes for the switch to change from on to off, or vice-versa..",
            changeCurveTooltip = "Animation controls for how the switch eases between states.",
            pingPongTooltip = "Does this switch go to it's \"on\" position and then immediately back? Useful for buttons or pressure plates.",
            switchDelayTooltip = "How long, in seconds, the switch holds in the \"on\" position before returning to the \"off\" position.",
            interactableTooltip = "Can the player interact with this door?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?",
            fireOnceTooltip = "Is this switch only allowed to change states once during play?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        s.displayName = EditorGUILayout.DelayedTextField(s.displayName, GUILayout.Width(270));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Change Time", changeTimeTooltip));
        s.switchTime = EditorGUILayout.DelayedFloatField(s.switchTime, GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Change Curve", changeCurveTooltip));
        s.curve = EditorGUILayout.CurveField(s.curve, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        s.isInteractable = EditorGUILayout.Toggle(s.isInteractable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Interactable?", interactableTooltip));
        GUILayout.Space(20);
        if (!s.isInteractable)
        {
            s.displayNameWhenUnactivatable = EditorGUILayout.Toggle(s.displayNameWhenUnactivatable, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Display Name Anyway?", displayNameTooltip));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        s.pingPong = EditorGUILayout.Toggle(s.pingPong, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Ping Pong?", pingPongTooltip));
        GUILayout.FlexibleSpace();
        if (s.pingPong)
        {
            GUILayout.Label(new GUIContent("Switch Delay", switchDelayTooltip));
            s.pingPongHoldTime = EditorGUILayout.DelayedFloatField(s.pingPongHoldTime, GUILayout.Width(36));
            GUILayout.FlexibleSpace();
        }
        s.fireOnlyOnce = EditorGUILayout.Toggle(s.fireOnlyOnce, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Fire Only Once?", fireOnceTooltip));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "States")
                DrawTabStates(s);
            if (toolbarNames[toolbarActive] == "Target")
                DrawTabTarget(s);
            if (toolbarNames[toolbarActive] == "Filters")
                DrawTabFilters(s);
        }
    }

    protected void DrawTabFilters(Tapestry_SwitchContact s)
    {
        DrawSubTabKeywords(s);
    }

    private void DrawSubTabKeywords(Tapestry_SwitchContact s)
    {
        if (ReferenceEquals(s.keywords, null))
            s.keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");

        s.keywords.DrawInspector();
    }
}
