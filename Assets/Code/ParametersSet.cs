using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Parameters Set", menuName = "Environment/Parameters Set")]
public class ParametersSet : ScriptableObject
{
    [Tooltip("Список имен числовых параметров неба")]
    public List<string> FloatValues = new List<string>();

    [Tooltip("Список имен цветовых параметров неба")]
    public List<string> ColorValues = new List<string>();
}
