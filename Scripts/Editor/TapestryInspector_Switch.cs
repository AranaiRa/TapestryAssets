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

        string targetTooltip = "What object, if any, this switch affects. Modifyable controls will appear based on what Tapestry components are detected.";

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Target Object", targetTooltip));
        s.target = (GameObject)EditorGUILayout.ObjectField(s.target, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        if (s.target != null)
        {
            Tapestry_Activatable comp = s.target.GetComponent<Tapestry_Activatable>();
            if(comp.GetType() == typeof(Tapestry_Door))
            {
                string
                    doorTooltip = "Detected component type is \"Door\".";

                GUILayout.Label(new GUIContent("Door Controls", doorTooltip));
                GUILayout.BeginVertical("box");
                if(s.pingPong)
                {
                    GUILayout.Label("When Switch is Activated...");

                    GUILayout.BeginHorizontal();
                    s.data.pp_swapOpenState = EditorGUILayout.Toggle(s.data.pp_swapOpenState, GUILayout.Width(14));
                    GUILayout.Label("Open/Close");
                    GUILayout.FlexibleSpace();
                    s.data.pp_swapInteractivityState = EditorGUILayout.Toggle(s.data.pp_swapInteractivityState, GUILayout.Width(14));
                    GUILayout.Label("Interactivity");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    s.data.pp_swapLockedState = EditorGUILayout.Toggle(s.data.pp_swapLockedState, GUILayout.Width(14));
                    GUILayout.Label("Lock/Unlock");
                    GUILayout.FlexibleSpace();
                    s.data.pp_swapBypassableState = EditorGUILayout.Toggle(s.data.pp_swapBypassableState, GUILayout.Width(14));
                    GUILayout.Label("Bypassability");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("When Switch is On...");

                    GUILayout.BeginHorizontal();
                    s.data.on_setOpen = EditorGUILayout.Toggle(s.data.on_setOpen, GUILayout.Width(14));
                    GUILayout.Label("Open", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.on_setClosed = EditorGUILayout.Toggle(s.data.on_setClosed, GUILayout.Width(14));
                    GUILayout.Label("Close", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.on_setInteractable = EditorGUILayout.Toggle(s.data.on_setInteractable, GUILayout.Width(14));
                    GUILayout.Label("Interactive", GUILayout.Width(72));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    s.data.on_setLocked = EditorGUILayout.Toggle(s.data.on_setLocked, GUILayout.Width(14));
                    GUILayout.Label("Lock", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.on_setUnlocked = EditorGUILayout.Toggle(s.data.on_setUnlocked, GUILayout.Width(14));
                    GUILayout.Label("Unlock", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.on_setBypassable = EditorGUILayout.Toggle(s.data.on_setBypassable, GUILayout.Width(14));
                    GUILayout.Label("Bypassable", GUILayout.Width(72));
                    GUILayout.EndHorizontal();

                    GUILayout.Label("When Switch is Off...");

                    GUILayout.BeginHorizontal();
                    s.data.off_setOpen = EditorGUILayout.Toggle(s.data.off_setOpen, GUILayout.Width(14));
                    GUILayout.Label("Open", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.off_setClosed = EditorGUILayout.Toggle(s.data.off_setClosed, GUILayout.Width(14));
                    GUILayout.Label("Close", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.off_setInteractable = EditorGUILayout.Toggle(s.data.off_setInteractable, GUILayout.Width(14));
                    GUILayout.Label("Interactive", GUILayout.Width(72));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    s.data.off_setLocked = EditorGUILayout.Toggle(s.data.off_setLocked, GUILayout.Width(14));
                    GUILayout.Label("Lock", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.off_setUnlocked = EditorGUILayout.Toggle(s.data.off_setUnlocked, GUILayout.Width(14));
                    GUILayout.Label("Unlock", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.off_setBypassable = EditorGUILayout.Toggle(s.data.off_setBypassable, GUILayout.Width(14));
                    GUILayout.Label("Bypassable", GUILayout.Width(72));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            if(comp.GetType() == typeof(Tapestry_AnimatedLight))
            {
                string
                    animLightTooltip = "Detected component type is \"Animated Light\".";

                GUILayout.Label(new GUIContent("Animated Light Controls", animLightTooltip));
                GUILayout.BeginVertical("box");
                if (s.pingPong)
                {
                    GUILayout.Label("When Switch is Activated...");

                    GUILayout.BeginHorizontal();
                    s.data.pp_swapOpenState = EditorGUILayout.Toggle(s.data.pp_swapOpenState, GUILayout.Width(14));
                    GUILayout.Label("Turn On/Turn Off");
                    GUILayout.FlexibleSpace();
                    s.data.pp_swapInteractivityState = EditorGUILayout.Toggle(s.data.pp_swapInteractivityState, GUILayout.Width(14));
                    GUILayout.Label("Interactivity");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("When Switch is On...");

                    GUILayout.BeginHorizontal();
                    s.data.on_setOpen = EditorGUILayout.Toggle(s.data.on_setOpen, GUILayout.Width(14));
                    GUILayout.Label("Turn On", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.on_setClosed = EditorGUILayout.Toggle(s.data.on_setClosed, GUILayout.Width(14));
                    GUILayout.Label("Turn Off", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.on_setInteractable = EditorGUILayout.Toggle(s.data.on_setInteractable, GUILayout.Width(14));
                    GUILayout.Label("Interactive", GUILayout.Width(72));
                    GUILayout.EndHorizontal();

                    GUILayout.Label("When Switch is Off...");

                    GUILayout.BeginHorizontal();
                    s.data.off_setOpen = EditorGUILayout.Toggle(s.data.off_setOpen, GUILayout.Width(14));
                    GUILayout.Label("Turn On", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.off_setClosed = EditorGUILayout.Toggle(s.data.off_setClosed, GUILayout.Width(14));
                    GUILayout.Label("Turn Off", GUILayout.Width(72));
                    GUILayout.FlexibleSpace();
                    s.data.off_setInteractable = EditorGUILayout.Toggle(s.data.off_setInteractable, GUILayout.Width(14));
                    GUILayout.Label("Interactive", GUILayout.Width(72));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            if(comp.GetType() == typeof(Tapestry_ItemSource))
            {
                string
                    animLightTooltip = "Detected component type is \"Item Source\".";

                GUILayout.Label(new GUIContent("Item Source Controls", animLightTooltip));
                GUILayout.BeginVertical("box");
                if (s.pingPong)
                {
                    GUILayout.Label("When Switch is Activated...");
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    s.data.pp_setSourceHarvestable = EditorGUILayout.Toggle(s.data.pp_setSourceHarvestable, GUILayout.Width(14));
                    GUILayout.Label("Harvestable");
                    GUILayout.FlexibleSpace();
                    s.data.pp_swapInteractivityState = EditorGUILayout.Toggle(s.data.pp_swapInteractivityState, GUILayout.Width(14));
                    GUILayout.Label("Interactivity");
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("When Switch is On...");

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    s.data.on_setSourceHarvestable = EditorGUILayout.Toggle(s.data.on_setSourceHarvestable, GUILayout.Width(14));
                    GUILayout.Label("Harvestability");
                    GUILayout.FlexibleSpace();
                    s.data.on_setInteractable = EditorGUILayout.Toggle(s.data.on_setInteractable, GUILayout.Width(14));
                    GUILayout.Label("Interactive", GUILayout.Width(72));
                    GUILayout.EndHorizontal();

                    GUILayout.Label("When Switch is Off...");

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    s.data.off_setSourceHarvestable = EditorGUILayout.Toggle(s.data.off_setSourceHarvestable, GUILayout.Width(14));
                    GUILayout.Label("Harvestability");
                    GUILayout.FlexibleSpace();
                    s.data.off_setInteractable = EditorGUILayout.Toggle(s.data.off_setInteractable, GUILayout.Width(14));
                    GUILayout.Label("Interactive", GUILayout.Width(72));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();
    }

    private string[] GetTabs(List<string> list)
    {
        string[] output = new string[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            output[i] = list[i];
        }
        return output;
    }
}
