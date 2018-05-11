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

    public string Name;
    public int Id;
    public string Description;
    public Sprite Icon;
    public int Cost;
    public int Power;
    public int Speed;
    public int StackCnt;
    public ItemType Type; 


    public Item(string name,int id,string desc,int cost,ItemType type)
    {
        this.Name = name;
        Id = id;
        Description = desc;
        Sprite[] myFruit = Resources.LoadAll<Sprite>("Inventory/34x34Icons");
        Icon = myFruit[id];
        Cost = cost;
        Power = 0;
        Type = type;
        Speed = 0;
        StackCnt = 1;
    }


    public Item()
    {
        Id = -1;
    }

}
