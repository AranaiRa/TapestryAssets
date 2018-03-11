using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tapestry_Activatable : MonoBehaviour {

    public string displayName = "";
    public bool 
        isInteractable = true,
        displayNameWhenUnactivatable = false;
    public List<string> keywords;

	public virtual void Activate(Tapestry_Entity activatingEntity)
    {

    }

    public virtual void Hover()
    {

    }
}
