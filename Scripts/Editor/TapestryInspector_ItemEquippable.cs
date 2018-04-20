using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Tapestry_ItemEquippable))]
public class TapestryInspector_ItemHoldable : TapestryInspector_Item
{
    List<string> slotEnum;
    string slotEnumSelection;

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
            slotEnum = new List<string>();
            foreach(Tapestry_EquipSlot es in System.Enum.GetValues(typeof(Tapestry_EquipSlot)))
            {
                if (es.ToString() != "LeftHand" && es.ToString() != "RightHand")
                {
                    slotEnum.Add(es.ToString());
                }
            }

            startup = false;
        }

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            prefabTooltip = "What is the name of the prefab (placed in the Resources/Items folder) that should be instantiated when this object is dropped in world?\n\nGenerally this should be the object's in-world name, rather than its display name.",
            interactableTooltip = "Can the player take this object to their inventory?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?",
            valueTooltip = "How valuable is this object?",
            slotTooltip = "What slot should this object go into when equipped?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        i.displayName = EditorGUILayout.DelayedTextField(i.displayName, GUILayout.Width(270));
        i.data.displayName = i.displayName;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Prefab Name", prefabTooltip));
        GUILayout.FlexibleSpace();
        i.data.prefabName = EditorGUILayout.DelayedTextField(i.data.prefabName, GUILayout.Width(270));
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

        slotEnumSelection = i.data.slot.ToString();
        int enumIndex = slotEnum.IndexOf(slotEnumSelection);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Slot", slotTooltip), GUILayout.Width(42));
        slotEnumSelection = slotEnum[EditorGUILayout.Popup(enumIndex, slotEnum.ToArray(), GUILayout.Width(122))];
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        i.data.slot = (Tapestry_EquipSlot)System.Enum.Parse(typeof(Tapestry_EquipSlot), slotEnumSelection);

        GUILayout.EndVertical();

        DrawSubTabItemData(i);

        DrawSubTabEffect(i);

        DrawSubTabKeywords(i);
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