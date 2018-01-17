using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Inventory : MonoBehaviour {

    public List<Tapestry_Item> items;
    public List<Tapestry_ItemKey> keys;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool ContainsKeyID(string id)
    {
        foreach(Tapestry_ItemKey k in keys)
        {
            if (k.keyID == id)
                return true;
        }
        return false;
    }
}
