using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_ItemSource))]
public class TapestryInspector_ItemSource : Editor {

    public override void OnInspectorGUI()
    {
        Tapestry_ItemSource i = target as Tapestry_ItemSource;

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            interactableTooltip = "Can the player take this object to their inventory?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        i.displayName = EditorGUILayout.DelayedTextField(i.displayName, GUILayout.Width(270));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        i.isInteractable = EditorGUILayout.Toggle(i.isInteractable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Interactable?", interactableTooltip));
        GUILayout.Space(20);
        if (!i.isInteractable)
        {
            i.displayNameWhenUnactivatable = EditorGUILayout.Toggle(i.displayNameWhenUnactivatable, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Display Name Anyway?", displayNameTooltip));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        string
            itemTooltip = "What item the Entity receives when activating this item source.",
            quantityTooltip = "The minimum and maximum amount of the item that the activating Entity can receive.",
            quantityWeightTooltip = "How much the quantity received is weighted. A linear line indicates no weighting.";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Item", itemTooltip));
        i.item = (Tapestry_Item)EditorGUILayout.ObjectField(i.item, typeof(Tapestry_Item), true, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Quantity", quantityTooltip));
        i.quantityMin = EditorGUILayout.DelayedIntField(i.quantityMin, GUILayout.Width(30));
        GUILayout.Label(new GUIContent("-", displayTooltip),GUILayout.Width(12));
        i.quantityMax = EditorGUILayout.DelayedIntField(i.quantityMax, GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        if (i.quantityMin != i.quantityMax)
        {
            GUILayout.Label(new GUIContent("Weighting", quantityWeightTooltip));
            i.quantityWeightCurve = EditorGUILayout.CurveField(i.quantityWeightCurve, GUILayout.Width(150));
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        string
            automaticRefreshTooltip = "Does this item source become harvestable again automatically?",
            requiresReloadTooltip = "Does this item source require a Level reload before it can be harvested again?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        i.automaticRefresh = EditorGUILayout.Toggle(i.automaticRefresh, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Automatic Refresh?", automaticRefreshTooltip));
        GUILayout.FlexibleSpace();
        i.requiresLevelReload = EditorGUILayout.Toggle(i.requiresLevelReload, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Require Reload?", requiresReloadTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (i.automaticRefresh)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Refresh after");
            i.resetDelay.z = EditorGUILayout.DelayedIntField(i.resetDelay.z, GUILayout.Width(30));
            GUILayout.Label("Days");
            i.resetDelay.y = EditorGUILayout.DelayedIntField(i.resetDelay.y, GUILayout.Width(30));
            GUILayout.Label("Hours");
            i.resetDelay.x = EditorGUILayout.DelayedIntField(i.resetDelay.x, GUILayout.Width(30));
            GUILayout.Label("Minutes");
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
