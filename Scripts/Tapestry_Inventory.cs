using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Inventory {

    public List<Tapestry_ItemStack> items;
    private Transform source;

    public Tapestry_Inventory(Transform source)
    {
        this.source = source;
        items = new List<Tapestry_ItemStack>();
    }

    public void AddItem(Tapestry_Item item, int quantity)
    {
        if (item != null)
        {
            bool added = false;
            for(int i=0;i<items.Count;i++)
            {
                if(items[i].item.Compare(item.data))
                {
                    items[i].quantity += quantity;
                    added = true;
                    break;
                }
            }
            if(!added)
                items.Add(new Tapestry_ItemStack(item.data, quantity));
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
            if(stack.item.Compare(item.data))
            {
                check = true;
                break;
            }
        }
        return check;
    }

    public void DropItem(Tapestry_ItemData id, int amount = 1)
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
                    clone.transform.position = source.position + source.forward * Tapestry_Config.ItemDropDistance;
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
}
