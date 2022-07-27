using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler, IPointerUpHandler
{
    public int index;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        InventoryUI.Instance.inventoryScript.DragEnd(index);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("drop_up");
        InventoryUI.Instance.inventoryScript.DragEnd(index);
    }
}
