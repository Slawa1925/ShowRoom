using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ItemData
{
    public enum ItemName
    {
        Топор,
        Молоток,
        Монтировка,
        Разводной_ключ,
        Фонарик,
        Головной_фонарь,
        Вальтер,
        Шмайсер,
        Граната,
        Патроны_вальтер,
        Патроны_шмайсер,
        Батарейка,
        Аптечка,
        Гриб,
        Немецкая_форма,
        Удостоверение,
        Немецкая_книга,
        Ключ,
        Ключ_карта
    };
    public enum ItemType { руки, голова, туловище, используемое, пассивно_используемое }
    public enum KeyType { NA, Калитка, Сарай, Гараж_Дверь, Гараж_Люк, Гараж_Мамка, Гараж_Выход }
    public enum UseEvent { NA, Heal, Drug}

    public class Item
    {
        public string name;
        public UseEvent useEvent;
        public Texture itemIcon;
        public ItemType type;

        public Item(string _name, UseEvent _useEvent, ItemType _type)
        {
            name = _name;
            useEvent = _useEvent;
            type = _type;
        }
    }

    public static GameObject[] itemModels;
    public static ItemsData itemsData;

    public static Item[] items = 
    {
        new Item("Топор", 0, ItemType.руки),
        new Item("Молоток", 0, ItemType.руки),
        new Item("Монтировка", 0, ItemType.руки),
        new Item("Разводной ключ", 0, ItemType.руки),
        new Item("Фонарик", 0, ItemType.руки),
        new Item("Головной фонарь", 0, ItemType.голова),
        new Item("Вальтер", 0, ItemType.руки),
        new Item("Шмайсер", 0, ItemType.руки),
        new Item("Граната", 0, ItemType.руки),
        new Item("Патроны вальтер", 0, ItemType.пассивно_используемое),
        new Item("Патроны шмайсер", 0, ItemType.пассивно_используемое),
        new Item("Батарейка", 0, ItemType.пассивно_используемое),
        new Item("Аптечка", UseEvent.Heal, ItemType.используемое),
        new Item("Гриб", UseEvent.Drug, ItemType.используемое),
        new Item("Немецкая форма", 0, ItemType.туловище),
        new Item("Удостоверение", 0, ItemType.руки),
        new Item("Немецкая книга", 0, ItemType.руки),
        new Item("Ключ", 0, ItemType.руки),
        new Item("Ключ-карта", 0, ItemType.руки),
    };


#if UNITY_EDITOR
    [MenuItem("AssetDatabase/LoadAssetExample")]
    public static Texture ImportTexture(string name)
    {
        Texture tempTexure = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Garage2/texures/InventoryTextures/" + name + ".png", typeof(Texture));

        if (tempTexure == null)
            Debug.Log("Alarm!!! " + name);

        return tempTexure;
    }

    [MenuItem("AssetDatabase/LoadAssetExample")]
    public static void LoadModels()
    {
        itemModels = new GameObject[items.Length];

        for (int i = 0; i < itemModels.Length; i++)
        {
            itemModels[i] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Garage2/texures/InventoryTextures/" + items[i].name, typeof(GameObject));
        }
    }
#endif
}
