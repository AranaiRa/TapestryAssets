﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Switch : Tapestry_Activatable {

    public GameObject pivot, target;
    public float
        switchTime = 0.4f,
        pingPongHoldTime = 0.4f;
    public AnimationCurve curve;
    public AudioClip
        onSound,
        offSound;
    public bool
        pingPong = false,
        fireOnlyOnce = false;
    /*[SerializeField]
    public Dictionary<Tapestry_Activatable, Tapestry_SwitchData> 
        data = new Dictionary<Tapestry_Activatable, Tapestry_SwitchData>();*/
    [SerializeField]
    public Tapestry_SwitchData 
        data;

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
    protected bool
        isOn = false,
        hasFired = false,
        isSwitchingOn = false,
        isSwitchingOff = false;
    private float
        time;

    protected override void Reset()
    {
        displayName = "Switch";
        curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        data = new Tapestry_SwitchData();

        bool
            hasPivot = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Pivot")
            {
                hasPivot = true;
                pivot = transform.GetChild(i).gameObject;
            }
        }

        if (!hasPivot)
        {
            pivot = new GameObject();
            pivot.transform.SetParent(transform);
            pivot.name = "T_Pivot";
            pivot.transform.localPosition = Vector3.zero;
        }

        base.Reset();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (isSwitchingOn)
        {
            time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
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
                EvaluateOnState();
                if (pingPong)
                {
                    SwitchOff(false, pingPongHoldTime);
                }
            }
        }
        else if (isSwitchingOff)
        {
            time += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
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
                if (!pingPong)
                    EvaluateOffState();
            }
        }
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {
        if (!fireOnlyOnce || (fireOnlyOnce && !hasFired))
        {
            if (!isSwitchingOn && !isSwitchingOff)
            {
                if (isOn)
                    SwitchOff();
                else
                    SwitchOn();
            }
        }
    }

    public void EvaluateOnState()
    {
        hasFired = true;
        isOn = true;
        if (target != null)
        {
            Tapestry_Activatable comp = target.GetComponent<Tapestry_Activatable>();
            if (pingPong)
            {
                if (comp.GetType() == typeof(Tapestry_Door))
                {
                    Tapestry_Door d = target.GetComponent<Tapestry_Door>();
                    if (data.pp_swapOpenState)
                    {
                        if (d.GetIsOpen()) d.Close();
                        else d.Open();
                    }
                    if (data.pp_swapLockedState) d.security.isLocked = !d.security.isLocked;
                    if (data.pp_swapBypassableState) d.security.canBeBypassed = !d.security.canBeBypassed;
                    if (data.pp_swapInteractivityState) d.isInteractable = !d.isInteractable;
                }
                if (comp.GetType() == typeof(Tapestry_AnimatedLight))
                {
                    Tapestry_AnimatedLight al = target.GetComponent<Tapestry_AnimatedLight>();
                    if (data.pp_swapOpenState)
                    {
                        if (al.GetIsOn()) al.TurnOff();
                        else al.TurnOn();
                    }
                    if (data.pp_swapInteractivityState) al.isInteractable = !al.isInteractable;
                }
                if (comp.GetType() == typeof(Tapestry_ItemSource))
                {
                    Tapestry_ItemSource i = target.GetComponent<Tapestry_ItemSource>();
                    if (data.pp_swapInteractivityState) i.isInteractable = !i.isInteractable;
                    if (data.pp_setSourceHarvestable) i.SetHarvestability(true);
                }
            }
            else
            {
                if (comp.GetType() == typeof(Tapestry_Door))
                {
                    Tapestry_Door d = target.GetComponent<Tapestry_Door>();
                    if (data.on_setOpen) d.Open();
                    if (data.on_setClosed) d.Close();
                    if (data.on_setLocked) d.security.isLocked = true;
                    if (data.on_setUnlocked) d.security.isLocked = false;
                    if (data.on_setBypassable) d.security.canBeBypassed = data.on_setBypassable;
                    if (data.on_setInteractable) d.isInteractable = data.on_setInteractable;
                }
                if (comp.GetType() == typeof(Tapestry_AnimatedLight))
                {
                    Tapestry_AnimatedLight al = target.GetComponent<Tapestry_AnimatedLight>();
                    if (data.on_setOpen) al.TurnOn();
                    if (data.on_setClosed) al.TurnOff();
                    if (data.on_setInteractable) al.isInteractable = data.on_setInteractable;
                }
                if (comp.GetType() == typeof(Tapestry_ItemSource))
                {
                    Tapestry_ItemSource i = target.GetComponent<Tapestry_ItemSource>();
                    if (data.on_setInteractable) i.isInteractable = data.on_setInteractable;
                }
            }
        }
        else
            Debug.Log("TAPESTRY WARNING: Switch does not have a target object. Did you forget to set one?");
    }

    public void EvaluateOffState()
    {
        hasFired = true;
        isOn = false;
        if (target != null)
        {
            Tapestry_Activatable comp = target.GetComponent<Tapestry_Activatable>();
            if (comp.GetType() == typeof(Tapestry_Door))
            {
                Tapestry_Door d = target.GetComponent<Tapestry_Door>();
                if (data.off_setOpen) d.Open();
                if (data.off_setClosed) d.Close();
                if (data.off_setLocked) d.security.isLocked = true;
                if (data.off_setUnlocked) d.security.isLocked = false;
                if (data.off_setBypassable) d.security.canBeBypassed = data.off_setBypassable;
                if (data.off_setInteractable) d.isInteractable = data.off_setInteractable;
            }
            if (comp.GetType() == typeof(Tapestry_AnimatedLight))
            {
                Tapestry_AnimatedLight al = target.GetComponent<Tapestry_AnimatedLight>();
                if (data.off_setOpen) al.TurnOn();
                if (data.off_setClosed) al.TurnOff();
                if (data.off_setInteractable) al.isInteractable = data.on_setInteractable;
            }
        }
        else
            Debug.Log("TAPESTRY WARNING: Switch does not have a target object. Did you forget to set one?");
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
