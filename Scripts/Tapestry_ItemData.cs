﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ItemData {

    public string prefabName;
    public List<string> keywords;
    public Tapestry_Entity owningEntity;
    public Tapestry_Faction owningFaction;
    public int value;
    public Sprite icon;
    public Tapestry_ItemSize size;
    public string displayName;

    public Tapestry_ItemData()
    {

    }

    public void Instantiate()
    {

    }

    public bool Compare(Tapestry_ItemData data)
    {
        bool check = true;
        if (prefabName != data.prefabName) check = check && false;
        if (value != data.value) check = check && false;
        if (icon != data.icon) check = check && false;
        if (size != data.size) check = check && false;
        if (owningEntity != data.owningEntity) check = check && false;
        if (owningFaction != data.owningFaction) check = check && false;
        if (displayName != data.displayName) check = check && false;
        return check;
    }
}
