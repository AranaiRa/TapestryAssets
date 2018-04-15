using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ItemList {

    public List<Tapestry_ItemListIndex> items;
    public int
        itemsToGenerateMin,
        itemsToGenerateMax;
    
    public Tapestry_ItemList()
    {
        items = new List<Tapestry_ItemListIndex>();
    }

    private int GetTotalWeight()
    {
        int totalWeight = 0;
        foreach (Tapestry_ItemListIndex index in items)
        {
            totalWeight += index.weight;
        }
        return totalWeight;
    }

    private Tapestry_ItemListIndex GetIndexAtWeight(int input)
    {
        Tapestry_ItemListIndex output = null;
        foreach (Tapestry_ItemListIndex index in items)
        {
            input -= index.weight;
            if (input <= 0)
            {
                output = index;
                break;
            }
        }
        return output;
    }

    public List<Tapestry_ItemStack> GenerateItems()
    {
        List<Tapestry_ItemStack> export = new List<Tapestry_ItemStack>();

        int totalWeight = GetTotalWeight();
        int totalToGenerate = Random.Range(itemsToGenerateMin, itemsToGenerateMax + 1);
        for(int i=0; i< totalToGenerate; i++)
        {
            Tapestry_ItemListIndex index = GetIndexAtWeight(Random.Range(0, totalWeight + 1));
            int amount = Random.Range(index.rangeMin, index.rangeMax + 1);
            bool hasItem = false;
            foreach (Tapestry_ItemStack exStack in export)
            {
                if (exStack.item.Equals(index.item))
                {
                    exStack.quantity += amount;
                    hasItem = true;
                    break;
                }
            }

            if (!hasItem)
            {
                export.Add(new Tapestry_ItemStack(index.item.data, amount));
            }
        }

        //TODO: Remove generator text
        foreach(Tapestry_ItemStack st in export)
        {
            Debug.Log("Generated " + st.quantity + "x " + st.item.displayName);
        }

        return export;
    }

    public Tapestry_Inventory GenerateInventory(Transform source)
    {
        Tapestry_Inventory export = new Tapestry_Inventory();

        export.items = GenerateItems();

        return export;
    }
}
