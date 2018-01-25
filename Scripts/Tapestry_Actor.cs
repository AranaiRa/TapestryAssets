using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Actor : MonoBehaviour {

    [Range(0,1000)]
    public float health = 1000;
    [Range(0, 1000)]
    public float threshold;
    public Tapestry_DamageProfile damageProfile;
    public float personalTimeFactor = 1.0f;
    public string[] keywords;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual Tapestry_HealthState GetHealthState()
    {
        if (health > 400) return Tapestry_HealthState.Intact;
        else if (health > 0) return Tapestry_HealthState.Broken;
        else return Tapestry_HealthState.Destroyed;
    }

    public void Activate()
    {

    }
}

public enum Tapestry_HealthState
{
    Intact, Broken, Destroyed,
    Stable, Bloodied, Dying, Dead
}