using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EffectZone : MonoBehaviour {
    
    public Tapestry_Effect effect;
    public bool removeEffectOnTriggerLeave = true;

    private void Reset()
    {
        effect = new Tapestry_Effect();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Tapestry_Actor a = other.GetComponentInParent<Tapestry_Actor>();
        if(a != null)
            a.AddEffect(effect.Clone());
    }

    private void OnTriggerExit(Collider other)
    {
        if(effect.duration != Tapestry_EffectBuilder_Duration.Instant)
        {
            Tapestry_Actor a = other.GetComponent<Tapestry_Actor>();
        }
    }
}
