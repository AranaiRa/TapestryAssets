using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Prop : Tapestry_Actor {

    public GameObject
        intact,
        broken,
        destroyed,
        attachPoints;
    public bool 
        isPushable, isLiftable, isDestructable, 
        gridAligned, allowLateral, allowPull,
        pushForcesThirdPerson, liftForcesThirdPerson;
    public int
        pushClumsy = 10, pushCompetent = 40, pushImpressive = 70,
        liftClumsy = 40, liftCompetent = 70, liftImpressive = 100;
    public AnimationCurve
        pushSpeedCurve, liftSpeedCurve;
    public float
        pushSpeedMin = 0.2f, pushSpeedMax = 0.6f,
        liftSpeedMin = 0.2f, liftSpeedMax = 0.9f,
        pushIncrement = 0.5f;
    public AudioClip
        pushingSound,
        collideNeutralSound,
        collidePushingSound;

    private float
        time,
        pushSpeed;
    private bool
        hasBoundEntity = false,
        isPushing = false;
    private GameObject
        activeAttachPoint;
    private Vector3
        startingPosGridbound,
        pushDir;

	void Start () {
		
	}
	
	void FixedUpdate () {
        if (!Tapestry_WorldClock.isPaused && hasBoundEntity)
        {
            HandlePush();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (isPushing)
        {
            if (collidePushingSound != null)
            {
                emitter.clip = collidePushingSound;
                emitter.loop = false;
                emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                emitter.Play();
            }
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
                isPushing = false;
        }
        else
        {
            if (collideNeutralSound != null)
            {
                emitter.clip = collideNeutralSound;
                emitter.loop = false;
                emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                emitter.Play();
            }
        }
    }

    protected override void Reset()
    {
        pushSpeedCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        liftSpeedCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        keywords = new List<string>();

        bool
            hasIntact = false, hasBroken = false, hasDestroyed = false, hasAttachPoints = false;

        for(int i=0; i<transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Intact")
            {
                hasIntact = true;
                intact = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "T_Broken")
            {
                hasBroken = true;
                broken = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "T_Destroyed")
            {
                hasDestroyed = true;
                destroyed = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "T_AttachPoints")
            {
                hasAttachPoints = true;
                attachPoints = transform.GetChild(i).gameObject;
            }
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.drag = Mathf.Infinity;
            rb.angularDrag = Mathf.Infinity;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.isKinematic = true;
            rb.mass = 15000;
        }

        if (!hasIntact)
        {
            intact = new GameObject();
            intact.transform.SetParent(transform);
            intact.transform.localPosition = Vector3.zero;
            intact.transform.localRotation = Quaternion.identity;
            intact.name = "T_Intact";
        }
        if (!hasBroken)
        {
            broken = new GameObject();
            broken.transform.SetParent(transform);
            broken.transform.localPosition = Vector3.zero;
            broken.transform.localRotation = Quaternion.identity;
            broken.name = "T_Broken";
            broken.SetActive(false);
        }
        if (!hasDestroyed)
        {
            destroyed = new GameObject();
            destroyed.transform.SetParent(transform);
            destroyed.transform.localPosition = Vector3.zero;
            destroyed.transform.localRotation = Quaternion.identity;
            destroyed.name = "T_Destroyed";
            destroyed.SetActive(false);
        }
        if (!hasAttachPoints)
        {
            attachPoints = new GameObject();
            attachPoints.transform.SetParent(transform);
            attachPoints.transform.SetParent(attachPoints.transform);
            attachPoints.transform.localPosition = Vector3.zero;
            attachPoints.transform.localRotation = Quaternion.identity;
            attachPoints.name = "T_AttachPoints";

            GameObject child = new GameObject();
            child.transform.SetParent(attachPoints.transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.name = "P_Attach";

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            gizmo.transform.SetParent(child.transform);
            gizmo.transform.localPosition = Vector3.zero;
            gizmo.transform.localRotation = Quaternion.identity;
        }

        base.Reset();
    }

    protected void HandlePush()
    {
        bool fwd =
            Input.GetKey(Tapestry_Config.KeyboardInput_Fwd) ||
            Input.GetKey(Tapestry_Config.ControllerInput_Fwd);
        bool bck =
            Input.GetKey(Tapestry_Config.KeyboardInput_Back) ||
            Input.GetKey(Tapestry_Config.ControllerInput_Back);
        bool lft =
            Input.GetKey(Tapestry_Config.KeyboardInput_Left) ||
            Input.GetKey(Tapestry_Config.ControllerInput_Left);
        bool rgt =
            Input.GetKey(Tapestry_Config.KeyboardInput_Right) ||
            Input.GetKey(Tapestry_Config.ControllerInput_Right);

        if (gridAligned)
        {
            if (isPushing)
            {
                time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor * personalTimeFactor;
                float prog = time / pushSpeed;
                if(prog > 1)
                {
                    prog = 1;
                    isPushing = false;
                }

                GetComponent<Rigidbody>().MovePosition(startingPosGridbound + pushDir * pushSpeedCurve.Evaluate(prog));
            }
            else if(!isPushing && fwd)
            {
                isPushing = true;
                time = 0;
                startingPosGridbound = transform.position;
                pushDir = activeAttachPoint.transform.forward * pushIncrement;
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = false;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else if (!isPushing && bck && allowPull)
            {
                isPushing = true;
                time = 0;
                startingPosGridbound = transform.position;
                pushDir = -activeAttachPoint.transform.forward * pushIncrement;
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = false;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else if (!isPushing && rgt && !lft && allowLateral)
            {
                isPushing = true;
                time = 0;
                startingPosGridbound = transform.position;
                pushDir = activeAttachPoint.transform.right * pushIncrement;
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = false;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else if (!isPushing && !rgt && lft && allowLateral)
            {
                isPushing = true;
                time = 0;
                startingPosGridbound = transform.position;
                pushDir = -activeAttachPoint.transform.right * pushIncrement;
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = false;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
        }
        else
        {
            isPushing = false;
            if (fwd)
            {
                isPushing = true;
                pushDir = activeAttachPoint.transform.forward * pushIncrement;
                GetComponent<Rigidbody>().MovePosition(transform.position + pushDir * pushSpeed * Time.deltaTime);
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = true;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else if (bck && allowPull)
            {
                isPushing = true;
                pushDir = -activeAttachPoint.transform.forward * pushIncrement;
                GetComponent<Rigidbody>().MovePosition(transform.position + pushDir * pushSpeed * Time.deltaTime);
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = true;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else if (rgt && !lft && allowLateral)
            {
                isPushing = true;
                pushDir = activeAttachPoint.transform.right * pushIncrement;
                GetComponent<Rigidbody>().MovePosition(transform.position + pushDir * pushSpeed * Time.deltaTime);
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = true;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else if (!rgt && lft && allowLateral)
            {
                isPushing = true;
                pushDir = -activeAttachPoint.transform.right * pushIncrement;
                GetComponent<Rigidbody>().MovePosition(transform.position + pushDir * pushSpeed * Time.deltaTime);
                if (pushingSound != null)
                {
                    emitter.clip = pushingSound;
                    emitter.loop = true;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                }
            }
            else
            {
                if (pushingSound != null)
                {
                    emitter.loop = false;
                }
            }
        }
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

    public override Tapestry_HealthState GetHealthState()
    {
        if (health > 400)
        {
            if (isDestructable)
            {
                intact.SetActive(true);
                broken.SetActive(false);
                destroyed.SetActive(false);
            }
            return Tapestry_HealthState.Intact;
        }
        else if (health > 0)
        {
            if (isDestructable)
            {
                intact.SetActive(false);
                broken.SetActive(true);
                destroyed.SetActive(false);
            }
            return Tapestry_HealthState.Broken;
        }
        else
        {
            if (isDestructable)
            {
                intact.SetActive(false);
                broken.SetActive(false);
                destroyed.SetActive(true);
            }
            return Tapestry_HealthState.Destroyed;
        }
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {
        if (isPushable)
        {
            if(activatingEntity.isPushing && !isPushing)
                UnbindEntity(activatingEntity);
            else
                BindEntityForPush(activatingEntity);
        }
        //base.Activate(activatingEntity);
    }

    public void BindEntityForPush(Tapestry_Entity e)
    {
        hasBoundEntity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        e.GetComponent<Rigidbody>().isKinematic = true;
        e.transform.SetParent(this.transform);
        e.isPushing = true;
        GameObject closest = null;
        float shortestDist = float.MaxValue;
        foreach(GameObject point in GetAttachPoints())
        {
            float thisDist = Vector3.Distance(point.transform.position, e.attachPoint.transform.position);
            if (thisDist < shortestDist)
            {
                closest = point;
                shortestDist = thisDist;
            }
        }
        e.transform.rotation = closest.transform.rotation;
        Vector3 offset = closest.transform.position - e.attachPoint.transform.position;
        e.transform.position = e.transform.position + offset;
        activeAttachPoint = closest;

        int score = e.attributeProfile.GetScore(Tapestry_Config.pushLiftAttribute);
        if (score > pushClumsy)
        {
            if (score > pushImpressive)
                score = pushImpressive;
            float prog = (float)(score - pushClumsy) / (float)(pushImpressive - pushClumsy);
            if (gridAligned)
            {
                pushSpeed = (pushSpeedCurve.Evaluate(prog) * (pushSpeedMax - pushSpeedMin)) + pushSpeedMin;
            }
            else
            {
                float 
                    min = Tapestry_Config.BaseEntityWalkSpeed * pushSpeedMin,
                    max = Tapestry_Config.BaseEntityWalkSpeed * pushSpeedMax;
                pushSpeed = (pushSpeedCurve.Evaluate(prog) * (max - min)) + min;
            }
        }
    }

    public void UnbindEntity(Tapestry_Entity e)
    {
        hasBoundEntity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        e.transform.SetParent(null);
        e.GetComponent<Rigidbody>().isKinematic = false;
        e.isPushing = false;
    }

    protected List<GameObject> GetAttachPoints()
    {
        List<GameObject> points = new List<GameObject>();
        for (int i = 0; i < attachPoints.transform.childCount; i++)
        {
            points.Add(attachPoints.transform.GetChild(i).gameObject);
        }
        return points;
    }
}
