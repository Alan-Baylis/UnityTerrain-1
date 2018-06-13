using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer {

    private Weapon _weapon = new Weapon();
    private Equipment _equipment = new Equipment();
    private Consumable _consumable = new Consumable();

    public Weapon Weapon
    {
        get { return _weapon; }
        set { _weapon = value; }
    }

    public Equipment Equipment
    {
        get { return _equipment; }
        set { _equipment = value; }
    }

    public Consumable Consumable
    {
        get { return _consumable; }
        set { _consumable = value; }
    }


    public int Id
    {
        get
        {
            if (Consumable != null)
                return Consumable.Id;
            if (Equipment != null)
                return Equipment.Id;
            if (Weapon != null)
                return Weapon.Id;
            return -1;
        }
    }

    public string Name 
    {
        get
        {
            if (Consumable != null)
                return Consumable.Name;
            if (Equipment != null)
                return Equipment.Name;
            if (Weapon != null)
                return Weapon.Name;
            return "";
        }
    }
    public string Description
    {
        get
        {
            if (Consumable != null)
                return Consumable.Description;
            if (Equipment != null)
                return Equipment.Description;
            if (Weapon != null)
                return Weapon.Description;
            return "";
        }
    }


    //public string IconPath { get; set; }
    //public int IconId { get; set; }
    public int Cost
    {
        get
        {
            if (Consumable != null)
                return Consumable.Cost;
            if (Equipment != null)
                return Equipment.Cost;
            if (Weapon != null)
                return Weapon.Cost;
            return 0;
        }
    }
    public int MaxStackCnt
    {
        get
        {
            if (Consumable != null)
                return Consumable.MaxStackCnt;
            if (Equipment != null)
                return Equipment.MaxStackCnt;
            if (Weapon != null)
                return Weapon.MaxStackCnt;
            return 0;
        }
    }



    public int StackCnt
    {
        get
        {
            if (Consumable != null)
                return Consumable.StackCnt;
            if (Equipment != null)
                return Equipment.StackCnt;
            if (Weapon != null)
                return Weapon.StackCnt;
            return 0;
        }
    }
    public Item.ItemType Type
    {
        get
        {
            if (Consumable != null)
                return Consumable.Type;
            if (Equipment != null)
                return Equipment.Type;
            if (Weapon != null)
                return Weapon.Type;
            return 0;
        }
    }
    public Item.ItemRarity Rarity
    {
        get
        {
            if (Consumable != null)
                return Consumable.Rarity;
            if (Equipment != null)
                return Equipment.Rarity;
            if (Weapon != null)
                return Weapon.Rarity;
            return 0;
        }
    }
    //public string Slug { get; set; }

    public int[] Values
    {
        get
        {
            if (Consumable != null)
                return new int[3] { Consumable.Health, Consumable.Mana, Consumable.Vitality};
            if (Equipment != null)
                return null;
            if (Weapon != null)
                return null;
            return null;
        }
    }


    public ItemContainer(int id,string name,string description,
                        //string iconPath,int iconId,
                        int cost,int maxStackCnt,int stackCnt,
                        Item.ItemType type,Item.ItemRarity rarity,
                        int[] values = null )
    {

        switch (type)
        {
            case Item.ItemType.Consumable:
                Consumable = new Consumable(id, name, description, cost, maxStackCnt, stackCnt, type, rarity, values);
                Equipment = null;
                Weapon = null;
                break;
            case Item.ItemType.Equipment:
                Consumable = null;
                Equipment = new Equipment(id, name, description, cost, maxStackCnt, stackCnt, type, rarity);
                Weapon = null;
                break;
            case Item.ItemType.Weapon:
                Consumable = null;
                Equipment = null;
                Weapon = new Weapon(id, name, description, cost, maxStackCnt, stackCnt, type, rarity);
                break;
        }
    }

    public ItemContainer()
    {
        Consumable = null;
        Equipment = null;
        Weapon = null;
    }


    public string GetTooltip()
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                return Consumable.GetTooltip();
            case Item.ItemType.Equipment:
                return Equipment.GetTooltip();
            case Item.ItemType.Weapon:
                return Weapon.GetTooltip();
            default:
                return "";
        }
    }

    public Sprite GetSprite()
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                return Consumable.GetSprite();
            case Item.ItemType.Equipment:
                return Equipment.GetSprite();
            case Item.ItemType.Weapon:
                return Weapon.GetSprite();
            default:
                return new Sprite();
        }
    }

    public bool Exist(List<ItemContainer> Items)
    {
        //Debug.Log("inside Exist " + Items.Count + " " + this.Id);
        //Debug.Log("inside Exist loop" + item.Id);
        for (int i = 0; i < Items.Count; i++)
            if (Items[i].Id == this.Id)
                return true;
        return false;
    }


    public void setStackCnt(int value)
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                Consumable.StackCnt = value;
                break;
            case Item.ItemType.Equipment:
                Equipment.StackCnt = value;
                break;
            case Item.ItemType.Weapon:
                Weapon.StackCnt = value;
                break;
        }
    }


    internal void Print()
    {
        if (Id == -1)
        {
            Debug.Log("Id:" + Id);
            return;
        }
        Sprite Sprite = this.GetSprite();
        Debug.Log("Id:" + Id + " Name:" + Name + " Sprite:" + Sprite.name + " Type:" + Type + "StackCnt:" + StackCnt + " Rarity:" + Rarity );
    }
}
