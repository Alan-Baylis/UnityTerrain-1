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
    //public string Description { get; set; }
    //public string IconPath { get; set; }
    //public int IconId { get; set; }
    //public int Cost { get; set; }
    //public int MaxStackCnt { get; set; }
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
        set
        {
            if (Consumable != null)
                Consumable.StackCnt = value;
            if (Equipment != null)
                Equipment.StackCnt = value;
            if (Weapon != null)
                Weapon.StackCnt = value;
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
    //public Item.ItemRarity Rarity { get; set; }
    //public string Slug { get; set; }

    public ItemContainer(int id,string name,string description,
                        //string iconPath,int iconId,
                        int cost,int maxStackCnt,int stackCnt,
                        Item.ItemType type,Item.ItemRarity rarity,
                        Consumable.ConsumableType conType = (Consumable.ConsumableType)(-1),
                        int[] values = null )
    {

        switch (type)
        {
            case Item.ItemType.Consumable:
                Consumable = new Consumable(id, name, description, cost, maxStackCnt, stackCnt, type, rarity, conType, values);
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
        Consumable = new Consumable();
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

}
