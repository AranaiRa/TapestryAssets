using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_SwitchData {
    public bool
        pp_swapOpenState,
        pp_swapLockedState,
        pp_swapBypassableState,
        pp_swapInteractivityState,
        pp_setSourceHarvestable,
        pp_toggleSourceHarvestable,

        on_setLocked,
        on_setUnlocked,
        on_setBypassable = true,
        on_setOpen,
        on_setClosed,
        on_setInteractable = true,
        on_setSourceHarvestable = true,

        off_setLocked,
        off_setUnlocked,
        off_setBypassable = true,
        off_setOpen,
        off_setClosed,
        off_setInteractable = true,
        off_setSourceHarvestable;

    public Tapestry_SwitchData()
    {

    }
}
