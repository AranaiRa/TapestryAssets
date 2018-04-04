using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ActorValue {

    float
        baseValue;
    List<float>
        bonusTypeAdditive,
        bonusTypeAdditiveMultiplicative,
        bonusTypeMultiplicative;

    public Tapestry_ActorValue(float baseValue)
    {

    }
}
