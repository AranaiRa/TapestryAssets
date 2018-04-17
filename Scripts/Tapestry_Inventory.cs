using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_Inventory : ScriptableObject {

    public List<Tapestry_ItemStack> items;
    private Tapestry_Item itemToAdd;

    public Tapestry_Inventory()
    {
        items = new List<Tapestry_ItemStack>();
    }

    public void AddItem(Tapestry_Item item, int quantity)
    {
        AddItem(item.data, quantity);
    }

    public void AddItem(Tapestry_ItemData item, int quantity)
    {
        Debug.Log("Running add method");
        if (item != null)
        {   
            if(items == null)
                items = new List<Tapestry_ItemStack>();
            bool added = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.IsEqual(item))
                {
                    items[i].quantity += quantity;
                    added = true;
                    break;
                }
            }
            if (!added)
                items.Add(new Tapestry_ItemStack(item, quantity));
            //items.Sort();
        }
    }

    public bool ContainsKeyID(string id)
    {
        foreach(Tapestry_ItemStack iStack in items)
        {
            Tapestry_ItemData i = iStack.item;
            if(i.isKey)
            {
                if (i.keyID == id)
                    return true;
            }
        }
        return false;
    }

    public bool ContainsItem(Tapestry_Item item)
    {
        bool check = false;
        foreach(Tapestry_ItemStack stack in items)
        {
            if(stack.item.IsEqual(item.data))
            {
                check = true;
                break;
            }
        }
        return check;
    }

    public void DropItem(Transform centerpoint, Tapestry_ItemData id, int amount = 1)
    {
        if (amount >= 0)
        {
            foreach(Tapestry_ItemStack stack in items)
            {
                if(stack.item.Equals(id))
                {
                    if (amount > stack.quantity)
                        amount = stack.quantity;
                    GameObject clone = GameObject.Instantiate(Resources.Load("Items/"+stack.item.prefabName) as GameObject);
                    clone.transform.position = centerpoint.position + centerpoint.forward * Tapestry_Config.ItemDropDistance;
                    clone.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    
                    stack.quantity -= amount;
                    if (stack.quantity == 0)
                    {
                        items.Remove(stack);
                    }
                    break;
                }
            }
        }
    }

    public void RemoveItem(Tapestry_ItemData id, int amount = 1)
    {
        if (amount >= 0)
        {
            foreach (Tapestry_ItemStack stack in items)
            {
                if (stack.item.Equals(id))
                {
                    if (amount > stack.quantity)
                        amount = stack.quantity;

                    stack.quantity -= amount;
                    if (stack.quantity == 0)
                    {
                        items.Remove(stack);
                    }
                    break;
                }
            }
        }
        //items.Sort();
    }

    public void RemoveKeyWithID(string id, int amount = 1)
    {
        if (amount >= 0)
        {
            foreach (Tapestry_ItemStack stack in items)
            {
                if (stack.item.isKey && stack.item.keyID == id)
                {
                    if (amount > stack.quantity)
                        amount = stack.quantity;

                    stack.quantity -= amount;
                    if (stack.quantity == 0)
                    {
                        items.Remove(stack);
                    }
                    break;
                }
            }
        }
    }

    public void DrawInspector()
    {
        GUIStyle title = new GUIStyle();
        title.fontStyle = FontStyle.Bold;
        title.fontSize = 14;

        int indexToRemove = -1;
        GUILayout.BeginVertical("box");
        GUILayout.Label("Inventory", title);
        GUILayout.BeginVertical("box");
        if (items.Count == 0)
            GUILayout.Label("No items in inventory.");
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                Tapestry_ItemStack stack = items[i];
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
                EditorGUILayout.TextField(stack.item.displayName, GUILayout.Width(270));
                GUILayout.EndHorizontal();
            }
        }
        if (indexToRemove != -1)
        {
            if (items.Count == 1)
                items.Clear();
            else
                items.RemoveAt(indexToRemove);
        }
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            if (itemToAdd != null)
            {
                AddItem(itemToAdd, 1);
                itemToAdd = null;
            }
        }
        itemToAdd = (Tapestry_Item)EditorGUILayout.ObjectField(itemToAdd, typeof(Tapestry_Item), true, GUILayout.Width(300));

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
