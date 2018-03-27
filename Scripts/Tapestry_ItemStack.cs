using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ItemStack {

    public Tapestry_ItemData item;
    public int quantity;

    public Tapestry_ItemStack(Tapestry_ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
