using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Prop : Tapestry_Actor {

    public GameObject
        intact,
        broken,
        destroyed;
    public bool 
        isPushable, isLiftable;
    public int
        pushClumsy = 10, pushCompetent = 40, pushImpressive = 70,
        liftClumsy = 40, liftCompetent = 70, liftImpressive = 100;
    public AnimationCurve
        pushSpeedCurve, liftSpeedCurve;
    public float
        pushSpeedMin = 0.2f, pushSpeedMax = 0.6f,
        liftSpeedMin = 0.2f, liftSpeedMax = 0.9f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Reset()
    {
        pushSpeedCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        liftSpeedCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
    }

    public void SetObjectState(Tapestry_HealthState state)
    {
        switch(state)
        {
            case Tapestry_HealthState.Intact:
                intact.SetActive(true);
                broken.SetActive(false);
                destroyed.SetActive(false);
                break;
            case Tapestry_HealthState.Broken:
                intact.SetActive(false);
                broken.SetActive(true);
                destroyed.SetActive(false);
                break;
            case Tapestry_HealthState.Destroyed:
                intact.SetActive(false);
                broken.SetActive(false);
                destroyed.SetActive(true);
                break;
            default:
                Debug.Log("[TAPESTRY WARNING] State passed to SetObjectState method on Prop \""+transform.name+"\" is an entity health state. Please use an object health state.");
                break;
        }
    }
}
