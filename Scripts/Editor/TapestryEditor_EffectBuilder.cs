using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TapestryEditor_EffectBuilder : EditorWindow
{
    [MenuItem("Tapestry/Effect Builder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_EffectBuilder), true, "Tapestry Effect Builder", true);
    }

    private void OnGUI()
    {

    }
}
