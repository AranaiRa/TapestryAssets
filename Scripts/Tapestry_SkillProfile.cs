using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_SkillProfile
{

    Dictionary<Tapestry_Skill, Tapestry_SkillIndex> dict = new Dictionary<Tapestry_Skill, Tapestry_SkillIndex>();
    public float
        ActionSpeed,
        CriticalHitRate,
        MovementSpeed,
        PhysicalStaminaMult,
        MentalStaminaMult;

    public Tapestry_SkillProfile()
    {
        foreach (Tapestry_Skill val in Enum.GetValues(typeof(Tapestry_Skill)))
        {
            dict.Add(val, new Tapestry_SkillIndex(5));
        }
    }

    public int GetScore(Tapestry_Skill Skill)
    {
        return dict[Skill].Score;
    }

    public float GetProgress(Tapestry_Skill Skill)
    {
        return dict[Skill].Progress;
    }

    public void SetScore(Tapestry_Skill Skill, int score)
    {
        dict[Skill].Score = score;
    }

    public void SetProgress(Tapestry_Skill Skill, float progress)
    {
        dict[Skill].Progress = progress;
    }

    public void AddProgress(Tapestry_Skill Skill, float amount)
    {
        dict[Skill].Progress = dict[Skill].Progress + amount;
    }
}
