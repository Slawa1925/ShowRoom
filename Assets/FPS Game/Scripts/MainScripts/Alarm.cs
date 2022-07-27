using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public GameObject redLight;
    public bool alarmOn;
    public float rotationSpeed;
    public Vector3 rotationDir;
    public float cooldown = 10.0f;
    public float time;
    public Light[] spotLights;

    void Update()
    {
        if (alarmOn)
        {
            redLight.transform.Rotate(rotationDir * rotationSpeed * Time.deltaTime);
            time += Time.deltaTime;

            if (time >= cooldown)
            {
                AlarmStop();
            }
        }
    }

    public void AlarmStart()
    {
        alarmOn = true;
        time = 0;
        GetComponent<AudioSource>().Play();
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].enabled = true; 
        }
    }
    public void AlarmStop()
    {
        alarmOn = false;
        GetComponent<AudioSource>().Stop();
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].enabled = false;
        }
    }
}
