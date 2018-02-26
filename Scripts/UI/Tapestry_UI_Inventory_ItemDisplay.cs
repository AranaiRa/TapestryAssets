using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_Inventory_ItemDisplay : MonoBehaviour {

    public Image iconImage;
    public Text quantityText;
    public int
        index, total;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void UpdateAppearance(float rad)
    {
        float increment = (float)index / (float)total * 360f;
        float theta = Mathf.Deg2Rad * (increment - 90) + rad;
        if (theta < 0) theta += Mathf.PI * 2;
        if (theta > Mathf.PI * 2) theta -= Mathf.PI * 2;

        float x = Mathf.Cos(theta);
        float y = Mathf.Sin(theta) * 0.5f;

        gameObject.transform.localPosition = new Vector3(x, y, 0) * Tapestry_UI_Inventory.itemRadius;
    }

    public void Init(Texture icon, int quantity, int index, int total)
    {
        //quantityText.text = quantity.ToString();
        float x = Mathf.Cos(Mathf.Deg2Rad * (360 * ((float)index / (float)total) + 90)) * Tapestry_UI_Inventory.itemRadius;
        float y = Mathf.Sin(Mathf.Deg2Rad * (360 * ((float)index / (float)total) + 90)) * 0.5f;

        float a1 = Mathf.Lerp(1.0f, 0.2f, y+0.5f);
        float s = Mathf.Lerp(1.0f, 0.4f, y + 0.5f);

        iconImage.color = new Color(1, 1, 1, a1);
        this.transform.localScale = new Vector3(s,s,s) * 1.5f;

        y *= Tapestry_UI_Inventory.itemRadius;
        gameObject.transform.localPosition = new Vector3(x, y, 0);
    }
}
