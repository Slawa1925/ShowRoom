using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStuff : MonoBehaviour {

    public Transform posProp;
    public GameObject prefab;
    public Vector3 position;
    public Vector3 offset;
    public Vector3 size;
    public int countX;
    public int countY;
    public int countZ;
    public bool copyObjectPos = false;

    void Start ()
    {
        int counter = 0;
        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                for (int k = 0; k < countZ; k++)
                {
                    GameObject prefInst = Instantiate(prefab, new Vector3(position.x + offset.x * i, position.y + offset.y * j, position.z + offset.z * k), transform.rotation);
                    if (prefInst.GetComponent<ItemPicUpScript>())
                        prefInst.GetComponent<ItemPicUpScript>().itemPropertis = (ItemData.ItemName)counter;
                    prefInst.transform.parent = transform;
                    counter++;
                }
            }
        }
    }
	

	void Update ()
    {
		
	}

    void OnDrawGizmosSelected()
    {
        if (copyObjectPos)
            position = posProp.position;

        Gizmos.color = Color.green;

        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                for (int k = 0; k < countZ; k++)
                {
                    Gizmos.DrawCube(new Vector3(position.x + offset.x * i, position.y + offset.y * j, position.z + offset.z * k), size);
                }
            }
        }
    }
}
