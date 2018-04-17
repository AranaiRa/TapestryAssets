using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EquipmentProfile : ScriptableObject {

    private Tapestry_ItemEquippable 
        leftHand, rightHand,
        head, body, hands, legs, feet,
        eyes, ears, neck, shoulders,
        wrists, fingers, waist, back;

    public Tapestry_EquipmentProfile()
    {

    }

    public Tapestry_ItemEquippable GetInSlot(Tapestry_EquipSlot slot)
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
            case Tapestry_EquipSlot.Wrist:
                return wrists;
            case Tapestry_EquipSlot.Fingers:
                return fingers;
            case Tapestry_EquipSlot.Waist:
                return waist;
            case Tapestry_EquipSlot.Back:
                return back;
        }
        return new Tapestry_ItemEquippable();
    }

    public void Equip(Tapestry_EquipSlot slot, Tapestry_ItemEquippable item)
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
}

public enum Tapestry_EquipSlot
{
    LeftHand, RightHand, BothHands, EitherHand,
    Head, Body, Hands, Legs, Feet,
    Eyes, Ears, Neck, Shoulders,
    Wrist, Fingers, Waist, Back
}