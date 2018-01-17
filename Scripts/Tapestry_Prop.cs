using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Prop : Tapestry_Actor {

    public GameObject
        intact,
        broken,
        destroyed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetObjectState(Tapestry_HealthState state)
    {
        switch(state)
        {
            case Tapestry_HealthState.Intact:
                intact.SetActive(true);
                broken.SetActive(false);
                destroyed.SetActive(false);
                break;
            case Tapestry_HealthState.Broken:
                intact.SetActive(false);
                broken.SetActive(true);
                destroyed.SetActive(false);
                break;
            case Tapestry_HealthState.Destroyed:
                intact.SetActive(false);
                broken.SetActive(false);
                destroyed.SetActive(true);
                break;
            default:
                Debug.Log("[TAPESTRY WARNING] State passed to SetObjectState method on Prop \""+transform.name+"\" is an entity health state. Please use an object health state.");
                break;
        }
    }
}
