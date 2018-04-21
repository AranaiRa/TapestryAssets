using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_ActorValue : ScriptableObject {

    [SerializeField]
    private float
        baseValue,
        finalValue;
    [SerializeField]
    private Dictionary<string, float>
        bonusTypeBase,
        bonusTypeAdditive,
        bonusTypeMultiplicative;

    public float Value
    {
        get
        {
            return finalValue;
        }
    }

    public Tapestry_ActorValue()
    {
        baseValue = 1.0f;
        bonusTypeBase = new Dictionary<string, float>();
        bonusTypeAdditive = new Dictionary<string, float>();
        bonusTypeMultiplicative = new Dictionary<string, float>();
        Recalculate();
    }

    public void AdjustBaseValue(float newBase)
    {
        baseValue = newBase;
        Recalculate();
    }

    public void AddBonus(float bonus, string ID, Tapestry_BonusType type, bool overwriteConflictingID = false, bool onlyOverwriteLowerMagnitude = false)
    {
        if(type == Tapestry_BonusType.AddsToBase)
        {
            if(overwriteConflictingID)
            {
                if (onlyOverwriteLowerMagnitude)
                {
                    if (bonusTypeBase.ContainsKey(ID))
                    {
                        if (Mathf.Abs(bonus) > Mathf.Abs(bonusTypeBase[ID]))
                            bonusTypeBase[ID] = bonus;
                    }
                }
                else
                    bonusTypeBase[ID] = bonus;
            }
            else
            {
                if (!bonusTypeBase.ContainsKey(ID))
                    bonusTypeBase.Add(ID, bonus);
            }
        }
        else if(type == Tapestry_BonusType.AdditiveBonus)
        {
            if (overwriteConflictingID)
            {
                if (onlyOverwriteLowerMagnitude)
                {
                    if (bonusTypeAdditive.ContainsKey(ID))
                    {
                        if (Mathf.Abs(bonus) > Mathf.Abs(bonusTypeAdditive[ID]))
                            bonusTypeAdditive[ID] = bonus;
                    }
                }
                else
                    bonusTypeAdditive[ID] = bonus;
            }
            else
            {
                if (!bonusTypeAdditive.ContainsKey(ID))
                    bonusTypeAdditive.Add(ID, bonus);
            }
        }
        else if(type == Tapestry_BonusType.MultiplicativeBonus)
        {
            if (overwriteConflictingID)
            {
                if (onlyOverwriteLowerMagnitude)
                {
                    if (bonusTypeMultiplicative.ContainsKey(ID))
                    {
                        if (Mathf.Abs(bonus) > Mathf.Abs(bonusTypeMultiplicative[ID]))
                            bonusTypeMultiplicative[ID] = bonus;
                    }
                }
                else
                    bonusTypeMultiplicative[ID] = bonus;
            }
            else
            {
                if (!bonusTypeMultiplicative.ContainsKey(ID))
                    bonusTypeMultiplicative.Add(ID, bonus);
            }
        }
        Recalculate();
    }

    public void RemoveBonus(string id, Tapestry_BonusType type)
    {
        if (type == Tapestry_BonusType.AddsToBase)
        {
            if(bonusTypeBase.ContainsKey(id))
                bonusTypeBase.Remove(id);
        }
        else if (type == Tapestry_BonusType.AdditiveBonus)
        {
            if (bonusTypeAdditive.ContainsKey(id))
                bonusTypeAdditive.Remove(id);
        }
        else if (type == Tapestry_BonusType.MultiplicativeBonus)
        {
            if (bonusTypeMultiplicative.ContainsKey(id))
                bonusTypeMultiplicative.Remove(id);
        }
        Recalculate();
    }

    private void Recalculate()
    {
        float modifiedBase = baseValue;
        foreach (float atb in bonusTypeBase.Values)
        {
            modifiedBase += atb;
        }

        float addBonus = 1.0f;
        foreach(float ab in bonusTypeAdditive.Values)
        {
            addBonus += ab;
        }
        modifiedBase *= addBonus;

        foreach (float mb in bonusTypeMultiplicative.Values)
        {
            modifiedBase *= (1.0f + mb);
        }

        finalValue = modifiedBase;
    }
}

public enum Tapestry_BonusType
{
    AddsToBase,         //Increases base value
    AdditiveBonus,      //Adds to a total bonus of 1 + Σ(x), which the base value is then multiplied by
    MultiplicativeBonus //Multiplies the base value by 1+x for each registered bonus
}