using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Player : Tapestry_Entity {

    private bool
        runToggleLastFrame;
    private Vector3
        momentum;
    public bool allowCameraMovement = true;

    protected override void Reset()
    {
        base.Reset();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        velocity2D.x = rb.velocity.x;
        velocity2D.y = rb.velocity.z;
        speed2D = velocity2D.magnitude;
        speed3D = rb.velocity.magnitude;

        bool runToggleThisFrame = Input.GetKey(Tapestry_Config.KeyboardInput_RunWalkToggle);
        if (!runToggleThisFrame && runToggleLastFrame)
        {
            isRunning = !isRunning;
        }

        HandleMouselook();
        HandleMovement();

        //end of frame
        runToggleLastFrame = runToggleThisFrame;
    }

    private Vector2 GetFwd2D()
    {
        Vector2 dir;
        dir.x = transform.forward.x;
        dir.y = transform.forward.z;
        return dir.normalized;
    }

    private Vector2 GetRight2D()
    {
        Vector2 dir;
        dir.x = transform.right.x;
        dir.y = transform.right.z;
        return dir.normalized;
    }

    private void HandleMouselook()
    {
        if (allowCameraMovement)
        {
            //Mouselook
            float invertX = 1;
            if (Tapestry_Config.InvertPlayerCameraY)
                invertX = -1;
            float rotVert = Camera.main.transform.localRotation.eulerAngles.x;
            rotVert += Input.GetAxis("Mouse Y") * Tapestry_Config.PlayerCameraSensitivityY * invertX;

            float invertY = 1;
            if (Tapestry_Config.InvertPlayerCameraX)
                invertY = -1;
            Camera.main.transform.localRotation = Quaternion.Euler(rotVert, 0, 0);
            float rotHori = transform.rotation.eulerAngles.y;
            rotHori += Input.GetAxis("Mouse X") * Tapestry_Config.PlayerCameraSensitivityX * invertY;
            transform.rotation = Quaternion.Euler(0, rotHori, 0);

            //Controller
        }
    }

    private void HandleMovement()
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

        Vector2 direction = new Vector2();

        if (fwd && !bck)
            direction += GetFwd2D();
        else if (bck && !fwd)
            direction -= GetFwd2D();

        if (rgt && !lft)
            direction += GetRight2D();
        else if (lft && !rgt)
            direction -= GetRight2D();

        if (fwd || bck || lft || rgt)
        {
            if (isRunning)
                direction = direction.normalized * Tapestry_Config.BaseEntityRunSpeed;
            else
                direction = direction.normalized * Tapestry_Config.BaseEntityWalkSpeed;

            Vector2 diff = direction - velocity2D;

            momentum = new Vector3(diff.x, 0, diff.y);
        }

        rb.AddForce(momentum, ForceMode.VelocityChange);

        momentum *= 0.0f;
    }
}
