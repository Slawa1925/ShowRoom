using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [System.Serializable]
    public class Slot
    {
        public GameObject gameObject;
        public GameObject item;
        public Image itemImage;
        public Text itemCount;
        public Text itemName;

        public Slot(GameObject _gameObject, GameObject _item, Image _itemImage, Text _itemCount, Text _itemName)
        {
            gameObject = _gameObject; 
            item = _item;
            item.SetActive(false);
            itemImage = _itemImage;
            itemCount = _itemCount;
            itemName = _itemName;
        }

        public void UpdateElement(bool _hasItem)
        {
            item.SetActive(false);
        }
        public void UpdateElement(Sprite _itemTexture, int _itemCount, string _itemName, bool _hasItem)
        {
            item.SetActive(_hasItem);
            itemImage.sprite = _itemTexture;
            itemCount.text = "" + _itemCount;
            itemName.text = _itemName;
        }
    }

    public Slot[] slot;
    public InventoryScript inventoryScript;
    public GameObject dragObject;
    public GameObject inventoryUIElements;

#if UNITY_EDITOR
    public bool autoAssign;
    public GameObject storage;
    public GameObject equipment;

    void OnValidate()
    {
        if (autoAssign)
        {
            slot = new Slot[12];
            Debug.Log("assign start");
            if (storage != null)
            {
                for(int i = 0; i < 9; i++)
                {
                    Debug.Log(i);
                    storage.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemSlotScript>().index = i;
                    slot[i] = new Slot(
                        storage.transform.GetChild(i).gameObject,
                        storage.transform.GetChild(i).GetChild(0).gameObject,
                        storage.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>(),
                        storage.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>(),
                        storage.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>()
                        );
                }
            }

            if (equipment != null)
            {
                for (int i = 9; i < 12; i++)
                {
                    Debug.Log(i);
                    equipment.transform.GetChild(i - 9).GetChild(0).gameObject.GetComponent<ItemSlotScript>().index = i;
                    slot[i] = new Slot(
                        equipment.transform.GetChild(i - 9).gameObject,
                        equipment.transform.GetChild(i - 9).GetChild(0).gameObject,
                        equipment.transform.GetChild(i - 9).GetChild(0).GetChild(0).GetComponent<Image>(),
                        equipment.transform.GetChild(i - 9).GetChild(0).GetChild(1).GetComponent<Text>(),
                        equipment.transform.GetChild(i - 9).GetChild(0).GetChild(2).GetComponent<Text>()
                        );
                }
            }
            autoAssign = false;
        }
    }
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < slot.Length; i++)
        {
            slot[i].gameObject.GetComponent<SlotHandler>().index = i;
        }
    }

    public void MoveToPointerSlot(int _index, Vector3 _position)
    {
        dragObject.GetComponent<ItemDragHandler>().index = _index;
        dragObject.transform.position = _position;
    }

    public void BeginDrag(int _itemIndex, int _count)
    {
        dragObject.SetActive(true);
        dragObject.transform.position = Input.mousePosition;
        dragObject.GetComponent<ItemDragHandler>().UpdateDrag(_itemIndex, _count);
    }
    public void EndDrag()
    {
        dragObject.SetActive(false);
    }

    public void EjectItem()
    {
        Debug.Log("EjectItem");
        inventoryScript.ItemEjection();
    }
}
