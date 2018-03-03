using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_Inventory_ItemDisplay : MonoBehaviour {

    public Image iconImage;
    public Text quantityText;
    public int
        index, total;

    private float
        changeTime;
    private bool
        isFadingIn,
        isFadingOut;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(isFadingOut)
        {
            changeTime -= Time.deltaTime;
            if (changeTime < 0)
                changeTime = 0;

            float a1 = changeTime / Tapestry_Config.InventoryItemFadeTime;
            float a2 = changeTime * 0.7f + 0.3f;
            float s = a1 * 0.25f + 0.75f;

            GetComponent<Image>().color = new Color(1, 1, 1, a1*0.2f);
            iconImage.color = new Color(1, 1, 1, a2);
            iconImage.rectTransform.localScale = new Vector3(s, s, s);
            quantityText.color = new Color(1, 1, 1, a1);

            if (changeTime == 0)
                isFadingOut = false;
        }
        else if (isFadingIn)
        {
            changeTime -= Time.deltaTime;
            if (changeTime < 0)
                changeTime = 0;

            float a = 1 - (changeTime / Tapestry_Config.InventoryItemFadeTime);
            float s = a * 0.25f + 0.75f;

            GetComponent<Image>().color = new Color(1, 1, 1, a * 0.2f);
            iconImage.color = new Color(1, 1, 1, a*0.7f + 0.3f);
            iconImage.rectTransform.localScale = new Vector3(s, s, s);
            quantityText.color = new Color(1, 1, 1, a);

            if (changeTime == 0)
                isFadingOut = false;
        }
    }

    public void FadeIn(bool instant = false)
    {
        if (instant)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            iconImage.color = new Color(1, 1, 1, 1);
            iconImage.rectTransform.localScale = Vector3.one;
            quantityText.color = Color.white;
        }
        else
        {
            isFadingIn = true;
            isFadingOut = false;
            
            changeTime = Tapestry_Config.InventoryItemFadeTime;
        }
    }

    public void FadeOut(bool instant = false)
    {
        if(instant)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
            iconImage.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            isFadingIn = false;
            isFadingOut = true;

            changeTime = Tapestry_Config.InventoryItemFadeTime;
        }
    }

    public void UpdatePosition(float rad)
    {
        float increment = (float)index / (float)total * 360f;
        float theta = Mathf.Deg2Rad * (increment - 90) + rad;
        if (theta < 0) theta += Mathf.PI * 2;
        if (theta > Mathf.PI * 2) theta -= Mathf.PI * 2;

        float x = Mathf.Cos(theta);
        float y = Mathf.Sin(theta) * 0.35f;

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
