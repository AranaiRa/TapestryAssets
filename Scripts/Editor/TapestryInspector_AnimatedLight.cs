using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_AnimatedLight))]
public class TapestryInspector_AnimatedLight : Editor
{
    string[] toolbarNames = { "Jitter", "Particles", "Sound", "Timing" };
    int toolbarActive = -1;

    public override void OnInspectorGUI()
    {
        Tapestry_AnimatedLight l = target as Tapestry_AnimatedLight;

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            changeTimeTooltip = "The amount of time, in seconds, it takes for the door to open or close.",
            interactableTooltip = "Can the player interact with this door?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?",
            toggleOnActivateTooltip = "Should the light switch between being on or off when the player activates it?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        l.displayName = EditorGUILayout.DelayedTextField(l.displayName, GUILayout.Width(270));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Change Time", changeTimeTooltip));
        l.transitionSpeed = EditorGUILayout.DelayedFloatField(l.transitionSpeed, GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Turn On", GUILayout.Width(70)))
        {
            if (Application.isPlaying)
                l.TurnOn();
            else
                l.TurnOn(true);
        }
        if (GUILayout.Button("Turn Off", GUILayout.Width(70)))
        {
            if (Application.isPlaying)
                l.TurnOff();
            else
                l.TurnOff(true);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        l.isInteractable = EditorGUILayout.Toggle(l.isInteractable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Interactable?", interactableTooltip));
        GUILayout.Space(20);
        if (!l.isInteractable)
        {
            l.displayNameWhenUnactivatable = EditorGUILayout.Toggle(l.displayNameWhenUnactivatable, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Display Name Anyway?", displayNameTooltip));
            GUILayout.FlexibleSpace();
        }
        else
        {
            l.toggleOnActivate = EditorGUILayout.Toggle(l.toggleOnActivate, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Toggle on Activate?", toggleOnActivateTooltip));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Jitter")
            {
                string
                    lintJitterTooltip = "Does the intensity of the light randomly change over time?",
                    lposJitterTooltip = "Does the position of the light randomly change over time?",
                    emissionJitterTooltip = "Does this light contain something that has emission that changes randomly over time?";

                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                l.lightIntensityJitter = EditorGUILayout.Toggle(l.lightIntensityJitter, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Light Intensity Jitter?", lintJitterTooltip));
                GUILayout.EndHorizontal();

                if (l.lightIntensityJitter)
                {
                    string
                        lintBaseTooltip = "The base intensity of the light. This will override any changes made to the Intensity parameter of the Light component on T_Light; if you intend to use jitter, set the value here instead.",
                        lvarTooltip = "How far the intensity can deviate from its base intensity.",
                        lspeedTooltip = "How long, in seconds, it takes to reach the jitter target at maximum. (Minimum is 40% of this value)";

                    GUILayout.BeginVertical("box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Intensity Variance", lvarTooltip));
                    l.lightIntensityJitterAmount = EditorGUILayout.DelayedFloatField(l.lightIntensityJitterAmount, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(new GUIContent("Jitter Speed", lspeedTooltip));
                    l.lightJitterSpeed = EditorGUILayout.DelayedFloatField(l.lightJitterSpeed, GUILayout.Width(40));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Base Intensity", lintBaseTooltip));
                    l.lightIntensityBase = EditorGUILayout.DelayedFloatField(l.lightIntensityBase, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                l.lightPositionJitter = EditorGUILayout.Toggle(l.lightPositionJitter, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Light Position Jitter?", lposJitterTooltip));
                GUILayout.EndHorizontal();

                if (l.lightPositionJitter)
                {
                    string
                        ldistTooltip = "How far, in Unity units, the light can deviate from its starting position.",
                        lspeedTooltip = "How long, in seconds, it takes to reach the jitter target at maximum. (Minimum is 40% of this value)";

                    GUILayout.BeginVertical("box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Position Variance", ldistTooltip));
                    l.positionJitterAmount = EditorGUILayout.DelayedFloatField(l.positionJitterAmount, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(new GUIContent("Jitter Speed", lspeedTooltip));
                    l.positionJitterSpeed = EditorGUILayout.DelayedFloatField(l.positionJitterSpeed, GUILayout.Width(40));
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                l.emissionJitter = EditorGUILayout.Toggle(l.emissionJitter, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Emission Jitter?", emissionJitterTooltip));
                GUILayout.EndHorizontal();

                if (l.emissionJitter)
                {
                    string
                        erangeTooltip = "The minimum and maximum emission values that the object will choose between randomly. Note that values above 1.0 will clamp to 1.0 unless the player has HDR enabled.",
                        ecolTooltip = "What color is multiplied against the emissive map.",
                        espeedTooltip = "How long, in seconds, it takes to reach the jitter target at maximum. (Minimum is 40% of this value)";

                    GUILayout.BeginVertical("box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Emission Range", erangeTooltip));
                    l.emissionMin = EditorGUILayout.DelayedFloatField(l.emissionMin, GUILayout.Width(40));
                    l.emissionMax = EditorGUILayout.DelayedFloatField(l.emissionMax, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(new GUIContent("Jitter Speed", espeedTooltip));
                    l.emissionJitterSpeed = EditorGUILayout.DelayedFloatField(l.emissionJitterSpeed, GUILayout.Width(40));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Emission Color", ecolTooltip));
                    l.emissionColor = EditorGUILayout.ColorField(l.emissionColor, GUILayout.Width(80));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Particles")
            {
                string
                    inTooltip = "Whether to use the particle system attached to T_PS_In. If true, plays the system when the light is turned on.",
                    activeTooltip = "Whether to use the particle system attached to T_PS_Active. If true, plays the system when the light is on, and shuts off the system when the light is off.",
                    outTooltip = "Whether to use the particle system attached to T_PS_Out. If true, plays the system when the light is turned off.";
                
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                l.useInPsys = EditorGUILayout.Toggle(l.useInPsys, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Use 'In'?", inTooltip));
                GUILayout.FlexibleSpace();
                l.useHoldPsys = EditorGUILayout.Toggle(l.useHoldPsys, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Use 'Active'?", activeTooltip));
                GUILayout.FlexibleSpace();
                l.useOutPsys = EditorGUILayout.Toggle(l.useOutPsys, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Use 'Out'?", outTooltip));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Sound")
            {
                string
                    inSoundTooltip = "The sound to play when the light is turned on.",
                    activeSoundTooltip = "The sound to play while the light is on. This soundclip is looped for as long as the light stays on.",
                    outSoundTooltip = "The sound to play when the light is turned off.";

                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("'In' Sound", inSoundTooltip));
                l.inSound = (AudioClip)EditorGUILayout.ObjectField(l.inSound, typeof(AudioClip), true, GUILayout.Width(240));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("'Active' Sound", activeSoundTooltip));
                l.activeSound = (AudioClip)EditorGUILayout.ObjectField(l.activeSound, typeof(AudioClip), true, GUILayout.Width(240));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("'Out' Sound", outSoundTooltip));
                l.outSound = (AudioClip)EditorGUILayout.ObjectField(l.outSound, typeof(AudioClip), true, GUILayout.Width(240));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Timing")
            {
                string
                    clockLightTooltip = "Does this light turn itself on and off at specific times of day? Note: Enabling this option will disable the Timed Light controls.",
                    timedLightTooltip = "Does this light switch between on and off after a certain amount of time elapses? Note: Enabling this option will disable the Clock Light controls.",
                    durationTooltip = "How long the light remains on (or off, if inverted) before switching to the other state.",
                    invertTooltip = "By default, a timed light will stay on for a certain time before turning itself off. Setting this to true reverses this behavior.",
                    onTimeTooltip = "What time of day the light turns itself on. Please note, at extremely high time speeds, the light may miss its cue to change.",
                    offTimeTooltip = "What time of day the light turns itself off. Please note, at extremely high time speeds, the light may miss its cue to change.";

                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                l.timedLight = EditorGUILayout.Toggle(l.timedLight, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Timed Light?", timedLightTooltip));
                GUILayout.FlexibleSpace();
                l.clockLight = EditorGUILayout.Toggle(l.clockLight, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Clock Light?", clockLightTooltip));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if(l.timedLight)
                {
                    l.clockLight = false;

                    GUILayout.BeginVertical("box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Duration", durationTooltip));
                    l.timedDuration = EditorGUILayout.DelayedFloatField(l.timedDuration, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                    l.invertTimed = EditorGUILayout.Toggle(l.invertTimed, GUILayout.Width(12));
                    GUILayout.Label(new GUIContent("Invert?", invertTooltip));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                }

                if (l.clockLight)
                {
                    l.timedLight = false;

                    GUILayout.BeginVertical("box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("On Time", onTimeTooltip));
                    l.timedOn.x = EditorGUILayout.DelayedIntField(l.timedOn.x, GUILayout.Width(24));
                    GUILayout.Label(":");
                    l.timedOn.y = EditorGUILayout.DelayedIntField(l.timedOn.y, GUILayout.Width(24));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(new GUIContent("Off Time", offTimeTooltip));
                    l.timedOff.x = EditorGUILayout.DelayedIntField(l.timedOff.x, GUILayout.Width(24));
                    GUILayout.Label(":");
                    l.timedOff.y = EditorGUILayout.DelayedIntField(l.timedOff.y, GUILayout.Width(24));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                }

                GUILayout.EndVertical();
            }
        }
    }
}
