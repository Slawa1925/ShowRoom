using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IPointerUpHandler, IDragHandler, IDropHandler
{
    public Image itemImage;
    public Text itemCount;
    public Text itemName;
    public ItemsData itemsData;
    public int index;

    public static bool mouse0Down;

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemName.gameObject.SetActive(true);
        if (!mouse0Down)
        {
            InventoryUI.Instance.inventoryScript.TryDragEnd(index);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemName.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryUI.Instance.inventoryScript.TryDragStart(index);
        //InventoryUI.Instance.dragObject.SetActive(true);
        //Debug.Log("Start drag drom [" + index + "]");
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("DragFrom [" + index + "]");
        //InventoryUI.Instance.dragObject.transform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Drop at slot [" + index + "]");
        InventoryUI.Instance.inventoryScript.TryDragEnd(index);
        //InventoryUI.Instance.dragObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mouse0Down = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mouse0Down = false;
        }
    }
}
