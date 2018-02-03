using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_ItemStack {

    public Tapestry_Item item;
    public int quantity;

    public Tapestry_ItemStack(Tapestry_Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
