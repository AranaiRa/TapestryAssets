using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Switch : Tapestry_Activatable {

    public GameObject pivot, target;
    public float
        switchTime = 0.4f,
        pingPongHoldTime = 0.4f;
    public AnimationCurve curve;
    public AudioSource
        emitter;
    public AudioClip
        onSound,
        offSound;
    public bool
        pingPong = false;

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
        isSwitchingOn = false,
        isSwitchingOff = false,
        isOn = false;
    private float
        time,
        jiggleTime = 0.4f;

    private void Reset()
    {
        displayName = "Switch";
        curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));

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

        if (!hasEmitter)
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
        if (isSwitchingOn)
        {
            time += Time.deltaTime;
            if (time >= switchTime)
                time = switchTime;
            float prog = curve.Evaluate(time / switchTime);
            prog = Mathf.Clamp(prog, 0, 1);

            Vector3 evalPos = Vector3.Lerp(startingPos, pos2, prog);
            Quaternion evalRot = Quaternion.Lerp(startingRot, rot2, prog);

            pivot.transform.localPosition = evalPos;
            pivot.transform.localRotation = evalRot;

            if (time == switchTime)
            {
                isSwitchingOn = false;
                isOn = true;
                if (pingPong)
                {
                    SwitchOff(false, pingPongHoldTime);
                }
            }
        }
        else if (isSwitchingOff)
        {
            time += Time.deltaTime;
            if (time >= switchTime)
                time = switchTime;
            float prog = curve.Evaluate(time / switchTime);
            if (prog > 1) prog = 1;
            
            if (time > 0)
            {
                Vector3 evalPos = Vector3.Lerp(startingPos, pos1, prog);
                Quaternion evalRot = Quaternion.Lerp(startingRot, rot1, prog);

                pivot.transform.localPosition = evalPos;
                pivot.transform.localRotation = evalRot;
            }

            if (time == switchTime)
            {
                isSwitchingOff = false;
                isOn = false;
            }
        }
    }

    public override void Activate()
    {
        if (!isSwitchingOn && !isSwitchingOff)
        {
            if (isOn)
                SwitchOff();
            else
                SwitchOn();
        }
    }

    public void SwitchOn(bool instant = false)
    {
        if (!instant)
        {
            startingPos = pivot.transform.localPosition;
            startingRot = pivot.transform.localRotation;
            time = 0;
            isSwitchingOn = true;
            isSwitchingOff = false;
            emitter.clip = onSound;
            emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
            emitter.Play();
        }
        else
        {
            pivot.transform.localPosition = pos2;
            pivot.transform.localRotation = rot2;
            isOn = true;
        }
    }

    public void SwitchOff(bool instant = false, float delay = 0)
    {
        if (!instant)
        {
            startingPos = pivot.transform.localPosition;
            startingRot = pivot.transform.localRotation;
            time = 0;
            if (delay > 0)
                time = -delay;
            isSwitchingOn = false;
            isSwitchingOff = true;
            emitter.clip = offSound;
            emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
            emitter.Play();
        }
        else
        {
            pivot.transform.localPosition = pos1;
            pivot.transform.localRotation = rot1;
            isOn = false;
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
}
