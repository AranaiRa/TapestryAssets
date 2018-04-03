using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Tapestry_EffectBuilder_Payload_Damage : ITapestry_EffectBuilder_Payload
{
    public Tapestry_DamageType type;
    public float 
        amountMin,
        amountMax;
    [SerializeField]
    private bool
        isStackable = true;
    [SerializeField]
    private Tapestry_Effect
        parent;
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
    public bool IsStackable
    {
        get
        {
            return isStackable;
        }

        set
        {
            isStackable = value;
        }
    }
    public bool AffectsEntitiesOnly
    {
        get { return false; }
    }
    public bool AffectsPropsOnly
    {
        get { return false; }
    }

    public Tapestry_EffectBuilder_Payload_Damage(Tapestry_Effect parent, Tapestry_DamageType type, float amountMin, float amountMax)
    {
        this.parent = parent;
        this.type = type;
        this.amountMin = amountMin;
        this.amountMax = amountMax;
    }

    public void Apply()
    {
        float amount = Random.Range(amountMin, amountMax);
        parent.target.DealDamage(type, amount);
        Debug.Log("Dealing " + amount + " raw " + type.ToString() + " damage to " + parent.target.name);
    }

    public void DrawInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUIStyle title = GUIStyle.none;
        title.fontSize = 18;
        title.fontStyle = FontStyle.Bold;
        title.padding = new RectOffset(2, 2, 2, 4);
        GUILayout.Label("DAMAGE", title);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Min", GUILayout.Width(30));
        amountMin = EditorGUILayout.DelayedFloatField(amountMin, GUILayout.Width(42));
        if (amountMin < 0)
            amountMin = 0;
        GUILayout.FlexibleSpace();
        GUILayout.Label("Max", GUILayout.Width(30));
        amountMax = EditorGUILayout.DelayedFloatField(amountMax, GUILayout.Width(42));
        if (amountMax < amountMin)
            amountMax = amountMin;
        GUILayout.FlexibleSpace();
        GUILayout.Label("Type");
        type = (Tapestry_DamageType)EditorGUILayout.EnumPopup(type, GUILayout.Width(128));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
