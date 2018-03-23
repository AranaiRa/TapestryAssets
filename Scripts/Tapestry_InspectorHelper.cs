using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_InspectorHelper : MonoBehaviour {

    public string helpMessage = "Whoops!\nThis component shouldn't be added to anything manually.\nPlease press the \"OK\" button below to remove it.";

    private void Start()
    {
        DestroyImmediate(this);
    }
}
