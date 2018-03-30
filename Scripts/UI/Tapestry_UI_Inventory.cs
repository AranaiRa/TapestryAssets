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

    public bool IsOpen
    {
        get
        {
            return _isOpen;
        }
    }

    private void Start()
    {

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
                Debug.Log("Clicked on \"" + active.title.text + "\"");

                if (side == 1)
                {
                    leftInv.AddItem(active.GetData(), 1);
                    rightInv.RemoveItem(active.GetData(), 1);
                    Open(leftInv, rightInv, leftName, rightName);
                }
                else if (side == -1 && rightInv != null)
                {
                    rightInv.AddItem(active.GetData(), 1);
                    leftInv.RemoveItem(active.GetData(), 1);
                    Open(leftInv, rightInv, leftName, rightName);
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

        if(_rightInv == null)
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
        Tapestry_WorldClock.isPaused = false;
        left.Clear();
        right.Clear();
        leftInv = null;
        rightInv = null;
    }
}
