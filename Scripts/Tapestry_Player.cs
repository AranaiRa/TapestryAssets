using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Player : Tapestry_Entity {

    public bool RestrictControls
    {
        get
        {
            if (timeUntilControlsReturned > 0)
                return true;
            else
                return false;
        }

        set
        {
            if (value == true)
                timeUntilControlsReturned = 0.5f;
            else if (value == false)
                timeUntilControlsReturned = 0.0f;
        }
    }

    public bool 
        allowCameraMovement = true,
        allowInputMovement = true;
    public Tapestry_Activatable objectInSights;
    public Tapestry_UI_Inventory inventoryUI;
    public Tapestry_ItemData
        equippedLeft,
        equippedRight;
    public GameObject
        equippedItemContainer;
    public Tapestry_ActorValue 
        reach;

    private bool
        runToggleLastFrame,
        activateLastFrame,
        pushLastFrame,
        liftLastFrame,
        openLastFrame,
        hideHeldLastFrame,
        jumpLastFrame,
        lmbLastFrame,
        rmbLastFrame,
        isHidingOrShowing,
        isHeldItemsHidden;
    private float 
        heldItemXDecay,
        hideTime,
        timeUntilControlsReturned;
    private Vector3
        heldItemStartingPos,
        heldItemHiddenPos = new Vector3(0, -.65f, -0.5f);
    private Quaternion
        heldItemStartingRot,
        heldItemHiddenRot = Quaternion.Euler(90, 0, 0);

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
        if (ReferenceEquals(inventory, null))
            inventory = (Tapestry_Inventory)ScriptableObject.CreateInstance("Tapestry_Inventory");
        if (damageProfile == null)
            damageProfile = new Tapestry_DamageProfile();
        if(attributeProfile == null)
            attributeProfile = new Tapestry_AttributeProfile();
        if(skillProfile == null)
            skillProfile = new Tapestry_SkillProfile();
        if(keywords == null)
            keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");
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
        heldItemStartingPos = equippedItemContainer.transform.localPosition;
        heldItemStartingRot = equippedItemContainer.transform.localRotation;
    }

	// Update is called once per frame
	protected override void Update ()
    {
        if (!Tapestry_WorldClock.IsPaused)
        {
            HandleMouselook();
            HandleActivation();
            HandleHideHeld();
        }
        HandleInventory();
        base.Update();

        if (timeUntilControlsReturned > 0 && !Tapestry_WorldClock.IsPaused)
            timeUntilControlsReturned -= Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleMovement();
        HandleJump();
    }

    public override void InitializeActorValues()
    {
        base.InitializeActorValues();
        if(ReferenceEquals(reach, null))
            reach = (Tapestry_ActorValue)ScriptableObject.CreateInstance("Tapestry_ActorValue");

        jumpPower.AdjustBaseValue(4.0f);
        reach.AdjustBaseValue(Tapestry_Config.EntityActivationDistance);
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
        bool push = Input.GetKey(Tapestry_Config.KeyboardInput_Push);
        bool lift = Input.GetKey(Tapestry_Config.KeyboardInput_Lift);
        objectInSights = null;

        RaycastHit hit;
        bool rayHit = Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            reach.Value,
            ~(LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("Tapestry Held Items"))
            );
        if (rayHit)
        {
            objectInSights = hit.transform.gameObject.GetComponentInParent<Tapestry_Activatable>();

            if (objectInSights != null)
            {
                objectInSights.Hover();

                if (activateLastFrame && !activate && objectInSights.GetComponent<Tapestry_Activatable>().isInteractable)
                {
                    if ((typeof(Tapestry_Item).IsAssignableFrom(objectInSights.GetType())))
                    {
                        Tapestry_Item i = (Tapestry_Item)objectInSights;
                        if (ReferenceEquals(inventory, null))
                            inventory = (Tapestry_Inventory)ScriptableObject.CreateInstance("Tapestry_Inventory");

                        inventory.AddItem(i, 1);
                        objectInSights.Activate(this);
                    }
                    else if (objectInSights.GetType() == typeof(Tapestry_Door))
                    {
                        Tapestry_Door d = (Tapestry_Door)objectInSights;
                        if (d.security.isLocked)
                        {
                            if (ReferenceEquals(inventory, null))
                                inventory = (Tapestry_Inventory)ScriptableObject.CreateInstance("Tapestry_Inventory");

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
                        inventoryUI.Open(inventory, null, c.inventory, "Inventory", c.displayName);
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
            float invertY = 1;
            if (Tapestry_Config.InvertPlayerCameraY)
                invertY = -1;
            float rotVert = Camera.main.transform.localRotation.eulerAngles.x;
            rotVert += Input.GetAxis("Mouse Y") * Tapestry_Config.PlayerCameraSensitivityY * invertY;
            float invertX = 1;
            if (Tapestry_Config.InvertPlayerCameraX)
                invertX = -1;
            Camera.main.transform.localRotation = Quaternion.Euler(rotVert, 0, 0);
            float rotVertReduced;
            if (rotVert < 180)
                rotVertReduced = rotVert / 15f;
            else
                rotVertReduced = (rotVert - 360f) / 15f;
            equippedItemContainer.transform.localRotation = Quaternion.Euler(-rotVertReduced, 0, 0);
            
            if (!isPushing)
            {
                float rotHori = transform.rotation.eulerAngles.y;
                rotHori += Input.GetAxis("Mouse X") * Tapestry_Config.PlayerCameraSensitivityX * invertX;
                transform.rotation = Quaternion.Euler(0, rotHori, 0);
                heldItemXDecay -= Input.GetAxis("Mouse X") * Tapestry_Config.PlayerCameraSensitivityX * invertX / 9f;
            }
            equippedItemContainer.transform.localRotation = Quaternion.Euler(-rotVertReduced, heldItemXDecay, 0);
            heldItemXDecay *= 0.85f;

            //Controller
        }
    }

    private void HandleMovement()
    {
        if (!Tapestry_WorldClock.IsPaused && isGrounded && timeUntilControlsReturned <= 0)
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
            bool lmb =
                Input.GetKey(Tapestry_Config.KeyboardInput_LeftHand) ||
                Input.GetKey(Tapestry_Config.ControllerInput_LeftHand);
            bool rmb =
                Input.GetKey(Tapestry_Config.KeyboardInput_LeftHand) ||
                Input.GetKey(Tapestry_Config.ControllerInput_LeftHand);

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

            //pass controls to item use function
            HandleHeldItemUse(fwd, bck, lft, rgt, !lmb && lmbLastFrame, !rmb && rmbLastFrame);

            //end of frame
            runToggleLastFrame = runToggleThisFrame;
            lmbLastFrame = lmb;
            rmbLastFrame = rmb;
        }
        //else
        //    Debug.Log("movement restricted this frame; impulse = "+GetComponent<Rigidbody>().velocity);
    }

    private void HandleJump()
    {
        if (!Tapestry_WorldClock.IsPaused && isGrounded && timeUntilControlsReturned <= 0)
        {
            bool jump = Input.GetKeyDown(Tapestry_Config.KeyboardInput_Jump) || Input.GetKeyDown(Tapestry_Config.KeyboardInput_Jump);

            if (!jump && jumpLastFrame)
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                isGrounded = false;

                rb.velocity = new Vector3(rb.velocity.x, jumpPower.Value, rb.velocity.z);
            }

            //end of frame
            jumpLastFrame = jump;
        }
    }

    private void HandleInventory()
    {
        if (ReferenceEquals(inventoryUI, null))
        {
            inventoryUI = FindObjectOfType<Tapestry_UI_Inventory>();
        }
        if (ReferenceEquals(equipmentProfile, null))
            equipmentProfile = (Tapestry_EquipmentProfile)ScriptableObject.CreateInstance("Tapestry_EquipmentProfile");

        bool open = Input.GetKey(Tapestry_Config.KeyboardInput_Inventory);

        if (openLastFrame && !open)
        {
            if (inventoryUI.IsOpen)
                inventoryUI.Close();
            else
                inventoryUI.Open(inventory, equipmentProfile);
        }
        if(Input.GetKey(Tapestry_Config.KeyboardInput_Cancel))
        {
            if (inventoryUI.IsOpen)
                inventoryUI.Close();
        }

        //End of frame
        openLastFrame = open;
    }

    private void HandleHideHeld()
    {
        bool swap = Input.GetKey(Tapestry_Config.KeyboardInput_Swap);

        if(hideHeldLastFrame && !isHidingOrShowing)
        {
            hideTime = Tapestry_Config.InventoryItemHideTime;
            isHidingOrShowing = true;
            equippedItemContainer.SetActive(true);
        }
        else if(!Tapestry_WorldClock.IsPaused && isHidingOrShowing)
        {
            hideTime -= Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor * personalTimeFactor;
            if (!isHeldItemsHidden)
            {
                float prog = hideTime / Tapestry_Config.InventoryItemHideTime;
                equippedItemContainer.transform.localPosition = Vector3.Lerp(heldItemHiddenPos, heldItemStartingPos, prog);
                equippedItemContainer.transform.localRotation = Quaternion.Lerp(heldItemHiddenRot, heldItemStartingRot, prog);
                if (prog <= 0)
                {
                    equippedItemContainer.SetActive(false);
                    isHeldItemsHidden = true;
                    isHidingOrShowing = false;
                }
            }
            else
            {
                float prog = hideTime / Tapestry_Config.InventoryItemHideTime;
                equippedItemContainer.transform.localPosition = Vector3.Lerp(heldItemStartingPos, heldItemHiddenPos, prog);
                equippedItemContainer.transform.localRotation = Quaternion.Lerp(heldItemStartingRot, heldItemHiddenRot, prog);
                if (prog <= 0)
                {
                    isHeldItemsHidden = false;
                    isHidingOrShowing = false;
                }
            }
        }

        //end of frame
        hideHeldLastFrame = swap;
    }

    private void HandleHeldItemUse(bool fwd, bool rev, bool lft, bool rgt, bool lmb, bool rmb)
    {
        if (!isHeldItemsHidden && lmb)
        {
            if (holdContainerLeft.transform.childCount != 0)
            {
                Transform heldTransform = holdContainerLeft.transform.GetChild(0);
                if(heldTransform.gameObject.GetComponent<Tapestry_ItemWeaponMelee>() != null)
                {
                    Tapestry_ItemWeaponMelee held = heldTransform.gameObject.GetComponent<Tapestry_ItemWeaponMelee>();
                    held.AttackStanding();
                }
            }
        }
    }

    public override void Equip(Tapestry_ItemData item, Tapestry_EquipSlot slot)
    {
        base.Equip(item, slot);

        if (slot == Tapestry_EquipSlot.LeftHand || slot == Tapestry_EquipSlot.RightHand)
        {
            GameObject obj = (GameObject)Instantiate(Resources.Load("Items/"+item.prefabName));

            //TODO: Dupe bug when switching hands
            if (slot == Tapestry_EquipSlot.LeftHand)
            {
                CleanEquippedItem(Tapestry_EquipSlot.LeftHand);
                obj.transform.SetParent(holdContainerLeft.transform);
                if (equippedLeft != null)
                    equippedLeft = item;
                else
                {
                    inventory.AddItem(equippedLeft, 1);
                    equippedLeft = item;
                }
            }
            if (slot == Tapestry_EquipSlot.RightHand)
            {
                CleanEquippedItem(Tapestry_EquipSlot.RightHand);
                obj.transform.SetParent(holdContainerRight.transform);
                if (equippedRight != null)
                    equippedRight = item;
                else
                {
                    inventory.AddItem(equippedLeft, 1);
                    equippedLeft = item;
                }
            }
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            obj.layer = 8;
            foreach (Transform t in obj.transform)
            {
                t.gameObject.layer = 8;
            }
            foreach (Collider c in obj.GetComponentsInChildren<Collider>())
            {
                if(!c.isTrigger)
                    c.enabled = false;
            }
            foreach (Rigidbody rb in obj.GetComponentsInChildren<Rigidbody>())
            {
                Destroy(rb);
            }
        }
    }

    public override void Unequip(Tapestry_EquipSlot slot)
    {
        CleanEquippedItem(slot);
        base.Unequip(slot);
    }

    public override void UnequipAndDestroy(Tapestry_EquipSlot slot)
    {
        CleanEquippedItem(slot);
        base.UnequipAndDestroy(slot);
    }

    private void CleanEquippedItem(Tapestry_EquipSlot slot)
    {
        if(slot == Tapestry_EquipSlot.LeftHand)
        {
            foreach(Transform child in holdContainerLeft.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (slot == Tapestry_EquipSlot.RightHand)
        {
            foreach (Transform child in holdContainerRight.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
