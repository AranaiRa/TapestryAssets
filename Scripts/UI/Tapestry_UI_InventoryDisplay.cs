using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_InventoryDisplay : MonoBehaviour {

    public Text title;
    public RectTransform content;
    public List<Tapestry_UI_InventoryDisplayTextElement> elements;

	// Use this for initialization
	void Start () {
        title.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Tapestry_UI_InventoryDisplayTextElement displayPrefab, Tapestry_Inventory inv, string inventoryName)
    {
        title.text = inventoryName;
        
        elements = new List<Tapestry_UI_InventoryDisplayTextElement>();
        int y = -24;
        int height = 24;
        if (inv.items.Count == 0)
        {
            Tapestry_UI_InventoryDisplayTextElement e =
                       (Tapestry_UI_InventoryDisplayTextElement)Instantiate(displayPrefab, content);
            e.title.text = "<i>No items</i>";
            e.title.color = new Color(1, 1, 1, 0.5f);
            e.quantity.text = "";
            e.size.text = "";
            elements.Add(e);
        }
        else
        {
            foreach (Tapestry_ItemStack stack in inv.items)
            {
                Tapestry_UI_InventoryDisplayTextElement e =
                    (Tapestry_UI_InventoryDisplayTextElement)Instantiate(displayPrefab, content);
                e.GetComponent<RectTransform>().localPosition = new Vector3(e.GetComponent<RectTransform>().localPosition.x, y, 0);
                e.SetData(stack);
                y -= 24;
                height += 24;
                elements.Add(e);
            }
        }
        height += 24;
    }

    public void Clear()
    {
        title.text = "";
        for (int i = elements.Count - 1; i >= 0; i--)
        {
            Destroy(elements[i].gameObject);
        }
        elements.Clear();
    }
}
