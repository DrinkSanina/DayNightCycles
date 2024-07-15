using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DayTimeCycle", menuName = "Environment/DayTimeCycle")]
public class DayTimeCycles : ScriptableObject
{
    public List<DayTime> dayTimes = new List<DayTime>();

    [Tooltip("Набор параметров среды")]
    public ParametersSet parametersSet;


    public int FullCycleDurationInSeconds
    {
        get
        {
            int duration = 0;
            foreach(DayTime dt in dayTimes)
            {
                duration += dt.DurationInSeconds;
            }
            return duration;
        }
    }

    public Vector3Int FullCycleDuration
    {
        get
        {
            Vector3Int duration = new Vector3Int(0,0,0);
            duration[2] = FullCycleDurationInSeconds % 60;
            duration[1] = FullCycleDurationInSeconds / 60 % 60;
            duration[0] = FullCycleDurationInSeconds / 3600;
            return duration;
        }
    }

    public int CycleCount
    {
        get
        {
            return dayTimes.Count;
        }
    }

    public void CopyValues(int targetInd, int sourceInd)
    {
        dayTimes[targetInd].CopyValues(dayTimes[sourceInd]);
    }

    public int StartingSecond(int index)
    {
        if (index < dayTimes.Count)
        {
            int startingSecond = 0;

            for (int i = 0; i < index; i++)
            {
                startingSecond += dayTimes[i].DurationInSeconds;
            }

            return startingSecond;
        }
        else
        {
            Debug.LogError("Current cycles has no such index");
            return 0;
        }
    }

    public void InsertParameters()
    {
        if (parametersSet != null)
        {
            foreach (DayTime dt in dayTimes)
                dt.InsertParameters(parametersSet);
        }
    }

    public int Define(int timeInSeconds)
    {
        for(int i = 0; i < CycleCount; i++)
        {
            if(i == CycleCount - 1)
            {
                if(timeInSeconds >= StartingSecond(i) && timeInSeconds <= FullCycleDurationInSeconds)
                {
                    return i;
                }
            }
            else
            {
                if (timeInSeconds >= StartingSecond(i) && timeInSeconds <= StartingSecond(i + 1))
                {
                    return i;
                }
            }
            
        }

        return 0;
    }
}