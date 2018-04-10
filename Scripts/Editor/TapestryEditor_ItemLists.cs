using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TapestryEditor_ItemLists : EditorWindow
{
    //[MenuItem("Tapestry/Item Lists")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_ItemLists), true, "Tapestry Item Lists", true);
    }

    private void OnGUI()
    {

    }
}
