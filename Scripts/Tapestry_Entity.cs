using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Entity : Tapestry_Actor {

    [Range(0, 1000)]
    public float stamina;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public override Tapestry_HealthState GetHealthState()
    {
        if (health > 500) return Tapestry_HealthState.Stable;
        else if (health > 200) return Tapestry_HealthState.Bloodied;
        else if (health > 0) return Tapestry_HealthState.Dying;
        else return Tapestry_HealthState.Dead;
    }

    public Tapestry_StaminaState GetStaminaState()
    {
        if (health > 500) return Tapestry_StaminaState.Fresh;
        else if (health > 200) return Tapestry_StaminaState.Winded;
        else if (health > 0) return Tapestry_StaminaState.Exhausted;
        else return Tapestry_StaminaState.Unconscious;
    }
}

public enum Tapestry_StaminaState
{
    Fresh, Winded, Exhausted, Unconscious
}