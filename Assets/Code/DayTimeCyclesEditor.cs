using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DayTimeCycles), true)]
public class DayTimeCyclesEditor : Editor
{
    DayTimeCycles dtc;
    SerializedObject GetTarget;
    SerializedProperty timelist;
    SerializedProperty parametersSet;
    int listSize;

    Light currentLight;
    Material currentSkybox;

    private void OnEnable()
    {
        dtc = (DayTimeCycles)target;
        GetTarget = new SerializedObject(dtc);
        timelist = GetTarget.FindProperty("dayTimes");
        parametersSet = GetTarget.FindProperty("parametersSet");

        Object[] lighting = GameObject.FindObjectsOfType<Light>();
        if (lighting.Length != 0)
            currentLight = (Light) lighting[0];
            
        if(RenderSettings.skybox != null)
            currentSkybox = RenderSettings.skybox;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(parametersSet, new GUIContent("Набор параметров неба"));
        GetTarget.ApplyModifiedProperties();
        dtc.InsertParameters();
        currentLight = (Light) EditorGUILayout.ObjectField("Освещение для теста", currentLight, typeof(Light));
        currentSkybox = (Material) EditorGUILayout.ObjectField("Скайбокс для теста", currentSkybox, typeof(Material));

        GetTarget.Update();

        EditorGUILayout.Vector3IntField("Время цикла (Ч:М:С)", dtc.FullCycleDuration);

        listSize = timelist.arraySize;
        listSize = EditorGUILayout.IntField("Number of day times", listSize);

        if(listSize != timelist.arraySize && listSize >= 0)
        {
            while (listSize > timelist.arraySize)
                timelist.InsertArrayElementAtIndex(timelist.arraySize);
            while (listSize < timelist.arraySize)
                timelist.DeleteArrayElementAtIndex(timelist.arraySize - 1);
        }

        EditorGUILayout.Space();

        for(int i = 0; i < timelist.arraySize;i++)
        {
            //Поиск свойств
            SerializedProperty ListRef = timelist.GetArrayElementAtIndex(i);

            SerializedProperty nameProperty = ListRef.FindPropertyRelative("Name");
            SerializedProperty durationProperty = ListRef.FindPropertyRelative("duration");

            SerializedProperty FloatValuesNamesProperty = ListRef.FindPropertyRelative("FloatValuesNames");
            SerializedProperty ColorValuesNamesProperty = ListRef.FindPropertyRelative("ColorValuesNames");
            SerializedProperty FloatValuesDataProperty = ListRef.FindPropertyRelative("FloatValuesData");
            SerializedProperty ColorValuesDataProperty = ListRef.FindPropertyRelative("ColorValuesData");

            SerializedProperty LightColorProperty = ListRef.FindPropertyRelative("LightColor");
            SerializedProperty LightAngleProperty = ListRef.FindPropertyRelative("LightAngle");
            SerializedProperty ItencityProperty = ListRef.FindPropertyRelative("Itencity");

            EditorGUILayout.LabelField($"Настройка диапазона №{i+1}", EditorStyles.toolbarPopup);

            EditorGUILayout.BeginBuildTargetSelectionGrouping();

            EditorGUILayout.PropertyField(nameProperty);
            EditorGUILayout.PropertyField(durationProperty);


            //Туть изменения

            for(int j = 0; j < FloatValuesNamesProperty.arraySize;j++)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(FloatValuesDataProperty.GetArrayElementAtIndex(j), new GUIContent(FloatValuesNamesProperty.GetArrayElementAtIndex(j).stringValue));
                if (EditorGUI.EndChangeCheck())
                    currentSkybox.SetFloat(FloatValuesNamesProperty.GetArrayElementAtIndex(j).stringValue, FloatValuesDataProperty.GetArrayElementAtIndex(j).floatValue);
            }

            for (int j = 0; j < ColorValuesNamesProperty.arraySize; j++)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(ColorValuesDataProperty.GetArrayElementAtIndex(j), new GUIContent(ColorValuesNamesProperty.GetArrayElementAtIndex(j).stringValue));
                if (EditorGUI.EndChangeCheck())
                    currentSkybox.SetColor(ColorValuesNamesProperty.GetArrayElementAtIndex(j).stringValue, ColorValuesDataProperty.GetArrayElementAtIndex(j).colorValue);
            }

            EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(LightColorProperty);
            if (EditorGUI.EndChangeCheck())
                currentLight.color = LightColorProperty.colorValue;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightAngleProperty);
            if (EditorGUI.EndChangeCheck())
            {
                currentLight.gameObject.transform.rotation = Quaternion.Euler(LightAngleProperty.vector3Value);
            }
                

            EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(ItencityProperty);
            if (EditorGUI.EndChangeCheck())
                currentLight.intensity = ItencityProperty.floatValue;


            if (GUILayout.Button("Установить эти параметры в сцене"))
            {
                for (int j = 0; j < FloatValuesNamesProperty.arraySize; j++)
                {

                    currentSkybox.SetFloat(FloatValuesNamesProperty.GetArrayElementAtIndex(j).stringValue, FloatValuesDataProperty.GetArrayElementAtIndex(j).floatValue);
                }

                for (int j = 0; j < ColorValuesNamesProperty.arraySize; j++)
                {
                    currentSkybox.SetColor(ColorValuesNamesProperty.GetArrayElementAtIndex(j).stringValue, ColorValuesDataProperty.GetArrayElementAtIndex(j).colorValue);
                }

                currentLight.color = LightColorProperty.colorValue;
                currentLight.gameObject.transform.rotation = Quaternion.Euler(LightAngleProperty.vector3Value);
                currentLight.intensity = ItencityProperty.floatValue;
            }

            if(GUILayout.Button("Скопировать текущие параметры сцены"))
            {

                for (int j = 0; j < FloatValuesNamesProperty.arraySize; j++)
                {
                    dtc.dayTimes[i].FloatValuesData[j] = currentSkybox.GetFloat(FloatValuesNamesProperty.GetArrayElementAtIndex(j).stringValue);
                }

                for (int j = 0; j < ColorValuesNamesProperty.arraySize; j++)
                {
                    dtc.dayTimes[i].ColorValuesData[j] = currentSkybox.GetColor(ColorValuesNamesProperty.GetArrayElementAtIndex(j).stringValue);
                }

                dtc.dayTimes[i].LightColor = currentLight.color;
                dtc.dayTimes[i].LightAngle = currentLight.gameObject.transform.rotation.eulerAngles;
                dtc.dayTimes[i].Itencity = currentLight.intensity;
            }

            if (GUILayout.Button("Скопировать прошлые данные цикла"))
            {
                if(i != 0)
                {
                    dtc.CopyValues(i, i - 1);
                }
                
            }

            EditorGUILayout.EndBuildTargetSelectionGrouping();
            
        }

        GetTarget.ApplyModifiedProperties();
    }


}
