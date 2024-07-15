using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DayTime
{
    [Header("Общие свойства")]
    [Tooltip("Название диапазона")]
    public string Name;

    [Tooltip("Продолжительность в часах, минутах и секундах")]
    public Vector3Int duration = new Vector3Int(0,0,0);

    public int DurationInSeconds
    {
        get
        {
            return duration[0] * 3600 + duration[1] * 60 + duration[2];
        }
    }


    [Header("Настройки неба")]
    [Tooltip("Список имен числовых параметров")]
    public List<string> FloatValuesNames = new List<string>();

    [Tooltip("Список имен цветовых параметров")]
    public List<string> ColorValuesNames = new List<string>();

    [Tooltip("Список значений числовых параметров")]
    public List<float> FloatValuesData = new List<float>();

    [Tooltip("Список значений цветовых параметров")]
    public List<Color> ColorValuesData = new List<Color>();

    [Header("Настройки освещения")]
    [Tooltip("Цвет освещения")]
    public Color LightColor;

    [Tooltip("Угол освещения")]
    public Vector3 LightAngle;

    [Tooltip("Интенсивность света")]
    public float Itencity;

    public void InsertParameters(ParametersSet parametersSet)
    {
        FloatValuesNames = parametersSet.FloatValues;
        foreach (string f in FloatValuesNames)
        {
            FloatValuesData.Add(0);
        }

        ColorValuesNames = parametersSet.ColorValues;
        foreach (string s in ColorValuesNames)
        {
            ColorValuesData.Add(Color.black);
        }
    }

    public void CopyValues(DayTime dt)
    {
        ColorValuesData.Clear();
        FloatValuesData.Clear();

        foreach (float f in dt.FloatValuesData)
            FloatValuesData.Add(f);
        foreach (Color c in dt.ColorValuesData)
            ColorValuesData.Add(c);

        LightColor = dt.LightColor;
        LightAngle = dt.LightAngle;
        Itencity = dt.Itencity;
    }
}