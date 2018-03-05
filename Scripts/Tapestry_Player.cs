using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Player : Tapestry_Entity {

    private bool
        runToggleLastFrame,
        activateLastFrame;
    public bool allowCameraMovement = true;
    public Tapestry_Activatable objectInSights;

    protected override void Reset()
    {
        base.Reset();
    }

    // Use this for initialization
    void Start ()
    {
        base.Reset();
        isRunning = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleMouselook();
        HandleActivation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
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

    private void HandleActivation()
    {
        bool activate = Input.GetKey(Tapestry_Config.KeyboardInput_Activate);
        objectInSights = null;

        RaycastHit hit;
        bool rayHit = Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            Tapestry_Config.EntityActivationDistance,
            ~LayerMask.GetMask("Ignore Raycast")
            );
        if(rayHit)
        {
            objectInSights = hit.transform.gameObject.GetComponentInParent<Tapestry_Activatable>();

            if (objectInSights != null)
            {
                objectInSights.Hover();

                if(activateLastFrame && !activate && objectInSights.GetComponent<Tapestry_Activatable>().isInteractable)
                {
                    if ((objectInSights.GetType() == typeof(Tapestry_Item)) || 
                        (objectInSights.GetType() == typeof(Tapestry_ItemKey)))
                    {
                        Tapestry_Item i = (Tapestry_Item)objectInSights;
                        if (inventory == null)
                            inventory = new Tapestry_Inventory(this.transform);

                        inventory.AddItem(i, 1);
                        objectInSights.Activate(this);
                    }
                    else if (objectInSights.GetType() == typeof(Tapestry_Door))
                    {
                        Tapestry_Door d = (Tapestry_Door)objectInSights;
                        if(d.security.isLocked)
                        {
                            if (inventory == null)
                                inventory = new Tapestry_Inventory(this.transform);

                            if (!d.GetIsOpen())
                            {
                                if (inventory.ContainsKeyID(d.security.keyID))
                                {
                                    d.security.isLocked = false;
                                    if(d.consumeKeyOnUnlock)
                                    {
                                        inventory.RemoveKeyWithID(d.security.keyID);
                                    }
                                }
                                objectInSights.Activate(this);
                            }
                        }
                        else
                            objectInSights.Activate(this);
                    }
                    else
                        objectInSights.Activate(this);
                }
            }
        }

        //End of frame
        activateLastFrame = activate;
    }

    private void HandleMouselook()
    {
        if (allowCameraMovement)
        {
            if (!Tapestry_WorldClock.isPaused)
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
    }

    private void HandleMovement()
    {
        if (!Tapestry_WorldClock.isPaused)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            bool runToggleThisFrame = Input.GetKey(Tapestry_Config.KeyboardInput_RunWalkToggle);
            if (!runToggleThisFrame && runToggleLastFrame)
            {
                isRunning = !isRunning;
            }
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

            if (isRunning)
                direction = direction.normalized * Tapestry_Config.BaseEntityRunSpeed;
            else
                direction = direction.normalized * Tapestry_Config.BaseEntityWalkSpeed;

            rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.y);

            //end of frame
            runToggleLastFrame = runToggleThisFrame;
        }
    }

    public Tapestry_Entity ClonePlayerAsEntity()
    {
        Debug.Log("TODO: ClonePlayerAsEntity");
        return new Tapestry_Entity();
    }
}
