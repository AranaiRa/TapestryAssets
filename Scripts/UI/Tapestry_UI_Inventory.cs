using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_UI_Inventory : MonoBehaviour {

    public GameObject itemDisplayPrefab;
    public static float itemRadius = 220.0f;

	// Use this for initialization
	void Start () {
        int total = 32;

		for(int i=0;i<total;i++)
        {
            GameObject go = GameObject.Instantiate(itemDisplayPrefab);
            go.transform.SetParent(this.transform);
            go.GetComponent<Tapestry_UI_Inventory_ItemDisplay>().Init(null, i * 5, i, total);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
