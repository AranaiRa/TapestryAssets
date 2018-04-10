using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Item))]
public class TapestryInspector_Item : Editor
{
    string keywordToAdd;
    int pSel;
    bool startup = true;

    public override void OnInspectorGUI()
    {
        Tapestry_Item i = target as Tapestry_Item;

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

    protected void DrawSubTabItemData(Tapestry_Item i)
    {
        string
            owningEntityTooltip = "What entity, if any, owns this object?",
            owningFactionTooltip = "What faction, if any, owns this object? If a faction owns the object, all Entities within the faction count as owning it.",
            iconTooltip = "What sprite is displayed in HUD elements (such as the inventory screen)",
            sizeTooltip = "What size is this object? Entities can carry as many Negligable sized objects as they like, but larger objects have limitations.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Owner", owningEntityTooltip));
        i.data.owningEntity = (Tapestry_Entity)EditorGUILayout.ObjectField(i.data.owningEntity, typeof(Tapestry_Entity), true, GUILayout.Width(140));
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Faction", owningFactionTooltip));
        i.data.owningFaction = (Tapestry_Faction)EditorGUILayout.ObjectField(i.data.owningFaction, typeof(Tapestry_Faction), true, GUILayout.Width(140));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Icon", iconTooltip));
        i.data.icon = (Sprite)EditorGUILayout.ObjectField(i.data.icon, typeof(Sprite), true, GUILayout.Width(140));
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Size", sizeTooltip));
        i.data.size = (Tapestry_ItemSize)EditorGUILayout.EnumPopup(Tapestry_ItemSize.Negligible, GUILayout.Width(100));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    protected void DrawSubTabEffect(Tapestry_Item i)
    {
        string
            isConsumableTooltip = "Is this object consumable?";

        if(ReferenceEquals(i.data.effect, null))
            i.data.effect = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        i.data.useEffect = EditorGUILayout.Toggle(i.data.useEffect, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Consumable?", isConsumableTooltip));
        GUILayout.EndHorizontal();
        if(i.data.useEffect)
        {
            pSel = i.data.effect.DrawInspector(pSel);
        }

        GUILayout.EndVertical();
    }

    protected void DrawSubTabKeywords(Tapestry_Item i)
    {
        if (i.keywords == null)
            i.keywords = new List<string>();

        int indexToRemove = -1;
        GUILayout.BeginVertical("box");
        GUILayout.Label("Keywords");
        GUILayout.BeginVertical("box");
        if (i.keywords.Count == 0)
        {
            GUILayout.Label("No keywords associated with this Item.");
        }
        else
        {
            for (int j = 0; j < i.keywords.Count; j++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    indexToRemove = j;
                }
                i.keywords[j] = EditorGUILayout.DelayedTextField(i.keywords[j]);
                GUILayout.EndHorizontal();
            }
        }
        if (indexToRemove != -1)
        {
            if (i.keywords.Count == 1)
                i.keywords.Clear();
            else
                i.keywords.RemoveAt(indexToRemove);
        }

        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            if (keywordToAdd != "")
            {
                i.keywords.Add(keywordToAdd);
                keywordToAdd = null;
            }
        }
        keywordToAdd = EditorGUILayout.TextField(keywordToAdd);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}