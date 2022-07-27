using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour
{
    public Image itemImage;
    public Text itemCount;
    public Text itemName;
    public ItemsData itemsData;
    public int index;
    public CanvasGroup canvasGroup;


    public void UpdateDrag(int _itemIndex, int _itemCount)
    {
        //Debug.Log(_itemIndex);
        itemImage.sprite = itemsData.items[_itemIndex].itemIcon;
        itemName.text = ItemData.items[_itemIndex].name;
        itemCount.text = "" + _itemCount;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
