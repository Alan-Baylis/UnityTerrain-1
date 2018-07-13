using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

[Serializable]
public abstract class Item 
{

    public enum ItemType
    {
        Consumable,
        Weapon,
        Equipment,
        Substance,
        Element,
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
    public int Cost { get; set; }
    public int Weight { get; set; }
    public int MaxStackCnt { get; set; }
    public int StackCnt { get; set; }
    public ItemType Type { get; set; }
    public ItemRarity Rarity { get; set; }
    public string Slug { get; set; }
    public DateTime ExpirationTime { get; set; }
    public bool IsEnable { get; set; }


    protected Item(int id, string name,string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity,DateTime expirationTime)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = iconPath; 
        IconId = iconId;
        Cost = cost;
        Weight = weight;
        Type = type;
        MaxStackCnt = maxStackCnt;
        StackCnt = stackCnt;
        Rarity = rarity;
        Slug = name.Replace(" ", "_");
        ExpirationTime = expirationTime;
        IsEnable = true;
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
            case Item.ItemType.Consumable:
                color = "Green";
                break;
            case Item.ItemType.Weapon:
            case Item.ItemType.Equipment:
                color = "Blue";
                break;
            default:
                color = "White";
                break;
        }
        var tooltip = "<color=" + color + ">  "+ this.Id+ "  -" + this.Name + "</color>\n\n" + this.Description + "\n<color=yellow>Cost:" + this.Cost + "</color>\n <color=green>Available:"+ this.StackCnt +" </color> ";
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
