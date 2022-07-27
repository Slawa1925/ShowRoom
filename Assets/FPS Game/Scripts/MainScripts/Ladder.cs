using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject exit0;
    public GameObject exit1;

    public GameObject bottomPoint;
    public GameObject topPoint;

    public float climbDistance;

    void Start()
    {
        climbDistance = Vector3.Distance(bottomPoint.transform.position, topPoint.transform.position);
    }
}
