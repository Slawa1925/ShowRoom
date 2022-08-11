using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    public void GetLights()
    {
        objects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.childCount > 0)
            {
                objects[i] = child.GetChild(0).gameObject;
            }
        }
    }

    public void EnableSoftShadows()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Light>().shadows = LightShadows.Soft;
        }
    }

    public void EnableHardShadows()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Light>().shadows = LightShadows.Hard;
        }
    }

    public void DisableShadows()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Light>().shadows = LightShadows.None;
        }
    }
}
