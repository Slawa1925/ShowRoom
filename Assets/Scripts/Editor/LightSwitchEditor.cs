using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightsSwitch))]
public class LightsSwitchEditor : Editor
{
    private LightsSwitch _lightsSwitch;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get Lights"))
        {
            _lightsSwitch.GetLights();
        }

        if (GUILayout.Button("Enable Soft Shadows"))
        {
            _lightsSwitch.EnableSoftShadows();
        }

        if (GUILayout.Button("Enable Hard Shadows"))
        {
            _lightsSwitch.EnableHardShadows();
        }

        if (GUILayout.Button("Disable Shadows"))
        {
            _lightsSwitch.DisableShadows();
        }
    }

    private void OnEnable()
    {
        _lightsSwitch = target as LightsSwitch;
    }
}

