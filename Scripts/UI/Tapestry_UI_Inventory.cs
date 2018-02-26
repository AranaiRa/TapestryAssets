using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class Tapestry_UI_Inventory : MonoBehaviour {

    public Tapestry_Player player;
    public GameObject itemDisplayPrefab;
    public static float itemRadius = 200.0f;
    public float
        delayTime = 0.25f;
    public bool
        isWindowOpen = false;
    public List<Tapestry_UI_Inventory_ItemDisplay> itemDisplays = new List<Tapestry_UI_Inventory_ItemDisplay>();

    private float
        timeOpenClose,
        timeRotation;
    
	void Start ()
    {
        /*int total = 11;

		for(int i=0;i<total;i++)
        {
            GameObject go = GameObject.Instantiate(itemDisplayPrefab);
            go.transform.SetParent(this.transform);
            Tapestry_UI_Inventory_ItemDisplay id = go.GetComponent<Tapestry_UI_Inventory_ItemDisplay>();
            id.index = i;
            id.total = total;
            id.UpdateAppearance(0);
            itemDisplays.Add(id);
        }*/
    }
	
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
            timeRotation += Time.deltaTime;
        else if (Input.GetKey(KeyCode.LeftArrow))
            timeRotation -= Time.deltaTime;

        if (timeRotation > Mathf.PI * 2)
            timeRotation -= Mathf.PI * 2;
        if (timeRotation < 0)
            timeRotation += Mathf.PI * 2;

        foreach(Tapestry_UI_Inventory_ItemDisplay id in itemDisplays)
        {
            id.UpdateAppearance(timeRotation);
        }

		if(Input.GetKey(Tapestry_Config.KeyboardInput_Inventory) && timeOpenClose == 0)
        {
            timeOpenClose = delayTime;
            isWindowOpen = !isWindowOpen;

            if (isWindowOpen)
            {
                Debug.Log(player.inventory == null);
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

        int total = inv.items.Count;

        for (int i = 0; i < total; i++)
        {
            GameObject go = GameObject.Instantiate(itemDisplayPrefab);
            go.transform.SetParent(this.transform);
            Tapestry_UI_Inventory_ItemDisplay id = go.GetComponent<Tapestry_UI_Inventory_ItemDisplay>();

            id.iconImage.sprite = inv.items[i].item.icon;
            id.quantityText.text = inv.items[i].quantity.ToString();

            id.index = i;
            id.total = total;
            id.UpdateAppearance(0);
            itemDisplays.Add(id);
        }
    }

    public void Close()
    {
        Tapestry_WorldClock.isPaused = false;
        GetComponent<Image>().color = new Color(1, 1, 1, 0);

        for (int i=itemDisplays.Count-1; i>=0; i--)
        {
            Destroy(itemDisplays[i].gameObject);
            itemDisplays.RemoveAt(i);
        }
    }
}
