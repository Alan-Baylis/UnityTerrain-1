using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer {

    private Consumable _consumable = new Consumable();
    private Weapon _weapon = new Weapon();
    private Equipment _equipment = new Equipment();
    private Substance _substance = new Substance();
    private ItemContainer item;

    public Consumable Consumable
    {
        get { return _consumable; }
        set { _consumable = value; }
    }
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

    public Substance Substance
    {
        get { return _substance; }
        set { _substance = value; }
    }


    public int Id
    {
        get
        {
            if (Consumable != null)
                return Consumable.Id;
            if (Weapon != null)
                return Weapon.Id;
            if (Equipment != null)
                return Equipment.Id;
            if (Substance != null)
                return Substance.Id;
            return -1;
        }
    }

    public string Name 
    {
        get
        {
            if (Consumable != null)
                return Consumable.Name;
            if (Weapon != null)
                return Weapon.Name;
            if (Equipment != null)
                return Equipment.Name;
            if (Substance != null)
                return Substance.Name;
            return "";
        }
    }
    public string Description
    {
        get
        {
            if (Consumable != null)
                return Consumable.Description;
            if (Weapon != null)
                return Weapon.Description;
            if (Equipment != null)
                return Equipment.Description;
            if (Substance != null)
                return Substance.Description;
            return "";
        }
    }

    public string IconPath 
    {
        get
        {
            if (Consumable != null)
                return Consumable.IconPath;
            if (Weapon != null)
                return Weapon.IconPath;
            if (Equipment != null)
                return Equipment.IconPath;
            if (Substance != null)
                return Substance.IconPath;
            return "";
        }
    }

    public int IconId
    {
        get
        {
            if (Consumable != null)
                return Consumable.IconId;
            if (Weapon != null)
                return Weapon.IconId;
            if (Equipment != null)
                return Equipment.IconId;
            if (Substance != null)
                return Substance.IconId;
            return -1;
        }
    }
    public int Cost
    {
        get
        {
            if (Consumable != null)
                return Consumable.Cost;
            if (Weapon != null)
                return Weapon.Cost;
            if (Equipment != null)
                return Equipment.Cost;
            if (Substance != null)
                return Substance.Cost;
            return 0;
        }
    }
    public int MaxStackCnt
    {
        get
        {
            if (Consumable != null)
                return Consumable.MaxStackCnt;
            if (Weapon != null)
                return Weapon.MaxStackCnt;
            if (Equipment != null)
                return Equipment.MaxStackCnt;
            if (Substance != null)
                return Substance.MaxStackCnt;
            return 0;
        }
    }

    public int StackCnt
    {
        get
        {
            if (Consumable != null)
                return Consumable.StackCnt;
            if (Weapon != null)
                return Weapon.StackCnt;
            if (Equipment != null)
                return Equipment.StackCnt;
            if (Substance != null)
                return Substance.StackCnt;
            return 0;
        }
    }
    public Item.ItemType Type
    {
        get
        {
            if (Consumable != null)
                return Consumable.Type;
            if (Weapon != null)
                return Weapon.Type;
            if (Equipment != null)
                return Equipment.Type;
            if (Substance != null)
                return Substance.Type;
            return 0;
        }
    }
    public Item.ItemRarity Rarity
    {
        get
        {
            if (Consumable != null)
                return Consumable.Rarity;
            if (Weapon != null)
                return Weapon.Rarity;
            if (Equipment != null)
                return Equipment.Rarity;
            if (Substance != null)
                return Substance.Rarity;
            return 0;
        }
    }
    //public string Slug { get; set; }
    public DateTime ExpirationTime
    {
        get
        {
            if (Consumable != null)
                return Consumable.ExpirationTime;
            if (Weapon != null)
                return Weapon.ExpirationTime;
            if (Equipment != null)
                return Equipment.ExpirationTime;
            if (Substance != null)
                return Substance.ExpirationTime;
            return DateTime.MinValue;
        }
    }


    public int Weight
    {
        get
        {
            if (Consumable != null)
                return Consumable.Weight;
            if (Weapon != null)
                return Weapon.Weight;
            if (Equipment != null)
                return Equipment.Weight;
            if (Substance != null)
                return Substance.Weight;
            return 0;
        }
    }

    public bool IsEnable
    {
        get
        {
            if (Consumable != null)
                return Consumable.IsEnable;
            if (Weapon != null)
                return Weapon.IsEnable;
            if (Equipment != null)
                return Equipment.IsEnable;
            if (Substance != null)
                return Substance.IsEnable;
            return false;
        }
    }

    public int[] Values
    {
        get
        {
            if (Consumable != null)
                return new int[3]
                {
                    Consumable.Health, Consumable.Mana, Consumable.Energy
                };
            if (Weapon != null)
                return new int[8] {
                    Weapon.AttackSpeed, Weapon.DefenceSpeed,
                    Weapon.AbilityAttack, Weapon.AbilityDefence,
                    Weapon.MagicAttack, Weapon.MagicDefence,
                    Weapon.PoisonAttack, Weapon.PoisonDefence
                };
            if (Equipment != null)
                return new int[12]
                {
                    (int)Equipment.PlaceHolder
                    ,Equipment.Agility 
                    ,Equipment.Bravery 
                    ,Equipment.Carry
                    ,Equipment.CarryCnt
                    ,Equipment.Confidence
                    ,Equipment.Intellect 
                    ,Equipment.Krafting 
                    ,Equipment.Researching 
                    ,Equipment.Speed
                    ,Equipment.Stemina
                    ,Equipment.Strength 
                };
            if (Equipment != null)
                return null;
            return null;
        }
    }

    public Equipment.PlaceType PlaceHolder
    {
        get
        {
            return Equipment.PlaceHolder;
        }
    }
    public ItemContainer(
        int id,string name,string description,
        string iconPath,int iconId,
        int cost, int weight, 
        int maxStackCnt,int stackCnt,
        Item.ItemType type,Item.ItemRarity rarity,
        DateTime expirationTime,
        int[] values = null )
    {

        switch (type)
        {
            case Item.ItemType.Consumable:
                Consumable = new Consumable(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, expirationTime, values);
                Equipment = null;
                Weapon = null;
                Substance = null;
                break;
            case Item.ItemType.Weapon:
                Consumable = null;
                Equipment = null;
                Weapon = new Weapon(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, expirationTime, values);
                Substance = null;
                break;
            case Item.ItemType.Equipment:
                Consumable = null;
                Equipment = new Equipment(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type,  rarity, expirationTime, values);
                Weapon = null;
                Substance = null;
                break;
            case Item.ItemType.Substance:
                Consumable = null;
                Equipment = null;
                Weapon = null;
                Substance = new Substance(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, expirationTime, values);
                break;
        }
    }

    public ItemContainer(ItemContainer item)
    {
        new ItemContainer(
            item.Id, item.Name, item.Description,
            item.IconPath, item.IconId,
            cost: item.Cost, weight: item.Weight,
            maxStackCnt: item.MaxStackCnt, stackCnt: item.StackCnt,
            type: item.Type, rarity: item.Rarity,
            expirationTime: item.ExpirationTime,
            values: item.Values);
    }


    public ItemContainer()
    {
        Consumable = null;
        Weapon = null;
        Equipment = null;
        Substance = null;
    }

    public string GetTooltip()
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                return Consumable.GetTooltip();
            case Item.ItemType.Weapon:
                return Weapon.GetTooltip();
            case Item.ItemType.Equipment:
                return Equipment.GetTooltip();
            case Item.ItemType.Substance:
                return Substance.GetTooltip();
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
            case Item.ItemType.Weapon:
                return Weapon.GetSprite();
            case Item.ItemType.Equipment:
                return Equipment.GetSprite();
            case Item.ItemType.Substance:
                return Substance.GetSprite();
            default:
                return new Sprite();
        }
    }

    public bool Exist(List<ItemContainer> items)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].Name == this.Name)
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
            case Item.ItemType.Weapon:
                Weapon.StackCnt = value;
                break;
            case Item.ItemType.Equipment:
                Equipment.StackCnt = value;
                break;
            case Item.ItemType.Substance:
                Substance.StackCnt = value;
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
