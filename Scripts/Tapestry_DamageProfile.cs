using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_DamageProfile : MonoBehaviour {

    Tapestry_DamageTypeIndex
        typeBur = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Burning,     0, 0),
        typeCho = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Choking,     0, 0),
        typeCor = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Corrosive,   0, 0),
        typeCru = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Crushing,    0, 0),
        typeEle = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Electric,    0, 0),
        typeExp = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Explosive,   0, 0),
        typeFre = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Freezing,    0, 0),
        typeIrr = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Irradiating, 0, 0),
        typePie = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Piercing,    0, 0),
        typeSla = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Slashing,    0, 0),
        typeTox = new Tapestry_DamageTypeIndex(Tapestry_DamageType.Toxic,       0, 0);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void SetProfile(Tapestry_DamageType type, float res, float mit)
    {
        switch (type)
        {
            case Tapestry_DamageType.Burning:
                typeBur.resistance = res;
                typeBur.mitigation = mit;
                break;
            case Tapestry_DamageType.Choking:
                typeCho.resistance = res;
                typeCho.mitigation = mit;
                break;
            case Tapestry_DamageType.Corrosive:
                typeCor.resistance = res;
                typeCor.mitigation = mit;
                break;
            case Tapestry_DamageType.Crushing:
                typeCru.resistance = res;
                typeCru.mitigation = mit;
                break;
            case Tapestry_DamageType.Electric:
                typeEle.resistance = res;
                typeEle.mitigation = mit;
                break;
            case Tapestry_DamageType.Explosive:
                typeExp.resistance = res;
                typeExp.mitigation = mit;
                break;
            case Tapestry_DamageType.Freezing:
                typeFre.resistance = res;
                typeFre.mitigation = mit;
                break;
            case Tapestry_DamageType.Irradiating:
                typeIrr.resistance = res;
                typeIrr.mitigation = mit;
                break;
            case Tapestry_DamageType.Piercing:
                typePie.resistance = res;
                typePie.mitigation = mit;
                break;
            case Tapestry_DamageType.Slashing:
                typeSla.resistance = res;
                typeSla.mitigation = mit;
                break;
            case Tapestry_DamageType.Toxic:
                typeTox.resistance = res;
                typeTox.mitigation = mit;
                break;
        }
    }

    public Tapestry_DamageTypeIndex GetProfile(Tapestry_DamageType type)
    {
        switch(type)
        {
            case Tapestry_DamageType.Burning:
                return typeBur;
            case Tapestry_DamageType.Choking:
                return typeCho;
            case Tapestry_DamageType.Corrosive:
                return typeCor;
            case Tapestry_DamageType.Crushing:
                return typeCru;
            case Tapestry_DamageType.Electric:
                return typeEle;
            case Tapestry_DamageType.Explosive:
                return typeExp;
            case Tapestry_DamageType.Freezing:
                return typeFre;
            case Tapestry_DamageType.Irradiating:
                return typeIrr;
            case Tapestry_DamageType.Piercing:
                return typePie;
            case Tapestry_DamageType.Slashing:
                return typeSla;
            case Tapestry_DamageType.Toxic:
                return typeTox;
            default:
                return new Tapestry_DamageTypeIndex(Tapestry_DamageType.Untyped, 0, 0);
        }
    }
}
