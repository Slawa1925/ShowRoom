using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectArray))]
public class LevelGeneratorArray : Editor
{
    private ObjectArray _objectArray;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Level"))
        {
            _objectArray.Generate();
        }

        if (GUILayout.Button("Update Level"))
        {
            _objectArray.UpdateLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            _objectArray.ClearLevel();
        }
    }

    private void OnEnable()
    {
        _objectArray = target as ObjectArray;
    }
}
