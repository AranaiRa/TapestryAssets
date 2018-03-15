using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapestryTech_Gizmo : MonoBehaviour {

    static readonly bool killOnPlay = true;

	void Start () {
        if(killOnPlay)
            Destroy(this.gameObject);
	}
}
