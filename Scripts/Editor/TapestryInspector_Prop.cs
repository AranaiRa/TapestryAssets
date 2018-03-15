using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(Tapestry_Prop))]
public class TapestryInspector_Prop : Editor
{
    int toolbarActive = -1;
    string[] toolbarNames = { "Interaction", "Destruction", "Other" };
    string keywordToAdd;

    public override void OnInspectorGUI()
    {
        Tapestry_Prop p = target as Tapestry_Prop;

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            interactableTooltip = "Can the player take this object to their inventory?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        p.displayName = EditorGUILayout.DelayedTextField(p.displayName, GUILayout.Width(270));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        p.isInteractable = EditorGUILayout.Toggle(p.isInteractable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Interactable?", interactableTooltip));
        GUILayout.Space(20);
        if (!p.isInteractable)
        {
            p.displayNameWhenUnactivatable = EditorGUILayout.Toggle(p.displayNameWhenUnactivatable, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Display Name Anyway?", displayNameTooltip));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Interaction")
            {
                DrawSubTabPushable(p);
                DrawSubTabLiftable(p);
            }
            if (toolbarNames[toolbarActive] == "Destruction")
            {
                DrawSubTabDestructable(p);
            }
            if (toolbarNames[toolbarActive] == "Other")
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Time");
                GUILayout.BeginHorizontal("box");
                string timeTooltip = "What time scale this entity operates at. 1.0 is normal timp.";

                GUILayout.Label(new GUIContent("Factor", timeTooltip));
                p.personalTimeFactor = EditorGUILayout.DelayedFloatField(p.personalTimeFactor);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                if (p.keywords == null)
                    p.keywords = new List<string>();

                int indexToRemove = -1;
                GUILayout.BeginVertical("box");
                GUILayout.Label("Keywords");
                GUILayout.BeginVertical("box");
                if (p.keywords.Count == 0)
                {
                    GUILayout.Label("No keywords associated with this Entity.");
                }
                else
                {
                    for (int i = 0; i < p.keywords.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            indexToRemove = i;
                        }
                        p.keywords[i] = EditorGUILayout.DelayedTextField(p.keywords[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                if (indexToRemove != -1)
                {
                    if (p.keywords.Count == 1)
                        p.keywords.Clear();
                    else
                        p.keywords.RemoveAt(indexToRemove);
                }

                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    if (keywordToAdd != "")
                    {
                        p.keywords.Add(keywordToAdd);
                        keywordToAdd = null;
                    }
                }
                keywordToAdd = EditorGUILayout.TextField(keywordToAdd);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
        }
    }

    protected void DrawSubTabPushable(Tapestry_Prop p)
    {
        string
            pushableTooltip = "Can Entities push this object if one of their " + Tapestry_Config.pushLiftAttribute.ToString() + " is high enough?",
            clumTooltip = "The minimum " + Tapestry_Config.pushLiftAttribute.ToString() + " required to push or lift (as appropriate). Uses the Clumsy animation when your Attribute is equal, mixes with the Competent animation as the score increases.",
            compTooltip = "The " + Tapestry_Config.pushLiftAttribute.ToString() + " score at which the Competent animation is used. Mixes with Clumsy as score goes down, mixes with Impressive as score goes up.",
            imprTooltip = "The " + Tapestry_Config.pushLiftAttribute.ToString() + " score at which the Impressive animation is used. Mixes with Competent as score goes down, doesn't improve any further as score goes up.",
            gridAlignedTooltip = "Does this object move in predetermined distance increments?",
            lateralTooltip = "Can the entity drag the object sideways relative to their hold position?",
            pullTooltip = "Can the entity pull the object backwards relative to their hold position?",
            forceTPTooltip = "Does binding to this object force the player into a third person camera view?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        p.isPushable = EditorGUILayout.Toggle(p.isPushable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Pushable?", pushableTooltip));
        GUILayout.EndHorizontal();

        if (p.isPushable)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Clumsy:", clumTooltip));
            p.pushClumsy = EditorGUILayout.DelayedIntField(p.pushClumsy, GUILayout.Width(40));

            if (p.pushClumsy > p.pushCompetent) p.pushCompetent = p.pushClumsy;
            if (p.pushClumsy > p.pushImpressive) p.pushImpressive = p.pushClumsy;

            GUILayout.FlexibleSpace();

            GUILayout.Label(new GUIContent("Competent:", compTooltip));
            p.pushCompetent = EditorGUILayout.DelayedIntField(p.pushCompetent, GUILayout.Width(40));

            if (p.pushCompetent < p.pushClumsy) p.pushClumsy = p.pushCompetent;
            if (p.pushCompetent > p.pushImpressive) p.pushImpressive = p.pushCompetent;

            GUILayout.FlexibleSpace();

            GUILayout.Label(new GUIContent("Impressive:", imprTooltip));
            p.pushImpressive = EditorGUILayout.DelayedIntField(p.pushImpressive, GUILayout.Width(40));

            if (p.pushImpressive < p.pushClumsy) p.pushClumsy = p.pushImpressive;
            if (p.pushImpressive < p.pushCompetent) p.pushCompetent = p.pushImpressive;

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            p.gridAligned = EditorGUILayout.Toggle(p.gridAligned, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Grid Aligned?", gridAlignedTooltip));
            GUILayout.FlexibleSpace();

            if (p.gridAligned)
            {
                string
                    minTooltip = "How long, in seconds, it takes to push the object one increment at the Clumsy level.",
                    maxTooltip = "How long, in seconds, it takes to push the object one increment at or above the Impressive level.",
                    gridTooltip = "How far, in meters, the entity moves the object each push.",
                    curveTooltip = "The animation curve used to move the object when pushed.";

                GUILayout.Label("Move Speeds");
                GUILayout.Label(new GUIContent("Slow", minTooltip), GUILayout.Width(30));
                p.pushSpeedMin = EditorGUILayout.FloatField(p.pushSpeedMin, GUILayout.Width(40));
                GUILayout.Label(new GUIContent("Fast", maxTooltip), GUILayout.Width(30));
                p.pushSpeedMax = EditorGUILayout.FloatField(p.pushSpeedMax, GUILayout.Width(40));

                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                GUILayout.Label(new GUIContent("Grid Increment", gridTooltip));
                p.pushIncrement = EditorGUILayout.FloatField(p.pushIncrement, GUILayout.Width(40));

                GUILayout.FlexibleSpace();

                GUILayout.Label(new GUIContent("Curve", curveTooltip));
                p.pushSpeedCurve = EditorGUILayout.CurveField(p.pushSpeedCurve, GUILayout.Width(150));
            }
            else
            {
                string
                    minTooltip = "Minimum movement speed while pushing the object, as a multiplier to the Entity's normal movement speed. (EG: 0.5 means you'll move at half speed while pushing)",
                    maxTooltip = "Maximum movement speed while pushing the object, as a multiplier to the Entity's normal movement speed. (EG: 0.5 means you'll move at half speed while pushing)",
                    curveTooltip = "How the speed values are interpolated as the entity's "+Tapestry_Config.pushLiftAttribute+" increases.";
                
                GUILayout.Label("Move Speeds");
                GUILayout.Label(new GUIContent("Min", minTooltip), GUILayout.Width(30));
                p.pushSpeedMin = EditorGUILayout.FloatField(p.pushSpeedMin, GUILayout.Width(40));
                GUILayout.Label(new GUIContent("Max", maxTooltip), GUILayout.Width(30));
                p.pushSpeedMax = EditorGUILayout.FloatField(p.pushSpeedMax, GUILayout.Width(40));
                GUILayout.EndHorizontal();

                p.pushSpeedMin = Mathf.Clamp(p.pushSpeedMin, 0, 1);
                p.pushSpeedMax = Mathf.Clamp(p.pushSpeedMax, 0, 1);

                GUILayout.BeginHorizontal();
                GUILayout.Space(80);
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("Curve", curveTooltip));
                p.pushSpeedCurve = EditorGUILayout.CurveField(p.pushSpeedCurve, GUILayout.Width(150));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            p.pushForcesThirdPerson = EditorGUILayout.Toggle(p.pushForcesThirdPerson, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Force Third Person?", forceTPTooltip));

            GUILayout.FlexibleSpace();

            p.allowPull = EditorGUILayout.Toggle(p.allowPull, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Allow Pull?", pullTooltip));

            GUILayout.FlexibleSpace();

            p.allowLateral = EditorGUILayout.Toggle(p.allowLateral, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Allow Lateral Drag?", lateralTooltip));

            GUILayout.EndHorizontal();

            string
                pushSoundTooltip = "What sound, if any, plays while the object is being pushed. If this object is grid aligned, the sound triggers every time a push is initiated. If the object is not grid aligned, the sound loops as long as the object is being pushed.",
                collideSoundNeutralTooltip = "What sound, if any, plays when something collides with the object while it is idle.",
                collideSoundPushingTooltip = "What sound, if any, plays when the object collides with something while being pushed.";

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Pushing", pushSoundTooltip));
            p.pushingSound = (AudioClip)EditorGUILayout.ObjectField(p.pushingSound, typeof(AudioClip), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Collision while Idle", collideSoundNeutralTooltip));
            p.pushingSound = (AudioClip)EditorGUILayout.ObjectField(p.pushingSound, typeof(AudioClip), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Collision while Pushing", collideSoundPushingTooltip));
            p.pushingSound = (AudioClip)EditorGUILayout.ObjectField(p.pushingSound, typeof(AudioClip), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }

    protected void DrawSubTabLiftable(Tapestry_Prop p)
    {
        string
            liftableTooltip = "Can Entities lift this object if one of their " + Tapestry_Config.pushLiftAttribute.ToString() + " is high enough?",
            clumTooltip = "The minimum " + Tapestry_Config.pushLiftAttribute.ToString() + " required to push or lift (as appropriate). Uses the Clumsy animation when your Attribute is equal, mixes with the Competent animation as the score increases.",
            compTooltip = "The " + Tapestry_Config.pushLiftAttribute.ToString() + " score at which the Competent animation is used. Mixes with Clumsy as score goes down, mixes with Impressive as score goes up.",
            imprTooltip = "The " + Tapestry_Config.pushLiftAttribute.ToString() + " score at which the Impressive animation is used. Mixes with Competent as score goes down, doesn't improve any further as score goes up.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        p.isLiftable = EditorGUILayout.Toggle(p.isLiftable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Liftable?", liftableTooltip));
        GUILayout.EndHorizontal();

        if (p.isLiftable)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Clumsy:", clumTooltip));
            p.liftClumsy = EditorGUILayout.DelayedIntField(p.liftClumsy, GUILayout.Width(40));

            if (p.liftClumsy > p.liftCompetent) p.liftCompetent = p.liftClumsy;
            if (p.liftClumsy > p.liftImpressive) p.liftImpressive = p.liftClumsy;

            GUILayout.FlexibleSpace();

            GUILayout.Label(new GUIContent("Competent:", compTooltip));
            p.liftCompetent = EditorGUILayout.DelayedIntField(p.liftCompetent, GUILayout.Width(40));

            if (p.liftCompetent < p.liftClumsy) p.liftClumsy = p.liftCompetent;
            if (p.liftCompetent > p.liftImpressive) p.liftImpressive = p.liftCompetent;

            GUILayout.FlexibleSpace();

            GUILayout.Label(new GUIContent("Impressive:", imprTooltip));
            p.liftImpressive = EditorGUILayout.DelayedIntField(p.liftImpressive, GUILayout.Width(40));

            if (p.liftImpressive < p.liftClumsy) p.liftClumsy = p.liftImpressive;
            if (p.liftImpressive < p.liftCompetent) p.liftCompetent = p.liftImpressive;

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();

            string
                minTooltip = "Minimum movement speed while lifting the object, as a multiplier to the Entity's normal movement speed. (EG: 0.5 means you'll move at half speed while lifting)",
                maxTooltip = "Maximum movement speed while lifting the object, as a multiplier to the Entity's normal movement speed. (EG: 0.5 means you'll move at half speed while lifting)";

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Move Speeds");
            GUILayout.Label(new GUIContent("Min", minTooltip), GUILayout.Width(30));
            p.liftSpeedMin = EditorGUILayout.FloatField(p.liftSpeedMin, GUILayout.Width(40));
            GUILayout.Label(new GUIContent("Max", maxTooltip), GUILayout.Width(30));
            p.liftSpeedMax = EditorGUILayout.FloatField(p.liftSpeedMax, GUILayout.Width(40));
            GUILayout.EndHorizontal();

            p.liftSpeedMin = Mathf.Clamp(p.liftSpeedMin, 0, 1);
            p.liftSpeedMax = Mathf.Clamp(p.liftSpeedMax, 0, 1);

            GUILayout.BeginHorizontal();
            GUILayout.Space(80);
            GUILayout.FlexibleSpace();
            p.liftSpeedCurve = EditorGUILayout.CurveField(p.liftSpeedCurve, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }

    protected void DrawSubTabDestructable(Tapestry_Prop p)
    {
        string
            destructableTooltip = "Does this object model swap when it reaches certain damage amounts?",
            intactTooltip = "What GameObjects to display when the object is " + Tapestry_HealthState.Intact.ToString() + ". Use an empty GameObject to contain the parts.",
            brokenTooltip = "What GameObjects to display when the object is " + Tapestry_HealthState.Broken.ToString() + ". Use an empty GameObject to contain the parts.",
            destroTooltip = "What GameObjects to display when the object is " + Tapestry_HealthState.Destroyed.ToString() + ". Use an empty GameObject to contain the parts.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        p.isDestructable = EditorGUILayout.Toggle(p.isDestructable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Destructable?", destructableTooltip));
        GUILayout.EndHorizontal();

        if (p.isDestructable)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("box");
            GUILayout.Label("Health (" + p.GetHealthState() + ")");
            GUILayout.BeginHorizontal();
            p.health = GUILayout.HorizontalSlider(p.health, 0, 1000);
            float.TryParse(GUILayout.TextField(p.health.ToString(), GUILayout.MaxWidth(40)), out p.health);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            string dtTooltip = "Damage Threshold: Any amount of damage sustained below this value will be ignored.";

            GUILayout.BeginVertical("box");
            GUILayout.Label(new GUIContent("Damage Threshold", dtTooltip));
            GUILayout.BeginHorizontal();
            p.threshold = GUILayout.HorizontalSlider(p.threshold, 0, 1000);
            float.TryParse(GUILayout.TextField(p.threshold.ToString(), GUILayout.MaxWidth(40)), out p.threshold);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(Tapestry_HealthState.Intact.ToString(), intactTooltip));
            GUILayout.FlexibleSpace();
            EditorGUILayout.ObjectField(p.intact, typeof(GameObject), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(Tapestry_HealthState.Broken.ToString(), brokenTooltip));
            GUILayout.FlexibleSpace();
            EditorGUILayout.ObjectField(p.broken, typeof(GameObject), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(Tapestry_HealthState.Destroyed.ToString(), destroTooltip));
            GUILayout.FlexibleSpace();
            EditorGUILayout.ObjectField(p.destroyed, typeof(GameObject), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }
}
