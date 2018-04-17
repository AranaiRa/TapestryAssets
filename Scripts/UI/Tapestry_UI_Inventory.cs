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
        activateLastFrame;
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
        bool activate = 
            Input.GetKey(Tapestry_Config.KeyboardInput_Activate) ||
            Input.GetMouseButton(0);

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
                    Open(leftInv, rightInv, leftName, rightName);
                }
                else if (side == -1)
                {
                    if (rightInv == null)
                    {
                        //Debug.Log("No right inventory");
                        if (active.GetData().isHoldable)
                        {
                            //Debug.Log("Is a holdable");
                            player.Equip(active.GetData(), Tapestry_EquipSlot.LeftHand);
                            active.SetEquippedState(1);
                            foreach(Tapestry_UI_InventoryDisplayTextElement te in left.elements)
                            {
                                if(te.GetData() != player.equippedLeft && te.GetData() != player.equippedRight)
                                    te.SetEquippedState(0);
                            }
                        }
                        else
                        {
                            //Debug.Log("Is a normal");
                            if (active.GetData().useEffect)
                            {
                                player.AddEffect(active.GetData().effect);
                                leftInv.RemoveItem(active.GetData(), 1);
                                Open(leftInv, rightInv, leftName, rightName);
                            }
                        }
                    }
                    else
                    {
                        rightInv.AddItem(active.GetData(), 1);
                        leftInv.RemoveItem(active.GetData(), 1);
                        Open(leftInv, rightInv, leftName, rightName);
                    }
                }
            }
        }

        //End of frame
        activateLastFrame = activate;
    }

    public void Open(Tapestry_Inventory _leftInv, Tapestry_Inventory _rightInv = null, string _leftName = "Inventory", string _rightName = "Target")
    {
        left.Clear();
        right.Clear();
        this.gameObject.SetActive(true);
        _isOpen = true;
        Tapestry_WorldClock.isPaused = true;
        left.Init(displayPrefab, _leftInv, _leftName);
        leftInv = _leftInv;
        leftName = _leftName;
        player = FindObjectOfType<Tapestry_Player>();

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

        foreach(Tapestry_UI_InventoryDisplayTextElement te in left.elements)
        {
            if (player.equippedLeft != null && te.GetData() != null)
            {
                Debug.Log("Comparing " + te.GetData().displayName + " to " + player.equippedLeft.displayName);
                if (te.GetData().Equals(player.equippedLeft))
                {
                    Debug.Log("It's a match!");
                    player.Equip(te.GetData(), Tapestry_EquipSlot.LeftHand);
                    te.SetEquippedState(1);
                }
            }
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        _isOpen = false;
        Tapestry_WorldClock.isPaused = false;
        left.Clear();
        right.Clear();
        leftInv = null;
        rightInv = null;
    }
}
