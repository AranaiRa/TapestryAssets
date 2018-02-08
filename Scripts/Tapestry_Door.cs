using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Door : Tapestry_Activatable {

    public GameObject pivot;
    public Tapestry_Lock security;
    public float openTime = 0.7f;
    public AnimationCurve curve;

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
        isOpen = false;
    private float time;

    private void Reset()
    {
        displayName = "Door";
        curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        security = new Tapestry_Lock(false, 0, "");

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
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isOpening)
        {
            time += Time.deltaTime;
            if (time >= openTime)
                time = openTime;
            float prog = curve.Evaluate(time / openTime);
            prog = Mathf.Clamp(prog, 0, 1);
            //Debug.Log("["+Mathf.RoundToInt(time*100/openTime)+"%] "+prog);

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
        // TODO: Needs updating after the opening block's fuckery is figured out
        else if (isClosing)
        {
            /*time += Time.deltaTime;
            float prog = curve.Evaluate(time / openTime);
            if (prog > 1) prog = 1;
            if (prog < 0) prog = 0;

            Vector3 pos = Vector3.Lerp(pivot.transform.localPosition, pos1, prog);
            Quaternion rot = Quaternion.Lerp(pivot.transform.localRotation, rot1, prog);

            pivot.transform.localPosition = pos;
            pivot.transform.localRotation = rot;

            if (prog == 1)
            {
                isClosing = false;
                isOpen = false;
            }*/

            time += Time.deltaTime;
            if (time >= openTime)
                time = openTime;
            float prog = curve.Evaluate(time / openTime);
            if (prog > 1) prog = 1;
            if (prog < 0) prog = 0;
            //Debug.Log("["+Mathf.RoundToInt(time*100/openTime)+"%] "+prog);

            Vector3 evalPos = Vector3.Lerp(startingPos, pos1, prog);
            Quaternion evalRot = Quaternion.Lerp(startingRot, rot1, prog);

            pivot.transform.localPosition = evalPos;
            pivot.transform.localRotation = evalRot;

            if (time == openTime)
            {
                isClosing = false;
                isOpen = false;
            }
        }
    }

    public override void Activate()
    {
        if(!isOpening && !isClosing)
        {
            if (isOpen)
                Close();
            else
                Open();
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
}
