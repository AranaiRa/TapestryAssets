using System.Collections;
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
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?";

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
        GUILayout.Label(new GUIContent("Ping Pong?", pingPongTooltip));
        s.pingPong = EditorGUILayout.Toggle(s.pingPong, GUILayout.Width(12));
        GUILayout.FlexibleSpace();
        if (s.pingPong)
        {
            GUILayout.Label(new GUIContent("Switch Delay", switchDelayTooltip));
            s.pingPongHoldTime = EditorGUILayout.DelayedFloatField(s.pingPongHoldTime, GUILayout.Width(36));
            GUILayout.FlexibleSpace();
        }
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
        if (s.keywords == null)
            s.keywords = new List<string>();

        int indexToRemove = -1;
        GUILayout.BeginVertical("box");
        GUILayout.Label("Keywords");
        GUILayout.BeginVertical("box");
        if (s.keywords.Count == 0)
        {
            GUILayout.Label("No keywords associated with this Contact Switch.");
        }
        else
        {
            for (int i = 0; i < s.keywords.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    indexToRemove = i;
                }
                s.keywords[i] = EditorGUILayout.DelayedTextField(s.keywords[i]);
                GUILayout.EndHorizontal();
            }
        }
        if (indexToRemove != -1)
        {
            if (s.keywords.Count == 1)
                s.keywords.Clear();
            else
                s.keywords.RemoveAt(indexToRemove);
        }

        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            if (keywordToAdd != "")
            {
                s.keywords.Add(keywordToAdd);
                keywordToAdd = null;
            }
        }
        keywordToAdd = EditorGUILayout.TextField(keywordToAdd);
        GUILayout.EndHorizontal();

        GUIStyle ww = new GUIStyle();
        ww.wordWrap = true;
        ww.alignment = TextAnchor.UpperCenter;
        GUILayout.Label("If this list is not empty, only objects with one of the provided keywords can activate the Contact Switch.", ww);

        GUILayout.EndVertical();
    }
}
