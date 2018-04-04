﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Player : Tapestry_Entity {

    private bool
        runToggleLastFrame,
        activateLastFrame,
        pushLastFrame,
        liftLastFrame,
        openLastFrame;
    public bool allowCameraMovement = true;
    public Tapestry_Activatable objectInSights;
    public Tapestry_UI_Inventory inventoryUI;

    protected override void Reset()
    {
        if (effects == null)
            effects = new List<Tapestry_Effect>();
        inventoryUI = FindObjectOfType<Tapestry_Level>().inventoryUI;
        base.Reset();
    }

    // Use this for initialization
    void Start ()
    {
        inventoryUI = FindObjectOfType<Tapestry_Level>().inventoryUI;
        if (inventory == null)
            inventory = new Tapestry_Inventory(this.transform);
        if(damageProfile == null)
            damageProfile = new Tapestry_DamageProfile();
        if(attributeProfile == null)
            attributeProfile = new Tapestry_AttributeProfile();
        if(skillProfile == null)
            skillProfile = new Tapestry_SkillProfile();
        if(keywords == null)
            keywords = new List<string>();
        if (effects == null)
            effects = new List<Tapestry_Effect>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Points")
            {
                GameObject pointContainer = transform.GetChild(i).gameObject;
                for (int j = 0; j < pointContainer.transform.childCount; j++)
                {
                    if (pointContainer.transform.GetChild(j).name == "P_Attach")
                    {
                        attachPoint = pointContainer.transform.GetChild(j).gameObject;
                    }
                }
            }
        }
        isRunning = true;
	}
    bool debugLastFrame;
	// Update is called once per frame
	protected override void Update ()
    {
        if (!Tapestry_WorldClock.isPaused)
        {
            HandleMouselook();
            HandleActivation();
        }
        HandleInventory();
        HandlePlayerEffects();
        base.Update();

        //TEMP
        bool debug = Input.GetKey(KeyCode.Space);

        if (debugLastFrame && !debug)
        {
            Tapestry_Effect e = new Tapestry_Effect(this.transform);
            e.delivery = new Tapestry_EffectBuilder_Delivery_Ray();
            e.duration = Tapestry_EffectBuilder_Duration.Instant;
            e.payload = new Tapestry_EffectBuilder_Payload_Damage();

            List<Tapestry_Actor> targets = e.delivery.GetAffectedTargets();

            foreach(Tapestry_Actor t in targets)
            {
                Debug.Log("post: hit " + t.name);
                e.target = t;
                t.AddEffect(e);
            }
        }

        debugLastFrame = debug;
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

    private void HandlePlayerEffects()
    {

    }

    private void HandleActivation()
    {
        bool activate = Input.GetKey(Tapestry_Config.KeyboardInput_Activate);
        bool push = Input.GetKey(Tapestry_Config.KeyboardInput_Push);
        bool lift = Input.GetKey(Tapestry_Config.KeyboardInput_Lift);
        objectInSights = null;

        RaycastHit hit;
        bool rayHit = Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            Tapestry_Config.EntityActivationDistance,
            ~LayerMask.GetMask("Ignore Raycast")
            );
        if (rayHit)
        {
            objectInSights = hit.transform.gameObject.GetComponentInParent<Tapestry_Activatable>();

            if (objectInSights != null)
            {
                objectInSights.Hover();

                if (activateLastFrame && !activate && objectInSights.GetComponent<Tapestry_Activatable>().isInteractable)
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
                        if (d.security.isLocked)
                        {
                            if (inventory == null)
                                inventory = new Tapestry_Inventory(this.transform);

                            if (!d.GetIsOpen())
                            {
                                if (inventory.ContainsKeyID(d.security.keyID))
                                {
                                    d.security.isLocked = false;
                                    if (d.consumeKeyOnUnlock)
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
                    else if (objectInSights.GetType() == typeof(Tapestry_Container))
                    {
                        Tapestry_Container c = (Tapestry_Container)objectInSights;
                        if (inventoryUI == null)
                            inventoryUI = FindObjectOfType<Tapestry_UI_Inventory>();
                        
                        Debug.Log(c.inventory.items.Count + " in target container.");
                        inventoryUI.Open(inventory, c.inventory, "Inventory", c.displayName);
                    }
                    else
                        objectInSights.Activate(this);
                }
                if (pushLastFrame && !push && objectInSights.GetComponent<Tapestry_Activatable>().isPushable)
                {
                    objectInSights.Push(this);
                }
                if (liftLastFrame && !lift && objectInSights.GetComponent<Tapestry_Activatable>().isLiftable)
                {
                    objectInSights.Lift(this);
                }
            }
        }

        //End of frame
        activateLastFrame = activate;
        pushLastFrame = push;
        liftLastFrame = lift;
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

            if (!isPushing)
            {
                float rotHori = transform.rotation.eulerAngles.y;
                rotHori += Input.GetAxis("Mouse X") * Tapestry_Config.PlayerCameraSensitivityX * invertY;
                transform.rotation = Quaternion.Euler(0, rotHori, 0);
            }

            //Controller
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
                direction = direction.normalized * Tapestry_Config.BaseEntityRunSpeed * personalTimeFactor;
            else
                direction = direction.normalized * Tapestry_Config.BaseEntityWalkSpeed * personalTimeFactor;

            rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.y);

            //end of frame
            runToggleLastFrame = runToggleThisFrame;
        }
    }

    private void HandleInventory()
    {
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<Tapestry_Level>().inventoryUI;

        bool open = Input.GetKey(Tapestry_Config.KeyboardInput_Inventory);

        if (openLastFrame && !open)
        {
            if (inventoryUI.IsOpen)
                inventoryUI.Close();
            else
                inventoryUI.Open(inventory);
        }
        if(Input.GetKey(Tapestry_Config.KeyboardInput_Cancel))
        {
            if (inventoryUI.IsOpen)
                inventoryUI.Close();
        }

        //End of frame
        openLastFrame = open;
    }

    public Tapestry_Entity ClonePlayerAsEntity()
    {
        Debug.Log("TODO: ClonePlayerAsEntity");
        return new Tapestry_Entity();
    }
}
