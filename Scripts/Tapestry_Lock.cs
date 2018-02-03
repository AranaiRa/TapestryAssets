using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Lock {

    public bool isLocked, canBeBypassed;
    private int lockLevel;
    public string keyID;

    public int LockLevel
    {
        get
        {
            return lockLevel;
        }

        set
        {
            if (value < 0)
                lockLevel = 0;
            else if (value > 100)
                lockLevel = 100;
            else
                lockLevel = value;
        }
    }

    public Tapestry_Lock(bool isLocked, int lockLevel, string keyID)
    {
        this.isLocked = isLocked;
        LockLevel = lockLevel;
        this.keyID = keyID;
        canBeBypassed = true;
    }

    public Tapestry_Lock(bool isLocked, bool canBeBypassed)
    {
        this.isLocked = isLocked;
        this.canBeBypassed = canBeBypassed;
        LockLevel = 0;
        keyID = "";
    }
}
