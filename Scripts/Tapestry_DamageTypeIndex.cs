using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tapestry_DamageTypeIndex {
    
    private float resistance;
    private float mitigation;

    public float Resistance
    {
        get
        {
            return resistance;
        }

        set
        {
            if (value < -1) resistance = -1;
            else if (value > 1) resistance = 1;
            else resistance = value;
        }
    }

    public float Mitigation
    {
        get
        {
            return mitigation;
        }

        set
        {
            mitigation = value;
        }
    }

    public Tapestry_DamageTypeIndex(float res, float mit) : this()
    {
        Resistance = res;
        Mitigation = mit;
    }
}

public enum Tapestry_DamageType
{
    Burning, Choking, Corrosive, Crushing,
    Electric, Explosive, Freezing, Holy,
    Irradiating, Necrotic, Piercing, Slashing,
    Toxic, Untyped
}