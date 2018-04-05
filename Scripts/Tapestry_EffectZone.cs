using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EffectZone : MonoBehaviour {

    public Tapestry_Effect effect;

    private void Reset()
    {
        effect = new Tapestry_Effect(this.transform);
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
            a.AddEffect(effect);
    }

    private void OnTriggerExit(Collider other)
    {
        Tapestry_Actor a = other.GetComponent<Tapestry_Actor>();
    }
}
