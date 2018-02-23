using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Container))]
public class TapestryInspector_Container : Editor {

    int toolbarActive = -1;
    string[] toolbarNames = { "Interaction", "Destruction", "Inventory", "Other" };
    Tapestry_Item itemToAdd;
    string keywordToAdd;

    public override void OnInspectorGUI()
    {
        Tapestry_Container p = target as Tapestry_Container;

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

        if (p.security == null)
            p.security = new Tapestry_Lock(false, 0, "");

        string lockedTooltip = "Is this container locked?";
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        p.security.isLocked = EditorGUILayout.Toggle(p.security.isLocked, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Locked?", lockedTooltip));
        GUILayout.EndHorizontal();

        if (p.security.isLocked)
        {
            GUILayout.BeginHorizontal("box");

            string
                bypassableTooltip = "Can the player bypass this lock with " + Tapestry_Config.lockBypassSkill.ToString() + "?",
                levelTooltip = "How difficult this lock is to bypass.",
                keyTooltip = "Entities with a key with this ID can open this door when locked. After passing through the door, the Entity will re-lock it.";

            GUILayout.Label(new GUIContent("Bypassable?", bypassableTooltip));
            p.security.canBeBypassed = EditorGUILayout.Toggle(p.security.canBeBypassed, GUILayout.Width(12));
            GUILayout.FlexibleSpace();
            if (p.security.canBeBypassed)
            {
                GUILayout.Label(new GUIContent("Level", levelTooltip));
                p.security.LockLevel = EditorGUILayout.DelayedIntField(p.security.LockLevel, GUILayout.Width(30));
                GUILayout.FlexibleSpace();
            }
            GUILayout.Label(new GUIContent("Key", keyTooltip));
            p.security.keyID = EditorGUILayout.DelayedTextField(p.security.keyID, GUILayout.Width(100));

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

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
                string
                    destructableTooltip = "Does this object model swap when it reaches certain damage amounts?",
                    intactTooltip = "What GameObjects to display when the object is " + Tapestry_HealthState.Intact.ToString() + ". Use an empty GameObject to contain the parts.",
                    brokenTooltip = "What GameObjects to display when the object is " + Tapestry_HealthState.Broken.ToString() + ". Use an empty GameObject to contain the parts.",
                    destroTooltip = "What GameObjects to display when the object is " + Tapestry_HealthState.Destroyed.ToString() + ". Use an empty GameObject to contain the parts.";

                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                p.isDestructable = EditorGUILayout.Toggle(p.isDestructable, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Destructable?", destructableTooltip));
                GUILayout.EndHorizontal();

                if (p.isDestructable)
                {
                    GUILayout.BeginVertical("box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent(Tapestry_HealthState.Intact.ToString(), intactTooltip));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.ObjectField(p.intact, typeof(GameObject), true, GUILayout.Width(250));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent(Tapestry_HealthState.Broken.ToString(), brokenTooltip));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.ObjectField(p.broken, typeof(GameObject), true, GUILayout.Width(250));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent(Tapestry_HealthState.Destroyed.ToString(), destroTooltip));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.ObjectField(p.destroyed, typeof(GameObject), true, GUILayout.Width(250));
                    GUILayout.EndHorizontal();

                    string ejectTooltip = "Does this object drop its inventory items on the ground when it reaches certain levels of damage?";
                    GUILayout.BeginHorizontal();
                    p.ejectInventory = EditorGUILayout.Toggle(p.ejectInventory, GUILayout.Width(12));
                    GUILayout.Label(new GUIContent("Eject Inventory?", ejectTooltip));
                    GUILayout.EndHorizontal();
                    
                    if (p.ejectInventory)
                    {
                        EjectOptions op;
                        if (p.ejectState == Tapestry_HealthState.Destroyed)
                            op = EjectOptions.Destroyed;
                        else
                            op = EjectOptions.Broken;

                        GUILayout.BeginHorizontal("box");
                        GUILayout.Label("Eject when object is:");
                        op = (EjectOptions)EditorGUILayout.EnumPopup(op);
                        GUILayout.EndHorizontal();

                        if (op == EjectOptions.Broken)
                            p.ejectState = Tapestry_HealthState.Broken;
                        else if (op == EjectOptions.Destroyed)
                            p.ejectState = Tapestry_HealthState.Destroyed;
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Inventory")
            {
                if (p.inventory == null)
                    p.inventory = new Tapestry_Inventory(p.transform);

                int indexToRemove = -1;
                GUILayout.BeginVertical("box");
                GUILayout.Label("Inventory");
                GUILayout.BeginVertical("box");
                if (p.inventory.items.Count == 0)
                    GUILayout.Label("No items in inventory.");
                else
                {
                    for (int i = 0; i < p.inventory.items.Count; i++)
                    {
                        Tapestry_ItemStack stack = p.inventory.items[i];
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
                    if (p.inventory.items.Count == 1)
                        p.inventory.items.Clear();
                    else
                        p.inventory.items.RemoveAt(indexToRemove);
                }
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("+", GUILayout.Width(20)))
                {
                    if (itemToAdd != null)
                    {
                        if (p.inventory.ContainsItem(itemToAdd) == false)
                            p.inventory.AddItem(itemToAdd, 1);
                        itemToAdd = null;
                    }
                }
                itemToAdd = (Tapestry_Item)EditorGUILayout.ObjectField(itemToAdd, typeof(Tapestry_Item), true, GUILayout.Width(300));
                
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            if (toolbarNames[toolbarActive] == "Other")
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Time");
                GUILayout.BeginHorizontal("box");
                string timeTooltip = "What time scale this entity operates at. 1.0 is normal timp.";

                GUILayout.Label(new GUIContent("Factor", timeTooltip));
                p.personalTimeFactor = EditorGUILayout.DelayedFloatField(p.personalTimeFactor);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                if (p.keywords == null)
                    p.keywords = new List<string>();

                int indexToRemove = -1;
                GUILayout.BeginVertical("box");
                GUILayout.Label("Keywords");
                GUILayout.BeginVertical("box");
                if (p.keywords.Count == 0)
                {
                    GUILayout.Label("No keywords associated with this Entity.");
                }
                else
                {
                    for (int i = 0; i < p.keywords.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            indexToRemove = i;
                        }
                        p.keywords[i] = EditorGUILayout.DelayedTextField(p.keywords[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                if (indexToRemove != -1)
                {
                    if (p.keywords.Count == 1)
                        p.keywords.Clear();
                    else
                        p.keywords.RemoveAt(indexToRemove);
                }

                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    if (keywordToAdd != "")
                    {
                        p.keywords.Add(keywordToAdd);
                        keywordToAdd = null;
                    }
                }
                keywordToAdd = EditorGUILayout.TextField(keywordToAdd);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
        }
    }
    enum EjectOptions
    {
        Broken, Destroyed
    }
}

