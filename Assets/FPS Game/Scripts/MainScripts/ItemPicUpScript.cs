using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicUpScript : MonoBehaviour
{
    public GameObject model;
    public GameObject player;
    public ItemData.ItemName itemPropertis;
    public ItemData.KeyType keyType;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public bool TakeItem()
    {
        if (player.GetComponent<InventoryScript>().AddItem(itemPropertis, 1, keyType))
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
