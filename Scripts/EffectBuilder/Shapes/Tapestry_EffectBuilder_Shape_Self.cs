using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EffectBuilder_Shape_Self : ITapestry_EffectBuilder_Shape {

    public string NameRegistration
    {
        get { return "Self"; }
    }

    private Tapestry_Effect parent;
    public Tapestry_Effect Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    public Tapestry_EffectBuilder_Shape_Self()
    {

    }

    public List<Tapestry_Actor> GetAffectedTargets()
    {
        List<Tapestry_Actor> targets = new List<Tapestry_Actor>();
        targets.Add(parent.initiator.GetComponentInParent<Tapestry_Actor>());
        return targets;
    }
}
