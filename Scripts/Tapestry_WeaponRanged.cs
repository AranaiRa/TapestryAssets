using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_WeaponRanged : MonoBehaviour {

    public float chargeUpTime;
    public Tapestry_Effect
        effectUncharged,
        effectCharged;
    [Range(0,1)]
    public float chargeEffectScale = 0.2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
