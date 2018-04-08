using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {
    
    public Tapestry_Effect e;

    private void Reset()
    {
        e = new Tapestry_Effect();
        //e.name = "buttfarts";
        string json = JsonUtility.ToJson(e);
        Debug.Log(json);
    }
}
