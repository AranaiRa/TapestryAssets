using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Tapestry_KeywordRegistry : ScriptableObject {

    public List<string> keywords = new List<string>();

    [SerializeField]
    private string keywordToAdd;

    public int Count
    {
        get
        {
            return keywords.Count;
        }
    }

    public bool Contains(string keywordToCheck)
    {
        if (keywords.Contains(keywordToCheck))
            return true;
        else
            return false;
    }

    public bool ContainsOne(string[] list)
    {
        bool check = false;
        foreach(string sThat in list)
        {
            foreach(string sThis in keywords)
            {
                if (sThis == sThat)
                {
                    check = true;
                    break;
                }
            }
            if (check)
                break;
        }
        return check;
    }

    public bool ContainsOne(List<string> list)
    {
        return ContainsOne(list.ToArray());
    }

    public bool ContainsOne(Tapestry_KeywordRegistry reg)
    {
        return ContainsOne(reg.keywords);
    }

    public bool ContainsAll(string[] list)
    {
        bool check = true;
        foreach (string sThat in list)
        {
            bool hasThis = false;
            foreach (string sThis in keywords)
            {
                if (sThis == sThat)
                {
                    hasThis = true;
                    break;
                }
            }
            check = check & hasThis;
            if (!check)
                break;
        }
        return check;
    }

    public bool ContainsAll(List<string> list)
    {
        return ContainsAll(list.ToArray());
    }

    public bool ContainsAll(Tapestry_KeywordRegistry reg)
    {
        return ContainsAll(reg.keywords);
    }

    public void Add(string str)
    {
        if (!keywords.Contains(str))
        {
            keywords.Add(str);
        }
    }

    public void Add(Tapestry_KeywordRegistry kr)
    {
        foreach(string str in kr.keywords)
        {
            Add(str);
        }
    }

    #if UNITY_EDITOR
    public void DrawInspector()
    {
        GUIStyle title = new GUIStyle();
        title.fontStyle = FontStyle.Bold;
        title.fontSize = 14;

        int indexToRemove = -1;
        GUILayout.BeginVertical("box");
        GUILayout.Label("Keywords", title);
        GUILayout.BeginVertical("box");
        if (keywords.Count == 0)
        {
            GUILayout.Label("No keywords associated with this component.");
        }
        else
        {
            for (int i = 0; i < keywords.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    indexToRemove = i;
                }
                keywords[i] = EditorGUILayout.DelayedTextField(keywords[i]);
                GUILayout.EndHorizontal();
            }
        }
        if (indexToRemove != -1)
        {
            if (keywords.Count == 1)
                keywords.Clear();
            else
                keywords.RemoveAt(indexToRemove);
        }

        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            if (keywordToAdd != "")
            {
                keywords.Add(keywordToAdd);
                keywordToAdd = null;
            }
        }
        keywordToAdd = EditorGUILayout.TextField(keywordToAdd);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    #endif
}
