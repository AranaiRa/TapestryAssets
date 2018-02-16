using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tapestry_WorldClock {

    public static float 
        worldTime,
        dayLength = 90.0f,
        globalTimeFactor = 1.0f;
    public static int
        hoursPerDay = 24,
        minutesPerHour = 60;

	public static float EvaluateTime(float timeToAdd = 0f)
    {
        worldTime += timeToAdd;
        if (worldTime > dayLength)
            worldTime -= dayLength;
        if (worldTime < 0)
            worldTime += dayLength;
        return worldTime / dayLength;
    }

    public static Color EvaluateColor(AnimationCurve twilightCurve)
    {
        Color c = Color.black;
        float dayProg = EvaluateTime();
        if (dayProg < 0) dayProg += 1.0f;

        if (dayProg >= 0.25f && dayProg < 0.27f)
        {
            float mix = (dayProg - 0.25f) / 0.02f;
            c = Color.Lerp(Color.black, Tapestry_Config.SunTwilightColor, mix);
        }
        else if (dayProg >= 0.27f && dayProg < 0.37f)
        {
            float mix = (dayProg - 0.27f) / 0.10f;
            c = Color.Lerp(Tapestry_Config.SunTwilightColor, Tapestry_Config.SunDayColor, mix);
        }
        else if (dayProg > 0.37f && dayProg < 0.63f)
        {
            c = Tapestry_Config.SunDayColor;
        }
        else if (dayProg > 0.63f && dayProg <= 0.73f)
        {
            float mix = (dayProg - 0.63f) / 0.10f;
            c = Color.Lerp(Tapestry_Config.SunDayColor, Tapestry_Config.SunTwilightColor, mix);
        }
        else if (dayProg > 0.73f && dayProg <= 0.75f)
        {
            float mix = (dayProg - 0.73f) / 0.02f;
            c = Color.Lerp(Tapestry_Config.SunTwilightColor, Color.black, mix);
        }

        return c;
    }

    public static float EvaluateSunSize(AnimationCurve twilightCurve)
    {
        return Mathf.Lerp(Tapestry_Config.SunSizeMidday, Tapestry_Config.SunSizeTwilight, twilightCurve.Evaluate(EvaluateTime()));
    }

    public static Vector2Int GetFormattedTime()
    {
        int h = Mathf.FloorToInt((worldTime / dayLength) * hoursPerDay);
        float seg = 1.0f / hoursPerDay;
        float minChange = ((worldTime / dayLength) % seg) / seg;
        int m = Mathf.FloorToInt(minChange * minutesPerHour);
        return new Vector2Int(h, m);
    }
}

public enum DaysOfTheWeek
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
}