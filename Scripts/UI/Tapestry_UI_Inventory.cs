using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tapestry_UI_Inventory : MonoBehaviour {
    
    public Tapestry_UI_InventoryDisplayTextElement displayPrefab;
    public Tapestry_UI_InventoryDisplay 
        left,
        right;
    private bool 
        _isOpen,
        activateLastFrame,
        leftClickLastFrame,
        rightClickLastFrame;
    private Tapestry_EquipmentProfile 
        equipment;
    private Tapestry_Inventory
        leftInv,
        rightInv;
    private string
        leftName,
        rightName;
    private Tapestry_Player player;

    public bool IsOpen
    {
        get
        {
            return _isOpen;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        HandleInteract();
	}

    public void HandleInteract()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButton(1);
        bool activate = 
            Input.GetKey(Tapestry_Config.KeyboardInput_Activate) ||
            leftClick || rightClick;

        if (activateLastFrame && !activate)
        {
            Tapestry_UI_InventoryDisplayTextElement active = null;
            sbyte side = 0;
            foreach(Tapestry_UI_InventoryDisplayTextElement e in left.elements)
            {
                if(e.Active)
                {
                    active = e;
                    side = -1;
                    break;
                }
            }
            if(side != -1)
            {
                foreach (Tapestry_UI_InventoryDisplayTextElement e in right.elements)
                {
                    if (e.Active)
                    {
                        active = e;
                        side = 1;
                        break;
                    }
                }
            }
            if(active != null)
            {
                //Debug.Log("Clicked on \"" + active.title.text + "\", side="+side);

                if (side == 1)
                {
                    leftInv.AddItem(active.GetData(), 1);
                    rightInv.RemoveItem(active.GetData(), 1);
                    Open(leftInv, equipment, rightInv, leftName, rightName);
                }
                else if (side == -1)
                {
                    if (ReferenceEquals(rightInv, null))
                    {
                        if (active.GetData().isHoldable)
                        {
                            if (active.isEquipment)
                            {
                                if (active.equippedInSlot == Tapestry_EquipSlot.LeftHand)
                                {
                                    if(rightClickLastFrame)
                                    {
                                        Debug.Log("Right clicked item in left slot");
                                        if (equipment.GetInSlot(Tapestry_EquipSlot.RightHand) != null)
                                        {
                                            Debug.Log("Thar be a thingus");
                                            player.Unequip(Tapestry_EquipSlot.RightHand);
                                            player.Equip(equipment.GetInSlot(Tapestry_EquipSlot.LeftHand), Tapestry_EquipSlot.RightHand);
                                            player.Unequip(Tapestry_EquipSlot.LeftHand);
                                            Open(leftInv, equipment, rightInv, leftName, rightName);
                                        }
                                        else
                                        {
                                            Tapestry_ItemData swap = equipment.GetInSlot(Tapestry_EquipSlot.LeftHand);
                                            player.Unequip(Tapestry_EquipSlot.LeftHand);
                                            player.Equip(swap, Tapestry_EquipSlot.RightHand);
                                            Open(leftInv, equipment, rightInv, leftName, rightName);
                                        }
                                    }
                                    else if (activateLastFrame || leftClickLastFrame)
                                    {
                                        Debug.Log("Left clicked item in left slot");
                                        player.Unequip(Tapestry_EquipSlot.LeftHand);
                                        Open(leftInv, equipment, rightInv, leftName, rightName);
                                    }
                                }
                                else if (active.equippedInSlot == Tapestry_EquipSlot.RightHand)
                                {
                                    if (leftClickLastFrame)
                                    {
                                        if (equipment.GetInSlot(Tapestry_EquipSlot.LeftHand) != null)
                                        {
                                            player.Unequip(Tapestry_EquipSlot.LeftHand);
                                            player.Equip(equipment.GetInSlot(Tapestry_EquipSlot.RightHand), Tapestry_EquipSlot.LeftHand);
                                            player.Unequip(Tapestry_EquipSlot.RightHand);
                                            Open(leftInv, equipment, rightInv, leftName, rightName);
                                        }
                                        else
                                        {
                                            Tapestry_ItemData swap = equipment.GetInSlot(Tapestry_EquipSlot.RightHand);
                                            player.Unequip(Tapestry_EquipSlot.RightHand);
                                            player.Equip(swap, Tapestry_EquipSlot.LeftHand);
                                            Open(leftInv, equipment, rightInv, leftName, rightName);
                                        }
                                    }
                                    else if (activateLastFrame || rightClickLastFrame)
                                    {
                                        player.Unequip(Tapestry_EquipSlot.RightHand);
                                        Open(leftInv, equipment, rightInv, leftName, rightName);
                                    }
                                }
                            }
                            else
                            {
                                if (active.GetData().slot == Tapestry_EquipSlot.EitherHand && leftClickLastFrame)
                                {
                                    player.Equip(active.GetData(), Tapestry_EquipSlot.LeftHand);
                                    active.SetEquippedState(1);
                                    active.equippedInSlot = Tapestry_EquipSlot.LeftHand;
                                    Open(leftInv, equipment, rightInv, leftName, rightName);
                                }
                                else if (active.GetData().slot == Tapestry_EquipSlot.EitherHand && rightClickLastFrame)
                                {
                                    player.Equip(active.GetData(), Tapestry_EquipSlot.RightHand);
                                    active.SetEquippedState(2);
                                    active.equippedInSlot = Tapestry_EquipSlot.RightHand;
                                    Open(leftInv, equipment, rightInv, leftName, rightName);
                                }
                                else if (active.GetData().slot == Tapestry_EquipSlot.BothHands && (leftClickLastFrame || rightClickLastFrame))
                                {
                                    player.Equip(active.GetData(), Tapestry_EquipSlot.BothHands);
                                    active.SetEquippedState(3);
                                    active.equippedInSlot = Tapestry_EquipSlot.BothHands;
                                    Open(leftInv, equipment, rightInv, leftName, rightName);
                                }
                                //foreach(Tapestry_UI_InventoryDisplayTextElement te in left.elements)
                                //{
                                //    if(te.GetData() != player.equippedLeft && te.GetData() != player.equippedRight)
                                //        te.SetEquippedState(0);
                                //}
                            }
                        }
                        else
                        {
                            if (active.GetData().useEffect)
                            {
                                player.AddEffect(active.GetData().effect);
                                leftInv.RemoveItem(active.GetData(), 1);
                                Open(leftInv, equipment, rightInv, leftName, rightName);
                            }
                        }
                    }
                    else
                    {
                        rightInv.AddItem(active.GetData(), 1);
                        leftInv.RemoveItem(active.GetData(), 1);
                        Open(leftInv, null, rightInv, leftName, rightName);
                    }
                }
            }
        }

        //End of frame
        activateLastFrame = activate;
        leftClickLastFrame = leftClick;
        rightClickLastFrame = rightClick;
    }

    public void Open(Tapestry_Inventory _leftInv, Tapestry_EquipmentProfile _equip, Tapestry_Inventory _rightInv = null, string _leftName = "Inventory", string _rightName = "Target")
    {
        left.Clear();
        right.Clear();
        this.gameObject.SetActive(true);
        _isOpen = true;
        Tapestry_WorldClock.IsPaused = true;
        left.Init(displayPrefab, _leftInv, _leftName, _equip);
        leftInv = _leftInv;
        leftName = _leftName;
        player = FindObjectOfType<Tapestry_Player>();
        equipment = _equip;

        if (_rightInv == null)
        {
            right.gameObject.SetActive(false);
        }
        else
        {
            right.gameObject.SetActive(true);
            right.Init(displayPrefab, _rightInv, _rightName);
            rightInv = _rightInv;
            rightName = _rightName;
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        _isOpen = false;
        Tapestry_WorldClock.IsPaused = false;
        left.Clear();
        right.Clear();
        leftInv = null;
        rightInv = null;
        equipment = null;
    }
}
