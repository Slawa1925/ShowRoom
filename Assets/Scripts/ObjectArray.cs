using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectArray : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private int countX;
    [SerializeField] private int countZ;

    [SerializeField] private int old_countX;
    [SerializeField] private int old_countZ;

    [SerializeField] private float distX;
    [SerializeField] private float distZ;
    [SerializeField] private GameObject[] objects;


    public void Generate()
    {
        objects = new GameObject[countX * countZ];

        old_countX = countX;
        old_countZ = countZ;

        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countZ; j++)
            {
                var obj = Instantiate(targetObject, transform);
                obj.transform.position += new Vector3(distX * i, 0, distZ * j);
                objects[i * countZ + j] = obj;
            }
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            print("NO");
            return;
        }
        UpdateLevel();
    }

    public void UpdateLevel()
    {
        if (old_countX != countX || old_countZ != countZ || objects.Length == 0)
        {
            ClearLevel();
            Generate();
        }
        else
        {
            for (int i = 0; i < countX; i++)
            {
                for (int j = 0; j < countZ; j++)
                {
                    objects[i * countZ + j].transform.position = targetObject.transform.position + new Vector3(distX * i, 0, distZ * j);
                }
            }
        }
    }

    public void ClearLevel()
    {
        print("clear");
        for (int i = 0; i < objects.Length; i++)
        {
            DestroyImmediate(objects[i]);
        }
    }
}
