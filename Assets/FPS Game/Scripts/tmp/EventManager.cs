using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action<int> OnShot;
    public static event Action<int, int> Damage;

    public static void DamageObject(int id, int damage)
    {
        if (Damage != null)
        {
            Damage(id, damage);
        }
    }
}
