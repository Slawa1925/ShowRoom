/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    public int id;

    void Start()
    {
        id = gameObject.GetInstanceID();
    }

    void OnEnable()
    {
        EventManager.OnShot += Die;
    }

    void OnDisable()
    {
        EventManager.OnShot -= Die;
    }

    void Die(int id)
    {
        if (id == this.id)
        {
            gameObject.SetActive(false);
        }
    }
}
*/