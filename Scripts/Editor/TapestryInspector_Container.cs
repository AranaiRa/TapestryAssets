using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tapestry_Container))]
public class TapestryInspector_Container : TapestryInspector_Prop {
    
    Tapestry_Item itemToAdd;

    public override void OnInspectorGUI()
    {
        string[] toolbarNames = { "Interaction", "Destruction", "Inventory", "Other" };
        Tapestry_Container p = target as Tapestry_Container;

        string
            displayTooltip = "What string will display on the player's HUD when looking at this object.",
            changeTimeTooltip = "The amount of time, in seconds, it takes for the door to open or close.",
            changeCurveTooltip = "Animation controls for how the door eases between states.",
            interactableTooltip = "Can the player interact with this door?",
            displayNameTooltip = "Should the object still show its display name when the player's cursor is hovering over the object?";

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Display Name", displayTooltip));
        GUILayout.FlexibleSpace();
        p.displayName = EditorGUILayout.DelayedTextField(p.displayName, GUILayout.Width(270));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        p.isInteractable = EditorGUILayout.Toggle(p.isInteractable, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Interactable?", interactableTooltip));
        GUILayout.Space(20);
        if (!p.isInteractable)
        {
            p.displayNameWhenUnactivatable = EditorGUILayout.Toggle(p.displayNameWhenUnactivatable, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Display Name Anyway?", displayNameTooltip));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        if (p.security == null)
            p.security = new Tapestry_Lock(false, 0, "");
        
        DrawSecurityTab(p);

        toolbarActive = GUILayout.Toolbar(toolbarActive, toolbarNames);

        if (toolbarActive != -1)
        {
            if (toolbarNames[toolbarActive] == "Interaction")
            {
                DrawSubTabAnimated(p);
                DrawSubTabPushable(p);
                DrawSubTabLiftable(p);
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



    protected void DrawSubTabAnimated(Tapestry_Container c)
    {
        string
            animatedTooltip = "Does this container have an open state and closed state?",
            openTransformTooltip = "The transform data for the container's open state. Don't worry about the actual numbers too much, but if they're the same as the closed values, you need to bake your open and closed states.",
            closedTransformTooltip = "The transform data for the container's closed state. Don't worry about the actual numbers too much, but if they're the same as the closed values, you need to bake your open and closed states.",
            openSoundTooltip = "The sound to play when the container opens, if any.",
            closedSoundTooltip = "The sound to play when the container closes, if any.";

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        c.isAnimated = EditorGUILayout.Toggle(c.isAnimated, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Animated Open/Close?", animatedTooltip));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (c.isAnimated)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("On");
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(c.GetOpenInspectorString(), openTransformTooltip));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Open"))
            {
                if (Application.isPlaying)
                    c.Open();
                else
                    c.Open(true);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Bake Open Pivot Transform"))
            {
                c.BakeOpenState();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Sound", openSoundTooltip));
            c.openSound = (AudioClip)EditorGUILayout.ObjectField(c.openSound, typeof(AudioClip), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Off");
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(c.GetClosedInspectorString(), closedTransformTooltip));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close"))
            {
                if (Application.isPlaying)
                    c.Close();
                else
                    c.Close(true);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Bake Closed Pivot Transform"))
            {
                c.BakeClosedState();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Sound", closedSoundTooltip));
            c.closeSound = (AudioClip)EditorGUILayout.ObjectField(c.closeSound, typeof(AudioClip), true, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }

    protected void DrawSecurityTab(Tapestry_Container c)
    {
        string lockedTooltip = "Is this container locked?";
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        c.security.isLocked = EditorGUILayout.Toggle(c.security.isLocked, GUILayout.Width(12));
        GUILayout.Label(new GUIContent("Locked?", lockedTooltip));
        GUILayout.EndHorizontal();

        if (c.security == null)
            c.security = new Tapestry_Lock(false, 0, "");

        if (c.security.isLocked)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();

            string
                bypassableTooltip = "Can the player bypass this lock with " + Tapestry_Config.lockBypassSkill.ToString() + "?",
                levelTooltip = "How difficult this lock is to bypass.",
                keyTooltip = "Entities with a key with this ID can open this container when locked. After closing the container, the Entity will re-lock it.",
                lockedJiggleTooltip = "If this container is locked, does it jiggle when activated?",
                jiggleIntensityTooltip = "How much this container jiggles on activation when locked. This is a percentage of the difference between the closed state and the open state.",
                lockedSoundTooltip = "The sound that plays when the container is unsuccessfully opened when locked, if any.",
                relockTooltip = "Should this container relock itself once closed?",
                consumeKeyTooltip = "Should the key for this container be removed from the player's inventory when the container is unlocked?";

            c.security.canBeBypassed = EditorGUILayout.Toggle(c.security.canBeBypassed, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Bypassable?", bypassableTooltip));
            GUILayout.FlexibleSpace();
            if (c.security.canBeBypassed)
            {
                GUILayout.Label(new GUIContent("Level", levelTooltip));
                c.security.LockLevel = EditorGUILayout.DelayedIntField(c.security.LockLevel, GUILayout.Width(30));
                GUILayout.FlexibleSpace();
            }
            GUILayout.Label(new GUIContent("Key", keyTooltip));
            c.security.keyID = EditorGUILayout.DelayedTextField(c.security.keyID, GUILayout.Width(100));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            c.jiggleOnActivateWhenLocked = EditorGUILayout.Toggle(c.jiggleOnActivateWhenLocked, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Jiggle?", lockedJiggleTooltip));
            if (c.jiggleOnActivateWhenLocked)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("Intensity", jiggleIntensityTooltip));
                c.lockJiggleIntensity = EditorGUILayout.DelayedFloatField(c.lockJiggleIntensity, GUILayout.Width(40));
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent("Sound", lockedSoundTooltip));
                c.lockedSound = (AudioClip)EditorGUILayout.ObjectField(c.lockedSound, typeof(AudioClip), true, GUILayout.Width(120));
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            c.relockWhenClosed = EditorGUILayout.Toggle(c.relockWhenClosed, GUILayout.Width(12));
            GUILayout.Label(new GUIContent("Relock When Closed?", relockTooltip));
            if (!c.relockWhenClosed)
            {
                GUILayout.FlexibleSpace();
                c.consumeKeyOnUnlock = EditorGUILayout.Toggle(c.consumeKeyOnUnlock, GUILayout.Width(12));
                GUILayout.Label(new GUIContent("Consume Key on Unlock?", consumeKeyTooltip));
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }

    enum EjectOptions
    {
        Broken, Destroyed
    }
}

