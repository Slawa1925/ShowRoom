using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{

    public bool PlayerEnter;
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            if (enterEvent != null)
                enterEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            if (exitEvent != null)
                exitEvent.Invoke();
        }
    }
}
