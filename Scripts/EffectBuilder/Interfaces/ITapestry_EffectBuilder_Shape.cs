using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITapestry_EffectBuilder_Shape {

    string NameRegistration { get; }
    Tapestry_Effect Parent { get; set; }

    List<Tapestry_Actor> GetAffectedTargets();
}
