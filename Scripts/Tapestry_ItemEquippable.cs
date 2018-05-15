using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

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
