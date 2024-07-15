using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime
{
    public float realTimeInSeconds;

    public int hour;
    public int minute;
    public int second;

    private float speed;
    private int flooredSecond;
    private float timePassedInterval;

    //Время цикла в часах, минутах и секундах
    private Vector3Int fullCycleTime;

    public DayTimeCycles currentCycles;


    public DayTime currentDayTime;
    public DayTime nextDayTime;

    public int currentDayTimeIndex;

    //Время цикла в секундах
    private int FullCycleTimeInSeconds
    {
        get
        {
            return fullCycleTime[0] * 3600 + fullCycleTime[1] * 60 + fullCycleTime[2];
        }
    }
    public float TimeSpeed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public Vector3Int CycleTime
    {
        get
        {
            return fullCycleTime;
        }
        set
        {
            fullCycleTime = value;
        }
    }

    public GameTime()
    {
        realTimeInSeconds = 0f;
        timePassedInterval = 0f;
        flooredSecond = 0;

        speed = 1;
        hour = 0;
        minute = 0;
        second = 0;

        fullCycleTime = new Vector3Int(23, 56, 4);
    }


    public void Tick()
    {
        realTimeInSeconds += Time.deltaTime * speed;
        timePassedInterval += Time.deltaTime * speed;

        if(timePassedInterval >=1)
        {
            timePassedInterval = 0;
            TickSecond();
        }

        flooredSecond = Mathf.FloorToInt(realTimeInSeconds);

        second = flooredSecond % 60;
        minute = flooredSecond / 60 % 60;
        hour = flooredSecond / 3600 % (fullCycleTime[0]+1);

        

        //Сброс цикла по достижении максимального числа секунд
        if (realTimeInSeconds >= FullCycleTimeInSeconds)
            realTimeInSeconds = 0;

    }

    /// <summary>
    /// Действия, происходящие каждую секунду времени
    /// </summary>
    public void TickSecond()
    {
        DefineDayTime();
    }

    public void DefineDayTime()
    {
        currentDayTimeIndex = currentCycles.Define(flooredSecond);
        currentDayTime = currentCycles.dayTimes[currentDayTimeIndex];

        if (currentDayTimeIndex < currentCycles.CycleCount-1)
            nextDayTime = currentCycles.dayTimes[currentDayTimeIndex + 1];
        else
            nextDayTime = currentCycles.dayTimes[0];
    }

    public string To24HStringFull()
    {
        return $"{hour}:{minute}:{second}";
    }

    public string To24HStringShort()
    {
        return $"{hour}:{minute}";
    }

}