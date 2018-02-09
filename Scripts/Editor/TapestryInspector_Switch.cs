using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Switch))]
public class TapestryInspector_Switch : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_Switch s = target as Tapestry_Switch;

        if (s.curve == null)
            s.curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            changeTimeTooltip = "The amount of time, in seconds, it takes for the switch to change from on to off, or vice-versa..",
            changeCurveTooltip = "Animation controls for how the switch eases between states.",
            pingPongTooltip = "Does this switch go to it's \"on\" position and then immediately back? Useful for buttons or pressure plates.",
            switchDelayTooltip = "How long, in seconds, the switch holds in the \"on\" position before returning to the \"off\" position.";

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

        string
            openTransformTooltip = "The transform data for the switch's on state. Don't worry about the actual numbers too much, but if they're the same as the closed values, you need to bake your open and closed states.",
            closedTransformTooltip = "The transform data for the switch's off state. Don't worry about the actual numbers too much, but if they're the same as the closed values, you need to bake your open and closed states.",
            openSoundTooltip = "The sound to play when the switch turns on, if any.",
            closedSoundTooltip = "The sound to play when the switch turns off, if any.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("On");
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent(s.GetOpenInspectorString(), openTransformTooltip));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Switch On"))
        {
            if (Application.isPlaying)
                s.SwitchOn();
            else
                s.SwitchOn(true);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Bake On Pivot Transform"))
        {
            s.BakeOpenState();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Sound", openSoundTooltip));
        s.onSound = (AudioClip)EditorGUILayout.ObjectField(s.onSound, typeof(AudioClip), true, GUILayout.Width(250));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndVertical();



        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Off");
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent(s.GetClosedInspectorString(), closedTransformTooltip));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Switch Off"))
        {
            if (Application.isPlaying)
                s.SwitchOff();
            else
                s.SwitchOff(true);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Bake Off Pivot Transform"))
        {
            s.BakeClosedState();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Sound", closedSoundTooltip));
        s.offSound = (AudioClip)EditorGUILayout.ObjectField(s.offSound, typeof(AudioClip), true, GUILayout.Width(250));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndVertical();

        string targetTooltip = "What object, if any, this switch affects. Modifyable controls will appear based on what Tapestry components are detected, including in children objects.";

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Target Object", targetTooltip));
        s.target = (GameObject)EditorGUILayout.ObjectField(s.target, typeof(GameObject), true);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
