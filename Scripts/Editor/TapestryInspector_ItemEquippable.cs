using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Tapestry_ItemEquippable))]
public class TapestryInspector_ItemHoldable : TapestryInspector_Item
{
    public override void OnInspectorGUI()
    {
        Tapestry_ItemEquippable i = target as Tapestry_ItemEquippable;
        i.data.isHoldable = true;

        if (startup)
        { 
            if (!ReferenceEquals(i.data.effect, null))
            {
                if (!ReferenceEquals(i.data.effect.payload, null))
                    pSel = ArrayUtility.IndexOf(Tapestry_Config.GetPayloadTypes().Values.ToArray(), i.data.effect.payload.GetType());
            }
            else
                pSel = 0;
            startup = false;
        }

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            interactableTooltip = "Can the player take this object to their inventory?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?",
            valueTooltip = "How valuable is this object?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        i.displayName = EditorGUILayout.DelayedTextField(i.displayName, GUILayout.Width(270));
        i.data.displayName = i.displayName;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        i.isInteractable = EditorGUILayout.Toggle(i.isInteractable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Takeable?", interactableTooltip));
        GUILayout.Space(20);
        if (!i.isInteractable)
        {
            i.displayNameWhenUnactivatable = EditorGUILayout.Toggle(i.displayNameWhenUnactivatable, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Display Name Anyway?", displayNameTooltip));
            GUILayout.FlexibleSpace();
        }
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Value", valueTooltip));
        i.data.value = EditorGUILayout.DelayedIntField(i.data.value, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        DrawSubTabItemData(i);

        DrawSubTabEffect(i);

        DrawSubTabKeywords(i);

        i.data.prefabName = i.transform.name;
    }

    protected override void DrawSubTabEffect(Tapestry_Item i)
    {
        string
            effectOnHoldTooltip = "Does this object apply an Effect to the holder while held?";

        if (ReferenceEquals(i.data.effect, null))
            i.data.effect = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        i.data.useEffect = EditorGUILayout.Toggle(i.data.useEffect, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Apply Effect While Held?", effectOnHoldTooltip));
        GUILayout.EndHorizontal();
        if (i.data.useEffect)
        {
            pSel = i.data.effect.DrawInspector(pSel);
        }

        GUILayout.EndVertical();
    }
}