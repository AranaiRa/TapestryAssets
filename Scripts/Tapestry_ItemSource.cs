using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_ItemSource : Tapestry_Activatable {

    public GameObject 
        meshHarvestable,
        meshUnharvestable;
    public bool
        isHarvestable = true,
        requiresLevelReload = false,
        automaticRefresh = true;
    public Tapestry_Item item;
    public int
        quantityMin,
        quantityMax;
    public AnimationCurve quantityWeightCurve;
    public Vector3Int resetDelay = new Vector3Int(0,0,3); //x=minutes,y=hours,z=days

    private Tapestry_TimeIndex resetTime;

    private void Reset()
    {
        bool
            hasHarvestable = false,
            hasUnharvestable = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Collection_On")
            {
                hasHarvestable = true;
                meshHarvestable = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "T_Collection_Off")
            {
                hasUnharvestable = true;
                meshUnharvestable = transform.GetChild(i).gameObject;
            }
        }

        if (!hasHarvestable)
        {
            meshHarvestable = new GameObject();
            meshHarvestable.transform.SetParent(transform);
            meshHarvestable.transform.localPosition = Vector3.zero;
            meshHarvestable.name = "T_Collection_On";
        }
        if (!hasUnharvestable)
        {
            meshUnharvestable = new GameObject();
            meshUnharvestable.transform.SetParent(transform);
            meshUnharvestable.transform.localPosition = Vector3.zero;
            meshUnharvestable.name = "T_Collection_Off";
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(automaticRefresh && !requiresLevelReload && !isHarvestable)
        {
            if(Tapestry_WorldClock.worldTime.HasPassedTime(resetTime))
            {
                SetHarvestability(true);
            }
        }
	}

    public void SetHarvestability(bool harvestable)
    {
        if(harvestable)
        {
            isHarvestable = true;
            meshHarvestable.SetActive(true);
            meshUnharvestable.SetActive(false);
        }
        else
        {
            isHarvestable = false;
            meshHarvestable.SetActive(false);
            meshUnharvestable.SetActive(true);

            resetTime = Tapestry_WorldClock.worldTime.GetIndexFromOffset(0, resetDelay.x, resetDelay.y, resetDelay.z);
        }
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {
        if (isHarvestable)
        {
            int q = quantityMin;
            if (quantityMin != quantityMax)
            {
                q = Mathf.RoundToInt(quantityWeightCurve.Evaluate(Random.value) * (float)quantityMax) - quantityMin;
            }

            activatingEntity.inventory.AddItem(item, q);
            SetHarvestability(false);
        }
        //base.Activate();
    }
}
