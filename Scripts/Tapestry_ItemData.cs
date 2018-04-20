using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ItemData {
    
    public List<string> keywords;
    public Tapestry_Entity owningEntity;
    public Tapestry_Faction owningFaction;
    public Tapestry_ItemSize size;
    public Tapestry_EquipSlot slot = Tapestry_EquipSlot.Unslotted;
    public Sprite icon;
    public int value;
    public string 
        prefabName,
        displayName,
        keyID;
    public bool
        isKey,
        isHoldable,
        useEffect;
    public Tapestry_Effect effect;

    public Tapestry_ItemData()
    {

    }

    public bool IsEqual(Tapestry_ItemData data)
    {
        bool check = true;
        if (prefabName != data.prefabName) check = check && false;
        if (value != data.value) check = check && false;
        if (icon != data.icon) check = check && false;
        if (size != data.size) check = check && false;
        if (owningEntity != data.owningEntity) check = check && false;
        if (owningFaction != data.owningFaction) check = check && false;
        if (displayName != data.displayName) check = check && false;
        if (isKey != data.isKey) check = check && false;
        if (isKey)
        {
            if(keyID != data.keyID) check = check && false;
        }
        if (useEffect && data.useEffect)
        {
            if (!effect.Equals(data.effect)) check = check && false;
        }
        return check;
    }
}
