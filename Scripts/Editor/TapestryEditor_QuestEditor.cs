using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TapestryEditor_QuestEditor : EditorWindow
{
    //[MenuItem("Tapestry/Quests")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_QuestEditor), true, "Tapestry Quests", true);
    }

    private void OnGUI()
    {

    }
}
