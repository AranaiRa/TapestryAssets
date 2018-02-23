using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Entity))]
public class TapestryInspector_Entity : Editor {

    int toolbarActive = -1;
    string[] toolbarNames = { "Skills", "Resist", "Inventory", "Other"};
    Tapestry_Item itemToAdd;
    string keywordToAdd;

    public override void OnInspectorGUI()
    {
        Tapestry_Entity e = target as Tapestry_Entity;

        //DrawDefaultInspector();

        //GUILayout.Box("BELOW IS CUSTOM INSPECTOR CODE");

        GUILayout.BeginHorizontal();

        //GUI.backgroundColor = new Color(0.9f, 1.0f, 0.9f);
        GUILayout.BeginVertical("box");
        GUILayout.Label("Health (" + e.GetHealthState() + ")");
        GUILayout.BeginHorizontal();
        e.health = GUILayout.HorizontalSlider(e.health, 0, 1000);
        float.TryParse(GUILayout.TextField(e.health.ToString(), GUILayout.MaxWidth(40)), out e.health);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        
        GUILayout.BeginVertical("box");
        GUILayout.Label("Stamina (" + e.GetStaminaState() + ")");
        GUILayout.BeginHorizontal();
        e.stamina = GUILayout.HorizontalSlider(e.stamina, 0, 1000);
        float.TryParse(GUILayout.TextField(e.stamina.ToString(), GUILayout.MaxWidth(40)), out e.stamina);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        
        if (e.attributeProfile == null)
            e.attributeProfile = new Tapestry_AttributeProfile();

        string scoreTooltip = "Score: What the score starts at. Ranges from 0 to 100, but Effect can push above that cap.";
        string progTooltip = "Progress: How close the Entity is to a new rank. Generally only the player or followers are going to make use of this. A new rank occurs at 1000 Progress.";

        GUILayout.BeginHorizontal("box");
        foreach(var v in Enum.GetValues(typeof(Tapestry_Attribute)))
        {
            Tapestry_Attribute val = (Tapestry_Attribute)v;

            GUILayout.BeginVertical("box");
            GUILayout.Label(val.ToString());
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("S", scoreTooltip), GUILayout.Width(12));
            e.attributeProfile.SetScore(val, EditorGUILayout.IntField(e.attributeProfile.GetScore(val), GUILayout.Width(32)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("P", progTooltip), GUILayout.Width(12));
            e.attributeProfile.SetProgress(val, EditorGUILayout.FloatField(e.attributeProfile.GetProgress(val), GUILayout.Width(32)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");

        string asTooltip = "Multiplier to the animation speed of abilities. 1.0 is normal speed, low numbers are faster.";
        string chrTooltip = "Chance that an ability is critically effective. Maximum of 0.25.";
        string msTooltip = "Multiplier to character movement speed. 1.0 is normal speed, high numbers are faster.";
        string pseTooltip = "Cost multiplier to physical actions that cost Stamina. 1.0 is normal cost, low numbers make actions cheaper.";
        string mseTooltip = "Cost multiplier to mental actions that cost Stamina. 1.0 is normal cost, low numbers make actions cheaper.";

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Action Speed", asTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format("{0:0.##}", e.attributeProfile.ActionSpeed*100.0f)+"%");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Critical Hit Rate", chrTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format("{0:0.##}",e.attributeProfile.CriticalHitRate*100.0f)+"%");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Movement Speed", msTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format("{0:0.##}", e.attributeProfile.MovementSpeed * 100.0f) + "%");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Physical Stamina Costs", pseTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format("{0:0.##}", e.attributeProfile.PhysicalStaminaMult * 100.0f) + "%");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Mental Stamina Costs", mseTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format("{0:##}", e.attributeProfile.MentalStaminaMult * 100.0f)+"%");
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Inventory")
            {
                if (e.inventory == null)
                    e.inventory = new Tapestry_Inventory(e.transform);

                int indexToRemove = -1;
                GUILayout.BeginVertical("box");
                GUILayout.Label("Inventory");
                GUILayout.BeginVertical("box");
                if (e.inventory.items.Count == 0)
                    GUILayout.Label("No items in inventory.");
                else
                {
                    for (int i = 0; i < e.inventory.items.Count; i++)
                    {
                        Tapestry_ItemStack stack = e.inventory.items[i];
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            indexToRemove = i;
                        }
                        GUILayout.FlexibleSpace();
                        stack.quantity = EditorGUILayout.DelayedIntField(stack.quantity, GUILayout.Width(36));
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("x", GUILayout.Width(12));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.TextField(stack.item.displayName, GUILayout.Width(300));
                        GUILayout.EndHorizontal();
                    }
                }
                if(indexToRemove != -1)
                {
                    if (e.inventory.items.Count == 1)
                        e.inventory.items.Clear();
                    else
                        e.inventory.items.RemoveAt(indexToRemove);
                }
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("+", GUILayout.Width(20)))
                {
                    if (itemToAdd != null)
                    {
                        e.inventory.AddItem(itemToAdd, 1);
                        itemToAdd = null;
                    }
                }
                itemToAdd = (Tapestry_Item)EditorGUILayout.ObjectField(itemToAdd, typeof(Tapestry_Item), true, GUILayout.Width(300));
                
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Skills")
            {

                if (e.skillProfile == null)
                    e.skillProfile = new Tapestry_SkillProfile();

                GUILayout.BeginVertical("box");

                foreach (var v in Enum.GetValues(typeof(Tapestry_Skill)))
                {
                    Tapestry_Skill val = (Tapestry_Skill)v;

                    GUILayout.BeginHorizontal();

                    GUILayout.Label(val.ToString(), GUILayout.Width(120));

                    GUILayout.FlexibleSpace();

                    GUILayout.Label(new GUIContent("Score:", scoreTooltip));
                    e.skillProfile.SetScore(val,EditorGUILayout.IntField(e.skillProfile.GetScore(val), GUILayout.Width(40)));

                    GUILayout.FlexibleSpace();

                    GUILayout.Label(new GUIContent("Prog:", progTooltip));
                    e.skillProfile.SetProgress(val, EditorGUILayout.FloatField(e.skillProfile.GetProgress(val), GUILayout.Width(40)));

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Resist")
            {
                if (e.damageProfile == null)
                    e.damageProfile = new Tapestry_DamageProfile();
                
                GUILayout.BeginVertical("box");
                
                string resTooltip = "Resistance: All incoming damage of this type is reduced by the listed value (EG: 0.5 will reduce damage by 50%, -0.5 will increase it by 50%).";
                string mitTooltip = "Mitigation: Damage taken subtracts this amount after Resistance is applied.";

                foreach (var v in Enum.GetValues(typeof(Tapestry_DamageType)))
                {
                    Tapestry_DamageType val = (Tapestry_DamageType)v;

                    GUILayout.BeginHorizontal();

                    GUILayout.Label(val.ToString(), GUILayout.Width(70));

                    GUILayout.FlexibleSpace();
                    
                    GUILayout.Label(new GUIContent("RES", resTooltip), GUILayout.Width(30));
                    e.damageProfile.SetRes(val, EditorGUILayout.FloatField(e.damageProfile.GetRes(val), GUILayout.Width(40)));

                    GUILayout.FlexibleSpace();
                    
                    GUILayout.Label(new GUIContent("MIT", mitTooltip), GUILayout.Width(30));
                    e.damageProfile.SetMit(val, EditorGUILayout.FloatField(e.damageProfile.GetMit(val), GUILayout.Width(40)));

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Other")
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Time");
                GUILayout.BeginHorizontal("box");
                string timeTooltip = "What time scale this entity operates at. 1.0 is normal time.";

                GUILayout.Label(new GUIContent("Factor", timeTooltip));
                e.personalTimeFactor = EditorGUILayout.DelayedFloatField(e.personalTimeFactor);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                if (e.keywords == null)
                    e.keywords = new List<string>();

                int indexToRemove = -1;
                GUILayout.BeginVertical("box");
                GUILayout.Label("Keywords");
                GUILayout.BeginVertical("box");
                if (e.keywords.Count == 0)
                {
                    GUILayout.Label("No keywords associated with this Entity.");
                }
                else
                {
                    for (int i = 0; i < e.keywords.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            indexToRemove = i;
                        }
                        e.keywords[i] = EditorGUILayout.DelayedTextField(e.keywords[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                if (indexToRemove != -1)
                {
                    if (e.keywords.Count == 1)
                        e.keywords.Clear();
                    else
                        e.keywords.RemoveAt(indexToRemove);
                }

                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
                if(GUILayout.Button("+", GUILayout.Width(20)))
                {
                    if (keywordToAdd != "")
                    {
                        e.keywords.Add(keywordToAdd);
                        keywordToAdd = null;
                    }
                }
                keywordToAdd = EditorGUILayout.TextField(keywordToAdd);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
        }
    }
}
