using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperInventory : MonoBehaviour
{
    public int row = 8;
    public int size = 50;
    public int spacing = 10;
    public AudioClip addItem;
    public AudioClip cantAdd;

    void OnAwake()
    {
        //GetComponent<AudioSource>().pitch = 1;
    }

    void OnGUI()
    {
        for (int i = 0; i < ItemData.items.Length; i++)
        {
            int j = i / row;
            int fullWidth = row * (size + spacing);
            int fullHeight = ItemData.items.Length / row * (size + spacing);
            int x = (Screen.width - fullWidth) / 2;
            int y = (Screen.height - fullHeight) / 2;

            Rect rect = new Rect(x + (i - j * row) * (size + spacing), y + j * (size + spacing), size, size);
            //GUI.Label(rect, ItemData.items[i].name);
            GUI.DrawTexture(rect, ItemData.itemsData.items[i].itemIcon.texture);
            if (GUI.Button(rect, ""))
            {
                Debug.Log("" + i);
                if (InventoryUI.Instance.inventoryScript.AddItem((ItemData.ItemName)i, 1))
                    GetComponent<AudioSource>().PlayOneShot(addItem);
                else
                    GetComponent<AudioSource>().PlayOneShot(cantAdd);
            }
        }
    }
}
