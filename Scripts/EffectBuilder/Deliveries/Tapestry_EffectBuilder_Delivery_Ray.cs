using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tapestry_EffectBuilder_Delivery_Ray : Tapestry_EffectBuilder_Delivery
{
    public float maxDistance;
    
    public Tapestry_EffectBuilder_Delivery_Ray()
    {

    }

    public override List<Tapestry_Actor> GetAffectedTargets()
    {
        //List<Tapestry_Actor> targets = new List<Tapestry_Actor>();
        //RaycastHit hit;
        //Physics.Raycast(parent.initiator.transform.position, parent.target.transform.forward, out hit, maxDistance, ~LayerMask.GetMask("Ignore Raycast"));
        //if(hit.collider != null)
        //{
        //    Debug.Log("pre: hit \"" + hit.collider.gameObject.name + "\"");
        //    Tapestry_Actor a = hit.collider.gameObject.GetComponentInParent<Tapestry_Actor>();
        //    if (a != null)
        //    {
        //        targets.Add(a);
        //    }
        //}
        //return targets;
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        string export = "[<DELIVERY:RAY> ";

        export += maxDistance + "m]";

        return export;
    }

    public override void DrawInspector()
    {
        GUILayout.BeginVertical("box");
        
        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Label("Max Distance");
        maxDistance = EditorGUILayout.DelayedFloatField(maxDistance, GUILayout.Width(42));
        if (maxDistance < 0)
            maxDistance = 0;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
