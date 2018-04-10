using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {
    
    public Tapestry_Effect e;
    public Tapestry_EffectBuilder_Payload_Damage d;

    private void Reset()
    {
        e = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");
        e.payload = new Tapestry_EffectBuilder_Payload_Damage();
        d = new Tapestry_EffectBuilder_Payload_Damage();
        //e.name = "buttfarts";
    }
}
