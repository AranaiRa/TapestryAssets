using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Actor : Tapestry_Activatable {
    
    public float health = 1000;
    public float threshold;
    public Tapestry_DamageProfile damageProfile;
    public float personalTimeFactor = 1.0f;
    public List<Tapestry_Effect> effects;

	// Use this for initialization
	void Start () {
        effects = new List<Tapestry_Effect>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        HandleActorEffects();
	}

    private void HandleActorEffects()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            Tapestry_Effect e = effects[i];

            e.payload.Apply(this);

            if (e.duration == Tapestry_EffectBuilder_Duration.Instant)
                effects.Remove(e);
        }
    }

    public virtual Tapestry_HealthState GetHealthState()
    {
        if (health > 400) return Tapestry_HealthState.Intact;
        else if (health > 0) return Tapestry_HealthState.Broken;
        else return Tapestry_HealthState.Destroyed;
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {

    }

    public virtual void AddEffect(Tapestry_Effect effect)
    {
        //temp: filter effects that don't work on actors
        effects.Add(effect);
    }

    public virtual void DealDamage(Tapestry_DamageType type, float amount)
    {
        amount *= (1 + damageProfile.GetRes(type));
        amount -= damageProfile.GetMit(type);
        if(amount >= threshold)
        {
            health -= amount;
            health = Mathf.Clamp(health, 0, 1000);
        }
    }

    public virtual void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 1000);
    }
}

public enum Tapestry_HealthState
{
    Intact, Broken, Destroyed,
    Stable, Bloodied, Dying, Dead
}