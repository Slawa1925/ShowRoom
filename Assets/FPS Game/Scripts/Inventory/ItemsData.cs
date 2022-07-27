using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "GarageSimulator/Items Data")]
public class ItemsData : ScriptableObject
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public Sprite itemIcon;
    }

    public Item[] items;
}