using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tapestry_WorldClock {

    public static Tapestry_TimeIndex worldTime = new Tapestry_TimeIndex();
    public static float
        dayLength = 90.0f;
    public static int
        hoursPerDay = 24,
        minutesPerHour = 60;
    private static bool
        isPaused = false;

    private static float
        globalTimeFactor = 1.0f,
        sunProg,
        leftoverTime;
    
    public static float GlobalTimeFactor
    {
        get
        {
            if (IsPaused)
                return 0;
            else
                return globalTimeFactor;
        }

        set
        {
            globalTimeFactor = value;
            Time.timeScale = value;
        }
    }

    public static bool IsPaused
    {
        get
        {
            return isPaused;
        }

        set
        {
            isPaused = value;
            if (value)
                Time.timeScale = 0;
            else
                Time.timeScale = globalTimeFactor;
        }
    }

    public static float EvaluateTime(float timeToAdd = 0f)
    {
        if (!IsPaused)
        {
            leftoverTime += timeToAdd;
            if (leftoverTime >= 1)
            {
                worldTime.AddTime(Mathf.FloorToInt(leftoverTime));
                leftoverTime = leftoverTime % 1;
            }
            sunProg = worldTime.ProgressUntilNextDay(leftoverTime);
        }

        return sunProg;
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