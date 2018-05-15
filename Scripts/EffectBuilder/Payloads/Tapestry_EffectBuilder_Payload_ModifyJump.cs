using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_ModifyJump : Tapestry_EffectBuilder_Payload {

    public float amount;
    public string id;
    public Tapestry_BonusType type;
    public bool
        overwrite,
        overwriteByMag,
        removeWhenEffectEnds;

    public Tapestry_EffectBuilder_Payload_ModifyJump()
    {
        amount = 0.10f;
        id = "Generic";
        type = Tapestry_BonusType.AdditiveBonus;
        overwrite = false;
        overwriteByMag = true;
        removeWhenEffectEnds = true;
    }

    public override void Apply(Tapestry_Actor target)
    {
        if(target.GetType() == typeof(Tapestry_Player))
        {
            Tapestry_Player p = target as Tapestry_Player;
            p.jumpPower.AddBonus(amount, id, type, overwrite, overwriteByMag);
        }
    }

    public override void Cleanup(Tapestry_Actor target)
    {
        if (removeWhenEffectEnds)
        {
            if (target.GetType() == typeof(Tapestry_Player))
            {
                Tapestry_Player p = target as Tapestry_Player;
                p.jumpPower.RemoveBonus(id, type);
            }
        }
    }

    #if UNITY_EDITOR
    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Amount");
        amount = EditorGUILayout.DelayedFloatField(amount, GUILayout.Width(42));
        GUILayout.FlexibleSpace();
        GUILayout.Label("ID");
        id = EditorGUILayout.DelayedTextField(id, GUILayout.Width(60));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Type");
        type = (Tapestry_BonusType)EditorGUILayout.EnumPopup(type, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        overwrite = EditorGUILayout.Toggle(overwrite, GUILayout.Width(12));
        GUILayout.Label("Overwrite Same ID?");
        GUILayout.FlexibleSpace();
        if(overwrite)
        {
            overwriteByMag = EditorGUILayout.Toggle(overwriteByMag, GUILayout.Width(12));
            GUILayout.Label("Only if magnitude is greater?");
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        removeWhenEffectEnds = EditorGUILayout.Toggle(removeWhenEffectEnds, GUILayout.Width(12));
        GUILayout.Label("Remove when Effect ends?");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    #endif
}
