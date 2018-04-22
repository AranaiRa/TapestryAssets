using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EquipmentProfile : ScriptableObject {

    private Tapestry_ItemData
        leftHand, rightHand,
        head, body, hands, legs, feet,
        eyes, ears, neck, shoulders,
        wrists, fingers, waist, back;

    public Tapestry_EquipmentProfile()
    {

    }

    public Tapestry_ItemData GetInSlot(Tapestry_EquipSlot slot)
    {
        switch(slot)
        {
            case Tapestry_EquipSlot.LeftHand:
                return leftHand;
            case Tapestry_EquipSlot.RightHand:
                return rightHand;
            case Tapestry_EquipSlot.Head:
                return head;
            case Tapestry_EquipSlot.Body:
                return body;
            case Tapestry_EquipSlot.Hands:
                return hands;
            case Tapestry_EquipSlot.Legs:
                return legs;
            case Tapestry_EquipSlot.Feet:
                return feet;
            case Tapestry_EquipSlot.Eyes:
                return eyes;
            case Tapestry_EquipSlot.Neck:
                return neck;
            case Tapestry_EquipSlot.Shoulders:
                return shoulders;
            case Tapestry_EquipSlot.Ears:
                return ears;
            case Tapestry_EquipSlot.Wrist:
                return wrists;
            case Tapestry_EquipSlot.Fingers:
                return fingers;
            case Tapestry_EquipSlot.Waist:
                return waist;
            case Tapestry_EquipSlot.Back:
                return back;
        }
        return null;
    }

    public void Equip(Tapestry_EquipSlot slot, Tapestry_ItemData item)
    {
        switch (slot)
        {
            case Tapestry_EquipSlot.LeftHand:
                leftHand = item;
                break;
            case Tapestry_EquipSlot.RightHand:
                rightHand = item;
                break;
            case Tapestry_EquipSlot.Head:
                head = item;
                break;
            case Tapestry_EquipSlot.Body:
                body = item;
                break;
            case Tapestry_EquipSlot.Hands:
                hands = item;
                break;
            case Tapestry_EquipSlot.Legs:
                legs = item;
                break;
            case Tapestry_EquipSlot.Feet:
                feet = item;
                break;
            case Tapestry_EquipSlot.Eyes:
                eyes = item;
                break;
            case Tapestry_EquipSlot.Neck:
                neck = item;
                break;
            case Tapestry_EquipSlot.Shoulders:
                shoulders = item;
                break;
            case Tapestry_EquipSlot.Ears:
                ears = item;
                break;
            case Tapestry_EquipSlot.Wrist:
                wrists = item;
                break;
            case Tapestry_EquipSlot.Fingers:
                fingers = item;
                break;
            case Tapestry_EquipSlot.Waist:
                waist = item;
                break;
            case Tapestry_EquipSlot.Back:
                back = item;
                break;
        }
    }

    public int GetNumberOfEquippedItems()
    {
        int num = 0;
        if (leftHand != null)  num++;
        if (rightHand != null) num++;
        if (head != null)      num++;
        if (body != null)      num++;
        if (hands != null)     num++;
        if (legs != null)      num++;
        if (feet != null)      num++;
        if (eyes != null)      num++;
        if (ears != null)      num++;
        if (neck != null)      num++;
        if (shoulders != null) num++;
        if (ears != null)      num++;
        if (wrists != null)    num++;
        if (fingers != null)   num++;
        if (waist != null)     num++;
        if (back != null)      num++;
        return num;
    }

    public Dictionary<Tapestry_EquipSlot, Tapestry_ItemStack> ToDict()
    {
        Dictionary<Tapestry_EquipSlot, Tapestry_ItemStack> isd = new Dictionary<Tapestry_EquipSlot, Tapestry_ItemStack>();

        if (leftHand != null)  isd.Add(Tapestry_EquipSlot.LeftHand, new Tapestry_ItemStack(leftHand, 1));
        if (rightHand != null) isd.Add(Tapestry_EquipSlot.RightHand, new Tapestry_ItemStack(rightHand, 1));
        if (head != null)      isd.Add(Tapestry_EquipSlot.Head, new Tapestry_ItemStack(head, 1));
        if (body != null)      isd.Add(Tapestry_EquipSlot.Body, new Tapestry_ItemStack(body, 1));
        if (hands != null)     isd.Add(Tapestry_EquipSlot.Hands, new Tapestry_ItemStack(hands, 1));
        if (legs != null)      isd.Add(Tapestry_EquipSlot.Legs, new Tapestry_ItemStack(legs, 1));
        if (feet != null)      isd.Add(Tapestry_EquipSlot.Feet, new Tapestry_ItemStack(feet, 1));
        if (eyes != null)      isd.Add(Tapestry_EquipSlot.Eyes, new Tapestry_ItemStack(eyes, 1));
        if (ears != null)      isd.Add(Tapestry_EquipSlot.Ears, new Tapestry_ItemStack(ears, 1));
        if (neck != null)      isd.Add(Tapestry_EquipSlot.Neck, new Tapestry_ItemStack(neck, 1));
        if (shoulders != null) isd.Add(Tapestry_EquipSlot.Shoulders, new Tapestry_ItemStack(shoulders, 1));
        if (wrists != null)    isd.Add(Tapestry_EquipSlot.Wrist, new Tapestry_ItemStack(wrists, 1));
        if (fingers != null)   isd.Add(Tapestry_EquipSlot.Fingers, new Tapestry_ItemStack(fingers, 1));
        if (waist != null)     isd.Add(Tapestry_EquipSlot.Waist, new Tapestry_ItemStack(waist, 1));
        if (back != null)      isd.Add(Tapestry_EquipSlot.Back, new Tapestry_ItemStack(back, 1));

        return isd;
    }
}

public enum Tapestry_EquipSlot
{
    LeftHand, RightHand, BothHands, EitherHand,
    Head, Body, Hands, Legs, Feet,
    Eyes, Ears, Neck, Shoulders,
    Wrist, Fingers, Waist, Back,
    Unslotted
}