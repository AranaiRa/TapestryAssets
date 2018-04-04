using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_Effect {

    public bool hideEffectDisplay = false;
    public Tapestry_EffectBuilder_Delivery delivery;
    public Tapestry_EffectBuilder_Duration duration;
    public Tapestry_EffectBuilder_Payload payload;
    public Transform initiator;
    public Tapestry_Actor target;
    
	public Tapestry_Effect(Transform initiator)
    {
        this.initiator = initiator;
    }
}

public enum Tapestry_EffectBuilder_Duration
{
    Instant,
    ActualTime, WorldTime,
    UntilEventRegistered, Permanent
}