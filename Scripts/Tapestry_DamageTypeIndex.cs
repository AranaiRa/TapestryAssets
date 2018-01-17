using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tapestry_DamageTypeIndex {
    public Tapestry_DamageType damageType;
    [Range(-1,1)]
    public float resistance;
    public float mitigation;

    public Tapestry_DamageTypeIndex(Tapestry_DamageType type, float res, float mit)
    {
        damageType = type;
        resistance = res;
        mitigation = mit;
    }
}

public enum Tapestry_DamageType
{
    Burning, Choking, Corrosive, Crushing,
    Electric, Explosive, Freezing, Irradiating,
    Piercing, Slashing, Toxic, Untyped
}