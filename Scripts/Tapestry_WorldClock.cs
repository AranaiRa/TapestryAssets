using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tapestry_WorldClock {

    public static float 
        worldTime,
        dayLength = 90.0f;
    public static int
        hoursPerDay = 24,
        minutesPerHour = 60;
    public static Vector2Int
        clockTime = new Vector2Int();
    public static string
        clockTimeString = "";
    public static bool
        isPaused = false;
    private static float globalTimeFactor = 1.0f;
    
    public static float GlobalTimeFactor
    {
        get
        {
            if (isPaused)
                return 0;
            else
                return globalTimeFactor;
        }

        set
        {
            globalTimeFactor = value;
        }
    }

    public static float EvaluateTime(float timeToAdd = 0f)
    {
        if(!isPaused)
        { 
        worldTime += timeToAdd;
        if (worldTime > dayLength)
            worldTime -= dayLength;
        if (worldTime < 0)
            worldTime += dayLength;

        int h = Mathf.FloorToInt((worldTime / dayLength) * hoursPerDay);
        float seg = 1.0f / hoursPerDay;
        float minChange = ((worldTime / dayLength) % seg) / seg;
        int m = Mathf.FloorToInt(minChange * minutesPerHour);
        clockTime = new Vector2Int(h, m);
        if(m <= 9)
            clockTimeString = h + ":0" + m;
        else
            clockTimeString = h + ":" + m;
        }
        return worldTime / dayLength;
    }

    public static Color EvaluateColor()
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

    public static Color EvaluateAmbientColor()
    {
        Color c = Tapestry_Config.AmbientNightColor;
        float dayProg = EvaluateTime();
        if (dayProg < 0) dayProg += 1.0f;

        if (dayProg >= 0.23f && dayProg < 0.25f)
        {
            float mix = (dayProg - 0.23f) / 0.02f;
            c = Color.Lerp(Tapestry_Config.AmbientNightColor, Tapestry_Config.AmbientTwilightColor, mix);
        }
        else if (dayProg >= 0.25f && dayProg < 0.27f)
        {
            float mix = (dayProg - 0.25f) / 0.02f;
            c = Color.Lerp(Tapestry_Config.AmbientDayColor, Tapestry_Config.AmbientTwilightColor, mix);
        }
        else if (dayProg >= 0.27f && dayProg < 0.37f)
        {
            float mix = (dayProg - 0.27f) / 0.10f;
            c = Color.Lerp(Tapestry_Config.AmbientTwilightColor, Tapestry_Config.AmbientDayColor, mix);
        }
        else if (dayProg > 0.37f && dayProg < 0.63f)
        {
            c = Tapestry_Config.AmbientDayColor;
        }
        else if (dayProg > 0.63f && dayProg <= 0.73f)
        {
            float mix = (dayProg - 0.63f) / 0.10f;
            c = Color.Lerp(Tapestry_Config.AmbientDayColor, Tapestry_Config.AmbientTwilightColor, mix);
        }
        else if (dayProg > 0.73f && dayProg <= 0.75f)
        {
            float mix = (dayProg - 0.73f) / 0.02f;
            c = Color.Lerp(Tapestry_Config.AmbientTwilightColor, Tapestry_Config.AmbientDayColor, mix);
        }
        else if (dayProg > 0.75f && dayProg <= 0.77f)
        {
            float mix = (dayProg - 0.75f) / 0.02f;
            c = Color.Lerp(Tapestry_Config.AmbientTwilightColor, Tapestry_Config.AmbientNightColor, mix);
        }

        return c;
    }

    public static float EvaluateSunSize()
    {
        //TODO: Sun size controls
        return 1.0f;
    }
}