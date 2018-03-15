using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Entity : Tapestry_Actor {

    [Range(0, 1000)]
    public float stamina = 1000;
    public Tapestry_Inventory inventory;
    public Tapestry_AttributeProfile attributeProfile;
    public Tapestry_SkillProfile skillProfile;
    public Tapestry_Prop pushTarget;
    public bool
        isRunning = false,
        isPushing = false,
        isLifting = false;
    public GameObject
        attachPoint;

    protected float speed;


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

    }

    protected virtual void Reset()
    {
        inventory = new Tapestry_Inventory(this.transform);
        damageProfile = new Tapestry_DamageProfile();
        attributeProfile = new Tapestry_AttributeProfile();
        skillProfile = new Tapestry_SkillProfile();
        keywords = new List<string>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Points")
            {
                GameObject pointContainer = transform.GetChild(i).gameObject;
                for (int j = 0; j < pointContainer.transform.childCount; j++)
                {
                    if (transform.GetChild(i).name == "P_Attach")
                    {
                        attachPoint = pointContainer.transform.GetChild(j).gameObject;
                    }
                }
            }
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