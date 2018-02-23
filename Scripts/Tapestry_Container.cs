using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Container : Tapestry_Prop {

    public Tapestry_Lock security;
    public Tapestry_Inventory inventory;
    public bool ejectInventory;
    public Tapestry_HealthState ejectState = Tapestry_HealthState.Destroyed;

	// Use this for initialization
	void Start () {
		
	}

    protected override void Reset()
    {
        inventory = new Tapestry_Inventory(this.transform);
        security = new Tapestry_Lock(false, 0, "");

        base.Reset();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
