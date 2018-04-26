using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Tapestry_ItemWeaponMelee))]
public class TapestryInspector_WeaponMelee : Editor {

    int pSel = 0;
    bool startup = true;

    public override void OnInspectorGUI()
    {
        Tapestry_ItemWeaponMelee w = target as Tapestry_ItemWeaponMelee;

        if (ReferenceEquals(w.effectStanding, null))
            w.effectStanding = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");
        if (ReferenceEquals(w.effectStanding.payload, null))
            w.effectStanding.payload = (Tapestry_EffectBuilder_Payload)ScriptableObject.CreateInstance("Tapestry_EffectBuilder_Payload_Damage");

        if (startup)
        {
            pSel = ArrayUtility.IndexOf(Tapestry_Config.GetPayloadTypes().Values.ToArray(), w.effectStanding.payload.GetType());
            startup = false;
        }

        GUILayout.BeginVertical("box");
        
        pSel = w.effectStanding.DrawInspector(pSel);

        GUILayout.EndVertical();

        base.OnInspectorGUI();
    }
}
