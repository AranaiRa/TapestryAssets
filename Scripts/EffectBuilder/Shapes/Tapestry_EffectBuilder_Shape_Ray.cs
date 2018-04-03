using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tapestry_EffectBuilder_Shape_Ray : ITapestry_EffectBuilder_Shape
{
    public string NameRegistration
    {
        get { return "Ray"; }
    }

    public float maxDistance;
    private Tapestry_Effect parent;
    public Tapestry_Effect Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    public Tapestry_EffectBuilder_Shape_Ray(Tapestry_Effect parent, float maxDistance)
    {
        this.parent = parent;
        this.maxDistance = maxDistance;
    }

    public List<Tapestry_Actor> GetAffectedTargets()
    {
        List<Tapestry_Actor> targets = new List<Tapestry_Actor>();
        RaycastHit hit;
        Physics.Raycast(parent.target.transform.position, parent.target.transform.forward, out hit, maxDistance, ~LayerMask.GetMask("Ignore Raycast"));
        if(hit.collider != null)
        {
            Debug.Log("pre: hit \"" + hit.collider.gameObject.name + "\"");
            Tapestry_Actor a = hit.collider.gameObject.GetComponentInParent<Tapestry_Actor>();
            if (a != null)
            {
                targets.Add(a);
            }
        }
        return targets;
    }



    public void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUIStyle title = GUIStyle.none;
        title.fontSize = 18;
        title.fontStyle = FontStyle.Bold;
        title.padding = new RectOffset(2, 2, 2, 4);
        GUILayout.Label("RAY", title);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Max Distance");
        maxDistance = EditorGUILayout.DelayedFloatField(maxDistance, GUILayout.Width(42));
        if (maxDistance < 0)
            maxDistance = 0;
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
