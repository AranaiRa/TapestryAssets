using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ItemListIndex {

    public Tapestry_Item item;
    public int 
        rangeMin,
        rangeMax,
        weight;

    public Tapestry_ItemListIndex(Tapestry_Item item, int min, int max, int weight)
    {
        this.item = item;
        this.rangeMin = min;
        this.rangeMax = max;
        this.weight = weight;
    }
}
