using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {

    public Tapestry_EffectBuilder_Payload_Damage d;
    public Tapestry_EffectBuilder_Shape_Ray s;

    private void Reset()
    {
        s = new Tapestry_EffectBuilder_Shape_Ray(null, 50);
        d = new Tapestry_EffectBuilder_Payload_Damage(null, Tapestry_DamageType.Holy, 7, 1400);
    }
}
