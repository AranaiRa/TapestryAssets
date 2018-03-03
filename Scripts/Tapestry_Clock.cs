using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Clock {

    public static uint
        secondsPerMinute = 60,
        minutesPerHour   = 60,
        hoursPerDay      = 24,
        daysPerMonth     = 90,
        startingMinute   = 0,
        startingHour     = 6,
        startingDate     = 23,
        startingYear     = 1220;
    public static DaysOfTheWeek startingDay = DaysOfTheWeek.Monday;
    public static MonthsOfTheYear startingMonth = MonthsOfTheYear.Spring;

    private float leftoverTime;
    private int
        second,
        minute,
        hour,
        date,
        year;
    private DaysOfTheWeek day;
    private MonthsOfTheYear month;

    public DaysOfTheWeek Day
    {
        get { return day; }
    }

    public MonthsOfTheYear Month
    {
        get { return month; }
    }

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

    public int Year
    {
        get { return year; }
    }

    public Tapestry_Clock()
    {
        ResetClock();
    }

    public void ResetClock()
    {
        second = 0;
        minute = 0;
        hour = (int)startingHour;
        day = startingDay;
        date = (int)startingDate;
        month = startingMonth;
        year = (int)startingYear;
    }

    public void ProgressTime(float delta)
    {
        leftoverTime += delta;
        if(leftoverTime >= 1)
        {
            leftoverTime--;

            second++;
            if(second >= secondsPerMinute)
            {
                second = 0;
                minute++;
                if(minute >= minutesPerHour)
                {
                    minute = 0;
                    hour++;
                    if(hour >= hoursPerDay)
                    {
                        hour = 0;
                        day++;
                        if ((int)day > Enum.GetValues(typeof(DaysOfTheWeek)).Length)
                            day = 0;

                        date++;
                        if(date > daysPerMonth)
                        {
                            date = 0;
                            month++;
                            if ((int)month > Enum.GetValues(typeof(MonthsOfTheYear)).Length)
                            {
                                month = 0;
                                year++;
                            }
                        }
                    }
                }
            }
        }
    }
}

public enum DaysOfTheWeek
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
}

public enum MonthsOfTheYear
{
    Spring, Summer, Autumn, Winter
}