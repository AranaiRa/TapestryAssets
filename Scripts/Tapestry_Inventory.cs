using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Inventory {

    public List<Tapestry_Item> items;

    public Tapestry_Inventory()
    {
        items = new List<Tapestry_Item>();
    }

    public void AddItem(Tapestry_Item item)
    {
        if (item != null)
        {
            /*if (item.GetType() == typeof(Tapestry_ItemKey))
                keys.Add((Tapestry_ItemKey)item);
            else*/
            items.Add(item);
            Debug.Log("added an item named " + item.name + ", currently " + items.Count + " total items");
        }
        //Debug.Log("item passed was null, currently " + items.Count + " total items");
    }

    public bool ContainsKeyID(string id)
    {
        foreach(Tapestry_Item i in items)
        {
            if (i.GetType() == typeof(Tapestry_ItemKey))
            {
                Tapestry_ItemKey k = (Tapestry_ItemKey)i;
                if (k.keyID == id)
                    return true;
            }
        }
        return false;
    }
}
