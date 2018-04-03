using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITapestry_EffectBuilder_Payload {

    Tapestry_Effect Parent { get; set; }
    bool IsStackable { get; set; }
    bool AffectsEntitiesOnly { get; }
    bool AffectsPropsOnly { get; }

    void Apply();
    void DrawInspector();
}
