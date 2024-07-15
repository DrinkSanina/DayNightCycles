using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSystem : MonoBehaviour
{
    public GameTime globalTime = new GameTime();
    public TMP_Text ct;

    [Header("Общая настройка временного цикла")]
    [SerializeField, Tooltip("Сколько секунд пройдет в игре за одну секунду реального времени")]
    private float secondMultiplication = 1f;
    [Tooltip("Используйте настроенный объект циклов")]
    public DayTimeCycles DayTimeCycles;
    
    void Start()
    {
        globalTime.realTimeInSeconds = 0;
        globalTime.TimeSpeed = secondMultiplication;
        globalTime.CycleTime = DayTimeCycles.FullCycleDuration;
        globalTime.currentCycles = DayTimeCycles;
        globalTime.DefineDayTime();
    }

    void Update()
    {
        globalTime.Tick();
       
        if (ct != null)
        {
            ct.text = globalTime.To24HStringFull() + "\r\n" + globalTime.currentDayTime.Name;
        }

    }
}
