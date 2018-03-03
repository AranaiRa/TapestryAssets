using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class Tapestry_UI_Inventory : MonoBehaviour {

    public Tapestry_Player player;
    public GameObject itemDisplayPrefab;
    public Text itemNameText;
    public static float itemRadius = 200.0f;
    public float
        delayTime = 0.16f;
    public bool
        isWindowOpen = false;
    public List<Tapestry_UI_Inventory_ItemDisplay> itemDisplays = new List<Tapestry_UI_Inventory_ItemDisplay>();

    private float
        timeOpenClose,
        timeRotationInputDelay,
        timeRotation,
        currentRot = 0,
        targetRot;
    private int
        selected = 0;
    private bool
        isRotating = false,
        blockDrops = false;
    
	void Start ()
    {
        itemNameText.text = "";
    }
	
	void Update () {
        if (isWindowOpen)
        {
            HandleDisplayRotation();
            HandleUseDrop();
        }
        HandleOpenClose();
	}

    private void HandleDisplayRotation()
    {
        bool rotLeft =
            Input.GetKey(KeyCode.LeftArrow) &&
            timeRotationInputDelay == 0;
        bool rotRight =
            Input.GetKey(KeyCode.RightArrow) &&
            timeRotationInputDelay == 0;

        if (rotLeft)
        {
            timeRotationInputDelay = delayTime;
            isRotating = true;

            int prev = selected;

            selected--;
            if (selected < 0)
                selected = itemDisplays.Count - 1;

            itemDisplays[prev].FadeOut();
            itemDisplays[selected].FadeIn();

            targetRot = currentRot + ((Mathf.PI * 2) / itemDisplays.Count);
            timeRotation = 0;
        }
        else if (rotRight)
        {
            timeRotationInputDelay = delayTime;
            isRotating = true;

            int prev = selected;

            selected++;
            if (selected >= itemDisplays.Count)
                selected = 0;

            itemDisplays[prev].FadeOut();
            itemDisplays[selected].FadeIn();

            targetRot = currentRot - ((Mathf.PI * 2) / itemDisplays.Count);
            timeRotation = 0;
        }
        else if (timeRotationInputDelay > 0)
        {
            timeRotationInputDelay -= Time.deltaTime;
            if (timeRotationInputDelay < 0) timeRotationInputDelay = 0;
        }

        float theta;
        if (isRotating)
        {
            timeRotation += Time.deltaTime;
            if (timeRotation > delayTime)
            {
                timeRotation = delayTime;
                currentRot = targetRot;
                theta = targetRot;
                isRotating = false;
                itemNameText.text = player.inventory.items[selected].item.displayName;
            }
            theta = Mathf.Lerp(currentRot, targetRot, timeRotation / delayTime);
        }
        else
            theta = currentRot;

        foreach (Tapestry_UI_Inventory_ItemDisplay id in itemDisplays)
        {
            id.UpdatePosition(theta);
        }
    }

    private void HandleUseDrop()
    {
        bool drop =
            Input.GetKey(Tapestry_Config.KeyboardInput_InventoryDrop) &&
            !isRotating;

        if(drop && !blockDrops)
        {
            timeOpenClose = delayTime;
            isWindowOpen = !isWindowOpen;
            blockDrops = true;

            player.inventory.DropItem(player.inventory.items[selected].item);
            Close();
        }
    }

    private void HandleOpenClose()
    {
        if (Input.GetKey(Tapestry_Config.KeyboardInput_Inventory) && timeOpenClose == 0)
        {
            timeOpenClose = delayTime;
            isWindowOpen = !isWindowOpen;

            if (isWindowOpen)
            {
                Open(player.inventory);
            }
            else
            {
                Close();
            }
        }

        timeOpenClose -= Time.deltaTime;
        if (timeOpenClose < 0) timeOpenClose = 0;
    }

    public void Open(Tapestry_Inventory inv)
    {
        Tapestry_WorldClock.isPaused = true;
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
        itemNameText.color = new Color(1, 1, 1, 1);
        itemNameText.text = player.inventory.items[selected].item.displayName;

        int total = inv.items.Count;
        currentRot = 0;
        selected = 0;

        for (int i = 0; i < total; i++)
        {
            GameObject go = GameObject.Instantiate(itemDisplayPrefab);
            go.transform.SetParent(this.transform);
            Tapestry_UI_Inventory_ItemDisplay id = go.GetComponent<Tapestry_UI_Inventory_ItemDisplay>();

            id.iconImage.sprite = inv.items[i].item.icon;
            id.quantityText.text = inv.items[i].quantity.ToString();

            id.index = i;
            id.total = total;
            id.UpdatePosition(0);
            itemDisplays.Add(id);
        }

        itemDisplays[selected].FadeIn(true);
    }

    public void Close()
    {
        Tapestry_WorldClock.isPaused = false;
        GetComponent<Image>().color = new Color(1, 1, 1, 0);
        itemNameText.color = new Color(1, 1, 1, 0);

        for (int i=itemDisplays.Count-1; i>=0; i--)
        {
            Destroy(itemDisplays[i].gameObject);
            itemDisplays.RemoveAt(i);
        }

        blockDrops = false;
    }
}
