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
        if (d.curve == null)
            d.curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));

        string 
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            changeTimeTooltip = "The amount of time, in seconds, it takes for the door to open or close.",
            changeCurveTooltip = "Animation controls for how the door eases between states.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        d.displayName = EditorGUILayout.DelayedTextField(d.displayName,GUILayout.Width(270));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Change Time", changeTimeTooltip));
        d.openTime = EditorGUILayout.DelayedFloatField(d.openTime, GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Change Curve", changeCurveTooltip));
        d.curve = EditorGUILayout.CurveField(d.curve, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        string lockedTooltip = "Is this container locked?";
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        d.security.isLocked = EditorGUILayout.Toggle(d.security.isLocked, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Locked?", lockedTooltip));
        GUILayout.EndHorizontal();

        if (d.security == null)
            d.security = new Tapestry_Lock(false, 0, "");

        if (d.security.isLocked)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();

            string 
                bypassableTooltip = "Can the player bypass this lock with "+Tapestry_Config.lockBypassSkill.ToString()+"?",
                levelTooltip = "How difficult this lock is to bypass.",
                keyTooltip = "Entities with a key with this ID can open this door when locked. After passing through the door, the Entity will re-lock it.",
                lockedJiggleTooltip = "If this door is locked, does it jiggle when activated?",
                jiggleIntensityTooltip = "How much this door jiggles on activation when locked. This is a percentage of the difference between the closed state and the open state.",
                lockedSoundTooltip = "The sound that plays when the door is unsuccessfully opened when locked, if any.";

            d.security.canBeBypassed = EditorGUILayout.Toggle(d.security.canBeBypassed, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Bypassable?",bypassableTooltip));
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
            GUILayout.BeginHorizontal();

            d.jiggleOnActivateWhenLocked = EditorGUILayout.Toggle(d.jiggleOnActivateWhenLocked, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Jiggle?", lockedJiggleTooltip));
            if (d.jiggleOnActivateWhenLocked)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("Intensity", jiggleIntensityTooltip));
                d.lockJiggleIntensity = EditorGUILayout.DelayedFloatField(d.lockJiggleIntensity, GUILayout.Width(40));
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("Sound", lockedSoundTooltip));
                d.lockedSound = (AudioClip)EditorGUILayout.ObjectField(d.lockedSound, typeof(AudioClip), true, GUILayout.Width(120));
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
        
        string
            openTransformTooltip = "The transform data for the door's open state. Don't worry about the actual numbers too much, but if they're the same as the closed values, you need to bake your open and closed states.",
            closedTransformTooltip = "The transform data for the door's closed state. Don't worry about the actual numbers too much, but if they're the same as the closed values, you need to bake your open and closed states.",
            openSoundTooltip = "The sound to play when the door is opened, if any.",
            closedSoundTooltip = "The sound to play when the door is closed, if any.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Open");
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent(d.GetOpenInspectorString(), openTransformTooltip));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
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

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Sound", openSoundTooltip));
        d.openSound = (AudioClip)EditorGUILayout.ObjectField(d.openSound, typeof(AudioClip), true, GUILayout.Width(250));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndVertical();



        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Closed");
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent(d.GetClosedInspectorString(), closedTransformTooltip));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
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

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Sound", closedSoundTooltip));
        d.closeSound = (AudioClip)EditorGUILayout.ObjectField(d.closeSound, typeof(AudioClip), true, GUILayout.Width(250));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndVertical();
    }
}
