using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    public Transform player;
    public float lastYposition;
    public float intensity;
    public float maxIntensity;

    public float surfaceHeight;
    public float undergroundHeight;

    float distance;

    void Start()
    {
        distance = Mathf.Abs(undergroundHeight - surfaceHeight);
    }
    void Update()
    {
        if (player.position.y != lastYposition)
        {
            lastYposition = player.position.y;

            intensity = maxIntensity + (player.position.y - surfaceHeight) / distance;

            if (player.position.y < surfaceHeight)
                RenderSettings.ambientIntensity = intensity;
            else
                RenderSettings.ambientIntensity = 1;
        }
    }
}
