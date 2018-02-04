using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_AttributeProfile {

    Dictionary<Tapestry_Attribute, Tapestry_AttributeIndex> dict = new Dictionary<Tapestry_Attribute, Tapestry_AttributeIndex>();
    public float 
        ActionSpeed,
        CriticalHitRate,
        MovementSpeed,
        PhysicalStaminaMult,
        MentalStaminaMult;

    public Tapestry_AttributeProfile()
    {
        foreach (Tapestry_Attribute val in Enum.GetValues(typeof(Tapestry_Attribute)))
        {
            dict.Add(val, new Tapestry_AttributeIndex(40));
        }
        RecalculateSubstats();
    }

	public int GetScore(Tapestry_Attribute attribute)
    {
        return dict[attribute].Score;
    }

    public float GetProgress(Tapestry_Attribute attribute)
    {
        return dict[attribute].Progress;
    }

    public void SetScore(Tapestry_Attribute attribute, int score)
    {
        dict[attribute].Score = score;
        RecalculateSubstats();
    }

    public void SetProgress(Tapestry_Attribute attribute, float progress)
    {
        dict[attribute].Progress = progress;
        RecalculateSubstats();
    }

    public void AddProgress(Tapestry_Attribute attribute, float amount)
    {
        dict[attribute].Progress = dict[attribute].Progress + amount;
        RecalculateSubstats();
    }

    private void RecalculateSubstats()
    {
        ActionSpeed = 100 - (GetScore(Tapestry_Attribute.Agility) - 40) / 3.0f;
        ActionSpeed /= 100.0f;
        if (ActionSpeed < 0.2f) ActionSpeed = 0.2f;

        CriticalHitRate = (5 + GetScore(Tapestry_Attribute.Insight)) / 10.0f;
        CriticalHitRate /= 100.0f;
        if (CriticalHitRate > 0.25f) CriticalHitRate = 0.25f;

        MovementSpeed = 100 + ((GetScore(Tapestry_Attribute.Fitness) * 2 - 80) + (GetScore(Tapestry_Attribute.Agility) - 40) / 6.0f);
        MovementSpeed /= 100.0f;
        if (MovementSpeed > 1.5f) MovementSpeed = 1.5f;

        PhysicalStaminaMult = 100 - (GetScore(Tapestry_Attribute.Fitness) - 40) / 2.0f;
        PhysicalStaminaMult /= 100.0f;
        if (PhysicalStaminaMult <= 0.4f) PhysicalStaminaMult = 0.4f;

        MentalStaminaMult   = 100 - (GetScore(Tapestry_Attribute.Resolve) - 40) / 2.0f;
        MentalStaminaMult /= 100.0f;
        if (MentalStaminaMult <= 0.4f) MentalStaminaMult = 0.4f;
    }
}
