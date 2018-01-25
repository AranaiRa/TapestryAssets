using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(Tapestry_Prop))]
public class TapestryInspector_Prop : Editor
{
    int toolbarActive = -1;
    string[] toolbarNames = { "Interaction", "Destruction", "Other" };

    public override void OnInspectorGUI()
    {
        Tapestry_Prop p = target as Tapestry_Prop;

        DrawDefaultInspector();

        GUILayout.Box("BELOW IS CUSTOM INSPECTOR CODE");

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

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Interaction")
            {
                string 
                    pushableTooltip = "Can Entities push this object if one of their " + Tapestry_Config.pushLiftAttribute.ToString() + " is high enough?",
                    liftableTooltip = "Can Entities lift this object if one of their " + Tapestry_Config.pushLiftAttribute.ToString() + " is high enough?",
                    clumTooltip = "The minimum " + Tapestry_Config.pushLiftAttribute.ToString() + " required to push or lift (as appropriate). Uses the Clumsy animation when your Attribute is equal, mixes with the Competent animation as the score increases.",
                    compTooltip = "The " + Tapestry_Config.pushLiftAttribute.ToString() + " score at which the Competent animation is used. Mixes with Clumsy as score goes down, mixes with Impressive as score goes up.",
                    imprTooltip = "The " + Tapestry_Config.pushLiftAttribute.ToString() + " score at which the Impressive animation is used. Mixes with Competent as score goes down, doesn't improve any further as score goes up.";

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

                    string
                        minTooltip = "Minimum movement speed while pushing the object, as a multiplier to the Entity's normal movement speed. (EG: 0.5 means you'll move at half speed while pushing)",
                        maxTooltip = "Maximum movement speed while pushing the object, as a multiplier to the Entity's normal movement speed. (EG: 0.5 means you'll move at half speed while pushing)";

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
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
                    p.pushSpeedCurve = EditorGUILayout.CurveField(p.pushSpeedCurve, GUILayout.Width(150));
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();

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
            if (toolbarNames[toolbarActive] == "Destruction")
            {

            }
            if (toolbarNames[toolbarActive] == "Other")
            {

            }
        }
    }
}
