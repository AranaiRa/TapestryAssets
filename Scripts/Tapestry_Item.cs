using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_Item : MonoBehaviour {

    public Sprite icon;
    public GameObject model;
    public Tapestry_Entity owningEntity;
    public Tapestry_Faction owningFaction;
    public Tapestry_ItemSize size = Tapestry_ItemSize.Negligible;
    public int value;
    public List<string> keywords;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum Tapestry_ItemSize
{
    Negligible, Small, Medium, Large
}