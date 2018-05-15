using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Tapestry_EffectBuilder_Payload_Purge : Tapestry_EffectBuilder_Payload
{
    Tapestry_KeywordRegistry keywords;
    public bool
        needsOneKeyword = true;

    public Tapestry_EffectBuilder_Payload_Purge()
    {
        mustBeInstant = true;
    }

    private void OnEnable()
    {
        keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");
    }

    public override void Apply(Tapestry_Actor target)
    {
        for(int i=target.effects.Count-1; i>=0; i--)
        {
            Tapestry_Effect e = target.effects[i];
            if (e.payload.GetType() != typeof(Tapestry_EffectBuilder_Payload_Purge))
            {
                if (needsOneKeyword)
                {
                    if (e.keywords.ContainsOne(keywords))
                        e.readyForRemoval = true;
                }
                else
                {
                    if (e.keywords.ContainsAll(keywords))
                        e.readyForRemoval = true;
                }
            }
        }
    }
    
    #if UNITY_EDITOR
    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        needsOneKeyword = EditorGUILayout.Toggle(needsOneKeyword, GUILayout.Width(12));
        GUILayout.Label("Needs One Keyword?");
        GUILayout.FlexibleSpace();
        needsOneKeyword = EditorGUILayout.Toggle(!needsOneKeyword, GUILayout.Width(12));
        GUILayout.Label("Needs All Keywords?");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        keywords.DrawInspector();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    #endif
}
