using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item 
{

    public enum ItemType
    {
        Weapon,
        Consumable,
        Useable,
        Chest,
        Head,
        Hands,
        Legs,
        Feet,
        Quest
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Icon { get; set; }
    public int Cost { get; set; }
    public int Power { get; set; }
    public int Speed { get; set; }
    public int MaxStackCnt { get; set; }
    public ItemType Type { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public int Vitality { get; set; }
    public int Defence { get; set; }
    public string Slug { get; set; }





    public Item(int id, string name,string desc,int cost, ItemType type)
    {
        Id = id;
        this.Name = name;
        Description = desc;
        Sprite[] myFruit = Resources.LoadAll<Sprite>("Inventory/34x34Icons");
        Icon = myFruit[id];
        Cost = cost;
        Power = 0;
        Type = type;
        Speed = 0;
        MaxStackCnt = 10;
        Stackable = false;
        Rarity = 0;
        Vitality = 0;
        Defence = 0;
        Slug = name.Replace(" ", "_"); 
    }

    public Item(string json)
    {
        Id = -1;
    }

    

    public Item()
    {
        Id = -1;
    }

}
