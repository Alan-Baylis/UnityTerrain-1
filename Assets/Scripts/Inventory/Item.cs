using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Item 
{

    public enum ItemType
    {
        Weapon,
        Consumable,
        Useable,
        Equipment,
        Chest,
        Head,
        Hands,
        Legs,
        Feet,
        Quest
    }

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Artifact
    }


    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    //public Sprite Icon { get; set; }
    public int Cost { get; set; }
    public int MaxStackCnt { get; set; }
    public int StackCnt { get; set; }
    public ItemType Type { get; set; }
    public ItemRarity Rarity { get; set; }
    public string Slug { get; set; }


    protected Item(int id, string name,string desc,int cost, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity)
    {
        Id = id;

        Name = name;
        Description = desc;
        IconPath = "Inventory/34x34Icons";
        IconId = id;
        Cost = cost;
        Type = type;
        MaxStackCnt = maxStackCnt;
        StackCnt = stackCnt;
        Rarity = rarity;
        Slug = name.Replace(" ", "_"); 
    }


    protected Item()
    {
        Id = -1;
    }
    
    public virtual string GetTooltip()
    {
        string color;
        switch (this.Type)
        {
            case Item.ItemType.Weapon:
                color = "Blue";
                break;
            case Item.ItemType.Consumable:
                color = "Blue";
                break;
            case Item.ItemType.Useable:
                color = "Blue";
                break;
            default:
                color = "white";
                break;
        }
        var tooltip = "<color=" + color + ">" + this.Name + "</color>\n\n" + this.Description + "\n<color=yellow>Cost:" + this.Cost + "</color>\n < color = green > Available:"+ this.StackCnt +" </ color > ";
        return tooltip;
    }


    public Sprite GetSprite()
    {
        Sprite[] spriteList = Resources.LoadAll<Sprite>(IconPath);
        return spriteList[IconId];
    }

    //Abstarct
    public abstract void Usage();


}
