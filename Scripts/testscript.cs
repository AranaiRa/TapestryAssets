using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {

    public Tapestry_EffectBuilder_Payload_Damage d;
    public Tapestry_EffectBuilder_Delivery_Ray s;

    private void Reset()
    {
        s = new Tapestry_EffectBuilder_Delivery_Ray();
        d = new Tapestry_EffectBuilder_Payload_Damage();
    }
}
