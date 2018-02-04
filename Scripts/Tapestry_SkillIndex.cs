using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tapestry_SkillIndex
{

    private int score;
    private float progress;

    public float Progress
    {
        get
        {
            return progress;
        }

        set
        {
            progress = value;
            if (progress < 0) progress = 0;
            else if (progress >= 1000)
            {
                progress -= 1000;
                Score += 1;
            }
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public Tapestry_SkillIndex(int startingScore)
    {
        Score = startingScore;
        progress = 0;
    }
}

public enum Tapestry_Skill
{
    Blitz, Rush, Strike, Parry,
    Conditioning, Disguise, Larceny, Mobility, Sneak, 
    Conjure, Divine, Enchant, Transmute
}