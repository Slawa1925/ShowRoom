using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotScript : MonoBehaviour, IBeginDragHandler, IDropHandler, IPointerEnterHandler
{
    public int index;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag start");
        //InventoryUI.Instance.inventoryScript.TryDragStart(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryUI.Instance.MoveToPointerSlot(index, transform.position);
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
