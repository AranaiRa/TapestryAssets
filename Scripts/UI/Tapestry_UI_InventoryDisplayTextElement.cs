﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tapestry_UI_InventoryDisplayTextElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Text
        quantity,
        size,
        title;
    public Image
        highlight,
        equipIcon;
    public Sprite
        spriteLeft,
        spriteRight,
        spriteBoth;
    private bool
        active;
    private Tapestry_ItemData data;

    public bool Active
    {
        get
        {
            return active;
        }
    }

    private void Start()
    {
        active = false;
        highlight.gameObject.SetActive(false);
        equipIcon.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (title.text != "<i>No items</i>")
        {
            active = true;
            highlight.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (title.text != "<i>No items</i>")
        {
            active = false;
            highlight.gameObject.SetActive(false);
        }
    }

    public void SetData(Tapestry_ItemStack data)
    {
        this.data = data.item;
        if (data.item.size == Tapestry_ItemSize.Large)
            size.text = "L";
        else if (data.item.size == Tapestry_ItemSize.Small)
            size.text = "S";
        else if (data.item.size == Tapestry_ItemSize.Negligible)
            size.text = "–";
        quantity.text = data.quantity.ToString();
        title.text = data.item.displayName;
    }

    public void SetEquippedState(int state)
    {
        if (state == 0)
        {
            equipIcon.gameObject.SetActive(false);
        }
        else if (state == 1)
        {
            equipIcon.sprite = spriteLeft;
            equipIcon.gameObject.SetActive(true);
        }
        else if (state == 2)
        {
            equipIcon.sprite = spriteRight;
            equipIcon.gameObject.SetActive(true);
        }
        else if (state == 3)
        {
            equipIcon.sprite = spriteBoth;
            equipIcon.gameObject.SetActive(true);
        }
    }

    public Tapestry_ItemData GetData()
    {
        return data;
    }
}
