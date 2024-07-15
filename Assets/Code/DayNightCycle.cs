using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material originalSkyBox;
    public Light sceneLighting;
    public TimeSystem TimeController;

    private Material realSkyBox;    
    private int currentRenderingDayTimeIndex=-1;


    void Start()
    {
        realSkyBox = Instantiate(originalSkyBox);
        RenderSettings.skybox = realSkyBox;
    }

    void Update()
    {
        
        if(currentRenderingDayTimeIndex != TimeController.globalTime.currentDayTimeIndex)
        {
            currentRenderingDayTimeIndex = TimeController.globalTime.currentDayTimeIndex;

            DayTime currentDayTime = TimeController.globalTime.currentDayTime;
            DayTime nextDayTime = TimeController.globalTime.nextDayTime;
            float multipliedDayTimeDuration = currentDayTime.DurationInSeconds / TimeController.globalTime.TimeSpeed;


            //Запуск поворота источника света и изменения цвета
            StartCoroutine(RotateLightWithinSeconds(Quaternion.Euler(currentDayTime.LightAngle), Quaternion.Euler(nextDayTime.LightAngle), multipliedDayTimeDuration));
            StartCoroutine(ChangeLightColorWithinSeconds(currentDayTime.LightColor, nextDayTime.LightColor, multipliedDayTimeDuration));
            StartCoroutine(ChangeItencityWithinSeconds(currentDayTime.Itencity, nextDayTime.Itencity, multipliedDayTimeDuration));

            //Запуск изменения цветов
            for(int i = 0; i < currentDayTime.ColorValuesNames.Count;i++)
            {
                StartCoroutine(ChangeColorWithinSeconds(currentDayTime.ColorValuesNames[i], currentDayTime.ColorValuesData[i], nextDayTime.ColorValuesData[i], multipliedDayTimeDuration));
            }

            //Запуск изменения численных величин
            for (int i = 0; i < currentDayTime.FloatValuesNames.Count; i++)
            {
                StartCoroutine(ChangeFloatWithinSeconds(currentDayTime.FloatValuesNames[i], currentDayTime.FloatValuesData[i], nextDayTime.FloatValuesData[i], multipliedDayTimeDuration));
            }
        }
        
    }


    IEnumerator RotateLightWithinSeconds(Quaternion from, Quaternion to, float duration)
    {
        float timePassed = 0f;
        Quaternion parameterData;

        while(timePassed < duration)
        {
            var factor = timePassed / duration;

            parameterData = Quaternion.Slerp(from, to, factor);
            sceneLighting.gameObject.transform.rotation = parameterData;
            timePassed += Mathf.Min(Time.deltaTime, duration - timePassed);
            yield return null;
        }

        parameterData = to;
    }


    IEnumerator ChangeLightColorWithinSeconds(Color from, Color to, float duration)
    {
        float timePassed = 0f;
        Color parameterData;

        while (timePassed < duration)
        {
            var factor = timePassed / duration;

            parameterData = Color.Lerp(from, to, factor);
            sceneLighting.color = parameterData;
            timePassed += Mathf.Min(Time.deltaTime, duration - timePassed);
            yield return null;
        }

        parameterData = to;
    }

    IEnumerator ChangeItencityWithinSeconds(float from, float to, float duration)
    {
        float timePassed = 0f;
        float parameterData;

        while (timePassed < duration)
        {
            var factor = timePassed / duration;

            parameterData = Mathf.Lerp(from, to, factor);
            sceneLighting.intensity = parameterData;
            timePassed += Mathf.Min(Time.deltaTime, duration - timePassed);
            yield return null;
        }

        parameterData = to;
    }

    IEnumerator ChangeColorWithinSeconds(string parameterName, Color from, Color to, float duration)
    {
        float timePassed = 0f;
        Color parameterData;

        while(timePassed < duration)
        {
            var factor = timePassed / duration;

            parameterData = Color.Lerp(from, to, factor);
            RenderSettings.skybox.SetColor(parameterName, parameterData);
            timePassed += Mathf.Min(Time.deltaTime, duration - timePassed);
            yield return null;
        }

        parameterData = to;
    }

    IEnumerator ChangeFloatWithinSeconds(string parameterName, float from, float to, float duration)
    {
        float timePassed = 0f;
        float parameterData;

        while(timePassed < duration)
        {
            var factor = timePassed / duration;

            parameterData = Mathf.Lerp(from, to, factor);
            RenderSettings.skybox.SetFloat(parameterName, parameterData);
            timePassed += Mathf.Min(Time.deltaTime, duration - timePassed);
            yield return null;
        }

        parameterData = to;
    }

}
