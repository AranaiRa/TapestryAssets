using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EffectBuilder_Shape_PointRadius : ITapestry_EffectBuilder_Shape {

    public string NameRegistration
    {
        get { return "Point Radius"; }
    }

    public float radius;

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

    public Tapestry_EffectBuilder_Shape_PointRadius()
    {

    }

    public List<Tapestry_Actor> GetAffectedTargets()
    {
        throw new System.NotImplementedException();
    }
}
