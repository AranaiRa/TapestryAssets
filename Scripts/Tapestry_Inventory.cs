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
            items.Add(new Tapestry_ItemStack(item, quantity));
        }
    }

    public bool ContainsKeyID(string id)
    {
        foreach(Tapestry_ItemStack iStack in items)
        {
            Tapestry_Item i = iStack.item;
            if (i.GetType() == typeof(Tapestry_ItemKey))
            {
                Tapestry_ItemKey k = (Tapestry_ItemKey)i;
                if (k.keyID == id)
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
            if(item == stack.item)
            {
                check = true;
                break;
            }
        }
        return check;
    }
}
