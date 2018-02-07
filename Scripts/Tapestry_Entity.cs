using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Entity : Tapestry_Actor {

    [Range(0, 1000)]
    public float stamina = 1000;
    public Tapestry_Inventory inventory;
    public Tapestry_AttributeProfile attributeProfile;
    public Tapestry_SkillProfile skillProfile;
    public bool isRunning = false;
    public Rigidbody rb;
    public float speed3D, speed2D;
    protected Vector2 velocity2D;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    protected virtual void Reset()
    {
        velocity2D = new Vector2();
        inventory = new Tapestry_Inventory();
        damageProfile = new Tapestry_DamageProfile();
        attributeProfile = new Tapestry_AttributeProfile();
        skillProfile = new Tapestry_SkillProfile();
        keywords = new List<string>();

        if (rb == null)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
                gameObject.AddComponent<Rigidbody>();
            else
                rb = rigidbody;
        }
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
        if (stamina > 500) return Tapestry_StaminaState.Fresh;
        else if (stamina > 200) return Tapestry_StaminaState.Winded;
        else if (stamina > 0) return Tapestry_StaminaState.Exhausted;
        else return Tapestry_StaminaState.Unconscious;
    }
}

public enum Tapestry_StaminaState
{
    Fresh, Winded, Exhausted, Unconscious
}