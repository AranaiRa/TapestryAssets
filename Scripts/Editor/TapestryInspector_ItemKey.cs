using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Tapestry_ItemKey))]
public class TapestryInspector_ItemKey : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_Item i = target as Tapestry_ItemKey;

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            interactableTooltip = "Can the player take this object to their inventory?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?",
            valueTooltip = "How valuable is this object?",
            keyIDTooltip = "What ID this key has. If this string matches the lock ID of a door, an entity can unlock it if this key is in their inventory.";

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

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Key ID", keyIDTooltip));
        i.data.keyID = EditorGUILayout.DelayedTextField(i.data.keyID);
        i.data.isKey = true;
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        
        i.data.prefabName = i.transform.name;
    }
}
