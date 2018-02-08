﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_Item : Tapestry_Activatable {

    public Tapestry_ItemData data;

    private void Reset()
    {
        displayName = data.displayName;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Activate()
    {
        Destroy(this.gameObject);
    }
}

public enum Tapestry_ItemSize
{
    Negligible, Small, Medium, Large
}