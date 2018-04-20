using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tapestry_ItemEquippable : Tapestry_Item {

    protected override void Reset()
    {
        base.Reset();
        data.isHoldable = true;
    }

    public virtual void OnHideEquipped()
    {

    }

    public virtual void OnShowEquipped()
    {

    }
}
