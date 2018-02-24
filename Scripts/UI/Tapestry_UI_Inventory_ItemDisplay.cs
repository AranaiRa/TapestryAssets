using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_Inventory_ItemDisplay : MonoBehaviour {

    public Image itemIcon;
    public Text quantityText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Sprite icon, int quantity, int index, int total)
    {
        itemIcon.sprite = icon;
        quantityText.text = quantity.ToString();
        float x = Mathf.Cos(Mathf.Deg2Rad * (360 * ((float)index / (float)total) + 90)) * Tapestry_UI_Inventory.itemRadius;
        float y = Mathf.Sin(Mathf.Deg2Rad * (360 * ((float)index / (float)total) + 90)) * Tapestry_UI_Inventory.itemRadius;
        gameObject.transform.localPosition = new Vector3(x, y, 0);
    }
}
