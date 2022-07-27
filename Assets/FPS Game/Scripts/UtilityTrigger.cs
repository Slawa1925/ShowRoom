using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityTrigger : MonoBehaviour
{
    public bool onTriggerStay;

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            onTriggerStay = true;
        }
    }
}
