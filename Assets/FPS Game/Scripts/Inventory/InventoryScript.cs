using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Texture))]
public class InventoryScript : MonoBehaviour
{
    public enum SlotType { руки, голова, туловище, инвентарь }

    public string filePath = "/GameDevelopment/UnitySavesTest/InventorySave.txt";
    public Texture[] Textures;
    public Texture CursorTex;
    public bool Opened;
    public GameObject ItemBox; // выброшенная вещь
    public GameObject PCamera;
    public ObjectDragScript ObjectDragScr;
    public GameObject ObjPoint;
    public GameObject[] handItems;
    public GameObject[] Armor;
    public GameObject weapons;

    public AudioClip[] Audio;

    public StatsAndMovementScript PlayerStats;

    [System.Serializable]
    public class Slot
    {
        //private int index;
        public SlotType type;
        public string name;
        public bool Blocked; // блокировка слота
        public bool MEnter; // наведение курсора


        public int Count;
        public int itemIndex;
        public ItemData.KeyType keyType; // the number for specific door


        // предмет ( в слоте )
        public Rect GUIinfo; // размер и координаты слота
        public Rect GUIinfo1; // размер и координаты чила предметов
        public Rect GUIinfo2; // размер и координаты названия предмета

        public Slot(int _Count, int _itemIndex, ItemData.KeyType _keyType)
        {
            Count = _Count;
            itemIndex = _itemIndex;
            keyType = _keyType;
        }

        public void UpdateElement(int i)
        {
            if (itemIndex == -1)
            {
                InventoryUI.Instance.slot[i].UpdateElement(false);
            }
            else
            {
                InventoryUI.Instance.slot[i].UpdateElement(
                    ItemData.itemsData.items[itemIndex].itemIcon,
                    Count,
                    ItemData.items[itemIndex].name + ((keyType == ItemData.KeyType.NA) ? "" : (" - " + keyType)),
                    true
                    );
            }
        }
    }

    [System.Serializable]
    public class Drag
    {
        public bool On;
        public int Count; // кол-во предметов
        public int itemIndex; // предмет ( в слоте )
        public ItemData.KeyType keyType;
        public Vector2 MousePos;
    }

    Vector2 MouseDeltaPos; //
    bool DragFirstStart;
    public Drag drag; // информация о переносимом слоте
    public Slot virtualSlot;
    public Slot[] slot; // массив слотов/кнопок
    int n = 80; // регулировщик размера ячеек
    public Rect GBox;
    Vector2 MPXY; // коодината курсора
    public GameObject TestSpawnPoint;
    bool DoubleClick; // двойной щелчек ( использование/экипировка )
    int ClickCount;
    float ClickTimer; // промежуток между кликами
    public bool loadedTextures = false;
    public Texture[] _itemIcon = new Texture[19];
    public ItemsData _itemsData;


#if UNITY_EDITOR
    void OnValidate()
    {
        if (!loadedTextures)
        {
            Debug.Log("load start");
            for (int i = 0; i < ItemData.items.Length; i++)
            {
                _itemIcon[i] = ItemData.ImportTexture(ItemData.items[i].name);
                //ItemData.items[i].itemIcon = ItemData.ImportTexture(ItemData.items[i].name);
            }
            loadedTextures = true;
        }
    }
#endif

    void Start()
    {
        ItemData.itemsData = _itemsData;
        for (int i = 0; i < ItemData.items.Length; i++)
            ItemData.items[i].itemIcon = _itemIcon[i];
        InventoryUI.Instance.inventoryScript = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < slot.Length; i++)
        { // автоматическое наименование слотов , итд 
            slot[i].GUIinfo.height = n;
            slot[i].GUIinfo.width = n;
            slot[i].name = "Slot[" + i + "]";
            slot[i].itemIndex = -1;

            if (i < 9)
                slot[i].type = SlotType.инвентарь;
        }

        slot[9].type = SlotType.голова; 
        slot[10].type = SlotType.туловище; // 11
        slot[11].type = SlotType.руки; // 12

        for (int j = 0; j < 12; j++)
        {
            InventoryUI.Instance.slot[j].UpdateElement(false);
        }

        // adding items
        AddItem(ItemData.ItemName.Монтировка, 1);
    }

    public void Inventory(bool open)
    {
        if (open)
            Open();
        else
            Close();
        GetComponent<CharacterMotor>().useFixedUpdate = false;
    }

    public void Open()
    {
        Opened = true;
        PlayerStats.OpenInterface();
        ObjectDragScr.Drop();
        HandUnHide();
        InventoryUI.Instance.inventoryUIElements.SetActive(true);
    }

    public void Close()
    {
        ItemEjection();
        Opened = false;
        PlayerStats.CloseInterface();
        InventoryUI.Instance.inventoryUIElements.SetActive(false);
    }

    public int FindItem(int index)
    {
        for (int i = 0; i < 9; i++)
            if (slot[i].itemIndex == index)
                return i;
        return -1;
    }

    public void ItemEjection()
    { // функция обнуления слота
        if (!drag.On)
            return;

        Debug.Log("Eject");

        for (int i = 0; i < drag.Count; i++)
        {
            GameObject EjectedItem = Instantiate(ItemBox, ObjPoint.transform.position, transform.rotation);
            ItemPicUpScript ES = EjectedItem.GetComponent<ItemPicUpScript>();
            ES.itemPropertis = (ItemData.ItemName)drag.itemIndex;
            ES.keyType = drag.keyType;
            ES.Start();
        }

        ClearDrag();
    }

    public void TryDragStart(int index)
    {
        if (drag.On)
            return;
        if (slot[index].itemIndex == -1)
            return;

        DragStart(index);
    }
    public void DragStart(int index)
    {
        drag.On = true;
        drag.Count = slot[index].Count;
        drag.itemIndex = slot[index].itemIndex;
        drag.keyType = slot[index].keyType;
        InventoryUI.Instance.BeginDrag(drag.itemIndex, drag.Count);

        switch (slot[index].type)
        {
            case (SlotType.туловище):
                Armor[1].SetActive(false);
                break;
            case (SlotType.голова):
                Armor[0].SetActive(false);
                break;
            case (SlotType.руки):
                WeaponHide(ItemData.items[slot[index].itemIndex].name);
                break;
            default:
                break;
        }

        ClearSlot(index);
    }
    public void TryDragEnd(int index)
    {
        if (!drag.On)
            return;
        if (!(slot[index].type == SlotType.инвентарь || (int)slot[index].type == (int)ItemData.items[drag.itemIndex].type))
            return;

        if (slot[index].itemIndex != -1)
            SwapDragAndSlot(index);
        else
            DragEnd(index);
    }
    public void SwapDragAndSlot(int index)
    {
        Debug.Log("SwapDragAndSlot");

        var _drag = new Drag();
        _drag.Count = drag.Count;
        _drag.itemIndex = drag.itemIndex;
        _drag.keyType = drag.keyType;

        switch (slot[index].type)
        {
            case (SlotType.туловище):
                Armor[1].SetActive(false);
                break;
            case (SlotType.голова):
                Armor[0].SetActive(false);
                break;
            case (SlotType.руки):
                WeaponHide(ItemData.items[slot[index].itemIndex].name);
                break;
            default:
                break;
        }

        drag.On = true;
        drag.Count = slot[index].Count;
        drag.itemIndex = slot[index].itemIndex;
        drag.keyType = slot[index].keyType;
        InventoryUI.Instance.BeginDrag(drag.itemIndex, drag.Count);

        slot[index].Count = _drag.Count;
        slot[index].itemIndex = _drag.itemIndex;
        slot[index].keyType = _drag.keyType;

        switch (slot[index].type)
        {
            case (SlotType.туловище):
                Armor[1].SetActive(true);
                break;
            case (SlotType.голова):
                Armor[0].SetActive(true);
                break;
            case (SlotType.руки):
                WeaponUnHide(ItemData.items[slot[index].itemIndex].name);
                break;
            default:
                break;
        }

        slot[index].UpdateElement(index);

    }
    public void DragEnd(int index)
    {
        if ((slot[index].Blocked == false) && ((slot[index].type == SlotType.инвентарь) || ((int)slot[index].type == (int)ItemData.items[drag.itemIndex].type)))
        {
            slot[index].Count = drag.Count;
            slot[index].itemIndex = drag.itemIndex;
            slot[index].keyType = drag.keyType;

            switch (slot[index].type)
            {
                case (SlotType.туловище):
                    Armor[1].SetActive(true);
                    break;
                case (SlotType.голова):
                    Armor[0].SetActive(true);
                    break;
                case (SlotType.руки):
                    WeaponUnHide(ItemData.items[drag.itemIndex].name);
                    break;
                default:
                    break;
            }

            slot[index].UpdateElement(index);
            ClearDrag();
        }
    }

    public void ClearSlot(int index)
    {
        slot[index].Count = 0;
        slot[index].itemIndex = -1;
        slot[index].keyType = ItemData.KeyType.NA;
        slot[index].UpdateElement(index);
    }

    public void ClearDrag()
    {
        drag.On = false;
        InventoryUI.Instance.EndDrag();
    }

    public void Heal(Vector2 Amount)
    {
        if (PlayerStats.Health < 100)
        {

            PlayerStats.Health += (int)Amount.x;

            if (PlayerStats.Health > 100)
                PlayerStats.Health = 100;

            if (slot[(int)Amount.y].Count > 1)
                slot[(int)Amount.y].Count -= 1;
            else
            {

                slot[(int)Amount.y].Count = 0;
                slot[(int)Amount.y].itemIndex = -1;

            }
        }
    }

    public void Drug_0(Vector2 Amount)
    { // фукция употребления наркотика 1
        PlayerStats.Drug += (int)Amount.x;

        if (slot[(int)Amount.y].Count > 1)
            slot[(int)Amount.y].Count -= 1;
        else
        {
            slot[(int)Amount.y].Count = 0;
            slot[(int)Amount.y].itemIndex = -1;
        }
    }

    public int FirstFreeSlot()
    {
        for (int i = 0; i < 9; i++)
            if ((slot[i].itemIndex == -1) && (slot[i].Blocked == false))
                return i;

        return -1;
    }
    public int FirstSimilarSlot(int itemIndex)
    {
        for (int i = 0; i < 9; i++)
            if (slot[i].itemIndex == itemIndex)
                return i;

        return -1;
    }

    public void WeaponHide(string name)
    {
        for (int i = 0; i < handItems.Length; i++)
        {
            if (handItems[i].name == name)
            {
                if (handItems[i].GetComponent<RangeWeaponScript>())
                {
                    //handItems[i].GetComponent<RangeWeaponScript>().WeaponPicked();
                    StartCoroutine(handItems[i].GetComponent<RangeWeaponScript>().WeaponHidden());
                }
                else
                {
                    handItems[i].SetActive(false);
                }
                break;
            }
        }
    }

    public void WeaponUnHide(string name)
    {
        for (int i = 0; i < handItems.Length; i++)
        {
            if (handItems[i].name == name)
            {
                handItems[i].SetActive(true);
                break;
            }
        }
    }

    public void HandHide()
    {
        weapons.SetActive(false);
    }

    public void HandUnHide()
    {
        weapons.SetActive(true);
    }

    public bool AddItem(ItemData.ItemName index, int count, int i, ItemData.KeyType keyType = default(ItemData.KeyType))
    {
        drag.On = true;
        drag.itemIndex = (int)index;
        drag.Count = count;
        drag.keyType = keyType;
        DragEnd(i);
        slot[i].UpdateElement(i);
        return false;
    }

    public bool AddItem(ItemData.ItemName index, int count, ItemData.KeyType keyType = default(ItemData.KeyType))
    {
        int i, j;

        j = FirstSimilarSlot((int)index);
        i = FirstFreeSlot();

        if ((ItemData.items[(int)index].type == ItemData.ItemType.руки) || (ItemData.items[(int)index].type == ItemData.ItemType.голова) || (ItemData.items[(int)index].type == ItemData.ItemType.туловище))
        { // для не стакающихся вещей
            if (i == -1) { }
            else
            {
                slot[i].itemIndex = (int)index;
                slot[i].Count = count;
                slot[i].keyType = keyType;
                slot[i].UpdateElement(i);
                return true;
            }
        }
        else
        {
            if (j == -1)
            {
                if (i != -1)
                {
                    slot[i].itemIndex = (int)index;
                    slot[i].Count = count;
                    slot[i].keyType = keyType;
                    slot[i].UpdateElement(i);
                    return true;
                }
            }
            else
            {
                slot[j].Count += count;
                slot[j].UpdateElement(j);
                return true;
            }
        }
        return false;
    }

    public void StartSlotDrag()
    {
        Debug.Log("DragStart");
    }

    public void EndSlotDrag()
    {
        Debug.Log("DragEnd");
    }

    public void Drop()
    {
        Debug.Log("Drop");
    }
}