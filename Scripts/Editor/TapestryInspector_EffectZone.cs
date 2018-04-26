using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_EffectZone))]
public class TapestryInspector_EffectZone : Editor {

    int pSel, toolbarActive;
    bool startup = true;
    string[] toolbarNames = { "Effect", "Filters" };

    public override void OnInspectorGUI()
    {
        Tapestry_EffectZone e = target as Tapestry_EffectZone;
        
        if (ReferenceEquals(e.effect, null))
            e.effect = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");
        if (ReferenceEquals(e.effect.payload, null))
            e.effect.payload = (Tapestry_EffectBuilder_Payload)ScriptableObject.CreateInstance("Tapestry_EffectBuilder_Payload_Damage");

        if (startup)
        {
            pSel = ArrayUtility.IndexOf(Tapestry_Config.GetPayloadTypes().Values.ToArray(), e.effect.payload.GetType());
            startup = false;
        }

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        e.removeEffectOnTriggerLeave = EditorGUILayout.Toggle(e.removeEffectOnTriggerLeave, GUILayout.Width(12));
        GUILayout.Label("Remove Effect on Trigger Leave?");
        GUILayout.EndHorizontal();

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Effect")
                pSel = e.effect.DrawInspector(pSel);

            else if (toolbarNames[toolbarActive] == "Filters")
                DrawTabFilters(e);
        }

        GUILayout.EndVertical();

        DrawDefaultInspector();
    }

    public void DrawTabFilters(Tapestry_EffectZone e)
    {
        if(ReferenceEquals(e.keywords, null))
            e.keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");

        GUILayout.BeginHorizontal();

        e.applyByKeyword = EditorGUILayout.Toggle(e.applyByKeyword, GUILayout.Width(12));
        GUILayout.Label("Apply by Keyword?");
        GUILayout.FlexibleSpace();
        e.applyByKeyword = !EditorGUILayout.Toggle(!e.applyByKeyword, GUILayout.Width(12));
        GUILayout.Label("Ignore by Keyword?");
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        e.keywords.DrawInspector();
    }
}
