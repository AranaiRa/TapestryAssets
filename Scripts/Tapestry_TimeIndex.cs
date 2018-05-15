using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Tapestry_TimeIndex {

    public static uint
        secondsPerMinute = 60,
        minutesPerHour   = 60,
        hoursPerDay      = 24,
        daysPerMonth     = 30,
        monthsPerYear    = 12,
        startingMinute   = 0,
        startingHour     = 6,
        startingDate     = 23,
        startingMonth    = 3,
        startingYear     = 1220;

    private int
        second,
        minute,
        hour,
        date,
        month,
        year;

    public int Second
    {
        get { return second; }
    }

    public int Minute
    {
        get { return minute; }
    }

    public int Hour
    {
        get { return hour; }
    }

    public int Date
    {
        get { return date; }
    }

    public int Month
    {
        get { return month; }
    }

    public int Year
    {
        get { return year; }
    }

    public Tapestry_TimeIndex()
    {
        ResetClock();
    }

    public Tapestry_TimeIndex(int s, int m=0, int h=0, int d=0, int mo=0, int y=0)
    {
        second = s;
        minute = m;
        hour = h;
        date = d;
        month = mo;
        year = y;
    }

    public void ResetClock()
    {
        second = 0;
        minute = 0;
        hour = (int)startingHour;
        date = (int)startingDate;
        month = (int)startingMonth;
        year = (int)startingYear;
    }

    public ulong TimeToNumber()
    {
        ulong eval = (ulong)second;
        eval += (ulong)(minute * secondsPerMinute);
        eval += (ulong)(hour * minutesPerHour * secondsPerMinute);
        eval += (ulong)(date * hoursPerDay * minutesPerHour * secondsPerMinute);
        eval += (ulong)(month * daysPerMonth * hoursPerDay * minutesPerHour * secondsPerMinute);
        eval += (ulong)(year * monthsPerYear * daysPerMonth * hoursPerDay * minutesPerHour * secondsPerMinute);

        return eval;
    }

    public bool HasPassedTime(Tapestry_TimeIndex ti)
    {
        return TimeToNumber() > ti.TimeToNumber();
    }

    public bool IsTime(Tapestry_TimeIndex ti, bool ignoreSeconds = true)
    {
        bool eq = true;
        eq = eq && (ti.Year == year);
        eq = eq && (ti.Month == month);
        eq = eq && (ti.Date == date);
        eq = eq && (ti.Hour == hour);
        eq = eq && (ti.Minute == minute);
        if (!ignoreSeconds)
            eq = eq && (ti.Second == second);
        return eq;
    }

    public bool IsTime(Vector3Int hourMinuteSecond)
    {
        bool eq = true;
        eq = eq && (hourMinuteSecond.x == hour);
        eq = eq && (hourMinuteSecond.y == minute);
        eq = eq && (hourMinuteSecond.z == second);
        return eq;
    }

    public void AddTime(int delta)
    {
        second += delta;
        while (second >= secondsPerMinute)
        {
            second -= (int)secondsPerMinute;
            minute++;
            while (minute >= minutesPerHour)
            {
                minute -= (int)minutesPerHour;
                hour++;
                while (hour >= hoursPerDay)
                {
                    hour -= (int)hoursPerDay;
                    date++;
                    while (date > daysPerMonth)
                    {
                        date -= (int)daysPerMonth;
                        month++;
                        while (month > monthsPerYear)
                        {
                            month -= (int)monthsPerYear;
                            year++;
                        }
                    }
                }
            }
        }
    }

    public Tapestry_TimeIndex GetIndexFromOffset(int seconds, int minutes=0, int hours=0, int days=0, int months=0, int years=0)
    {
        Tapestry_TimeIndex ti = new Tapestry_TimeIndex(Second,Minute,Hour,Date,Month,Year);
        int delta = seconds;
        delta += (int)(minutes * secondsPerMinute);
        delta += (int)(hours * minutesPerHour * secondsPerMinute);
        delta += (int)(days * hoursPerDay * minutesPerHour * secondsPerMinute);
        delta += (int)(months * daysPerMonth * hoursPerDay * minutesPerHour * secondsPerMinute);
        delta += (int)(years * monthsPerYear * daysPerMonth * hoursPerDay * minutesPerHour * secondsPerMinute);

        ti.AddTime(delta);
        return ti;
    }

    public Tapestry_TimeIndex GetIndexFromOffset(Tapestry_TimeIndex ti)
    {
        return GetIndexFromOffset(ti.second, ti.minute, ti.hour, ti.date, ti.month, ti.year);
    }

    public float ProgressUntilNextDay(float partialSeconds)
    {
        float prog = (float)hour / hoursPerDay;
        prog += ((float)minute / minutesPerHour) / hoursPerDay;
        prog += ((float)second / secondsPerMinute) / minutesPerHour / hoursPerDay;
        prog += partialSeconds / secondsPerMinute / minutesPerHour / hoursPerDay;
        return prog;
    }

    #if UNITY_EDITOR
    public void DrawInspector(bool useMonths=false, bool useYears=false)
    {
        if (useYears)
        {
            year = EditorGUILayout.DelayedIntField(year, GUILayout.Width(28));
            GUILayout.Label("Years");
            GUILayout.FlexibleSpace();
        }
        if (useMonths)
        {
            month = EditorGUILayout.DelayedIntField(month, GUILayout.Width(28));
            GUILayout.Label("Months");
            GUILayout.FlexibleSpace();
        }
        date = EditorGUILayout.DelayedIntField(date, GUILayout.Width(28));
        GUILayout.Label("Days");
        GUILayout.FlexibleSpace();
        hour = EditorGUILayout.DelayedIntField(hour, GUILayout.Width(28));
        GUILayout.Label("Hours");
        GUILayout.FlexibleSpace();
        minute = EditorGUILayout.DelayedIntField(minute, GUILayout.Width(28));
        GUILayout.Label("Minutes");
        GUILayout.FlexibleSpace();
        second = EditorGUILayout.DelayedIntField(second, GUILayout.Width(28));
        GUILayout.Label("Seconds");
    }
    #endif
}