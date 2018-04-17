using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tapestry_ItemEquippable : Tapestry_Item {

    public bool isTwoHanded = false;

    protected override void Reset()
    {
        base.Reset();
        data.isHoldable = true;
    }
}
