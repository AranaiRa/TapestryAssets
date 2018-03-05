using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Door : Tapestry_Activatable {

    public GameObject pivot;
    public Tapestry_Lock security;
    public float 
        openTime = 0.7f,
        lockJiggleIntensity = 0.016f;
    public AnimationCurve curve;
    public bool 
        jiggleOnActivateWhenLocked = false,
        relockWhenClosed = false,
        consumeKeyOnUnlock = false;
    public AudioSource
        emitter;
    public AudioClip
        openSound,
        closeSound,
        lockedSound;

    [SerializeField]
    private Vector3
        pos1 = Vector3.zero,
        pos2 = Vector3.zero,
        startingPos = Vector3.zero;
    [SerializeField]
    private Quaternion
        rot1 = Quaternion.identity,
        rot2 = Quaternion.identity,
        startingRot = Quaternion.identity;
    private bool
        isOpening = false,
        isClosing = false,
        isJiggling = false,
        isOpen = false;
    private float 
        time,
        jiggleTime = 0.4f;

    private void Reset()
    {
        displayName = "Door";
        curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        security = new Tapestry_Lock(false, 0, "");

        bool
            hasPivot = false,
            hasEmitter = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Pivot")
            {
                hasPivot = true;
                pivot = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "T_Emitter")
            {
                hasEmitter = true;
                emitter = transform.GetChild(i).gameObject.GetComponent<AudioSource>();
                if (emitter == null)
                    transform.GetChild(i).gameObject.AddComponent<AudioSource>();
            }
        }

        if (!hasPivot)
        {
            pivot = new GameObject();
            pivot.transform.SetParent(transform);
            pivot.name = "T_Pivot";
            pivot.transform.localPosition = Vector3.zero;
        }

        if(!hasEmitter)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.name = "T_Emitter";
            go.AddComponent<AudioSource>();
            go.transform.localPosition = Vector3.zero;
            emitter = go.GetComponent<AudioSource>();
        }

        emitter.playOnAwake = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isOpening)
        {
            time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
            if (time >= openTime)
                time = openTime;
            float prog = curve.Evaluate(time / openTime);
            prog = Mathf.Clamp(prog, 0, 1);

            Vector3 evalPos = Vector3.Lerp(startingPos, pos2, prog);
            Quaternion evalRot = Quaternion.Lerp(startingRot, rot2, prog);

            pivot.transform.localPosition = evalPos;
            pivot.transform.localRotation = evalRot;
            
            if (time == openTime)
            {
                isOpening = false;
                isOpen = true;
            }
        }
        else if (isClosing)
        {
            time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
            if (time >= openTime)
                time = openTime;
            float prog = curve.Evaluate(time / openTime);
            if (prog > 1) prog = 1;
            if (prog < 0) prog = 0;

            Vector3 evalPos;
            Quaternion evalRot;
            
            evalPos = Vector3.Lerp(startingPos, pos1, prog);
            evalRot = Quaternion.Lerp(startingRot, rot1, prog);

            pivot.transform.localPosition = evalPos;
            pivot.transform.localRotation = evalRot;

            if (time == openTime)
            {
                isClosing = false;
                isOpen = false;
                if (relockWhenClosed)
                    security.isLocked = true;
            }
        }
        else if(isJiggling)
        {
            time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
            if (time >= jiggleTime)
                time = jiggleTime;
            float amt = Random.Range(-lockJiggleIntensity, lockJiggleIntensity);

            if (!Tapestry_WorldClock.isPaused)
            {
                Quaternion evalRot = Quaternion.LerpUnclamped(rot1, rot2, amt);
                pivot.transform.localRotation = evalRot;
            }

            if (time == jiggleTime)
            {
                Close(true);
                isJiggling = false;
            }
        }
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {
        if(!isOpening && !isClosing)
        {
            if (!security.isLocked)
            {
                if (isOpen)
                    Close();
                else
                    Open();
            }
            else
            {
                if(!isOpen && jiggleOnActivateWhenLocked)
                {
                    emitter.clip = lockedSound;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.Play();
                    isJiggling = true;
                    time = 0;
                }
            }
        }
    }

    public void Open(bool instant=false)
    {
        if(!instant)
        {
            startingPos = pivot.transform.localPosition;
            startingRot = pivot.transform.localRotation;

            time = 0;
            isOpening = true;
            isClosing = false;
            emitter.clip = openSound;
            emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
            emitter.Play();
        }
        else
        {
            pivot.transform.localPosition = pos2;
            pivot.transform.localRotation = rot2;
            isOpen = true;
        }
    }

    public void Close(bool instant=false)
    {
        if(!instant)
        {
            startingPos = pivot.transform.localPosition;
            startingRot = pivot.transform.localRotation;
            time = 0;
            isOpening = false;
            isClosing = true;
            emitter.clip = closeSound;
            emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
            emitter.Play();
        }
        else
        {
            pivot.transform.localPosition = pos1;
            pivot.transform.localRotation = rot1;
            isOpen = false;
        }
    }

    public void BakeOpenState()
    {
        pos2 = pivot.transform.localPosition;
        rot2 = pivot.transform.localRotation;
    }

    public void BakeClosedState()
    {
        pos1 = pivot.transform.localPosition;
        rot1 = pivot.transform.localRotation;
    }

    public string GetOpenInspectorString()
    {
        return pos2.ToString() + " | " + rot2.ToString();
    }

    public string GetClosedInspectorString()
    {
        return pos1.ToString() + " | " + rot1.ToString();
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}
