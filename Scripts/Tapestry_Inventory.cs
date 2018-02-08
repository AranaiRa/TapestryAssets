using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Inventory {

    public List<Tapestry_ItemStack> items;

    public Tapestry_Inventory()
    {
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
                    Debug.Log("items are equal");
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
        Debug.Log("TODO: ContainsKeyID");
        /*
        foreach(Tapestry_ItemStack iStack in items)
        {
            Tapestry_Item i = iStack.item.data;
            if (i.GetType() == typeof(Tapestry_ItemKey))
            {
                Tapestry_ItemKey k = (Tapestry_ItemKey)i;
                if (k.keyID == id)
                    return true;
            }
        }*/
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
}
