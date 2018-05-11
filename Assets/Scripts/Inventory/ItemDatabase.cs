using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

    public List<Item>  Items = new List<Item>();

    void Start()
    {
        //Sprite[] enemySprites = Resources.LoadAll("Sprites/Enemies");
        Items.Add(new Item("Apple",0, "Apple Consumable", 1, Item.ItemType.Consumable));
        Items.Add(new Item("Sowrd", 1, "Sowrd Weapon", 10, Item.ItemType.Weapon));
        Items.Add(new Item("Grape", 2, "Grape Consumable Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable ", 1, Item.ItemType.Consumable));
    }
}
