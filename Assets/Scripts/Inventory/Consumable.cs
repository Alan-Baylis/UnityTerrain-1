using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item {
    public enum ConsumableType
    {
        H,
        M,
        HM,
        MH
    }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Vitality { get; set; }


    public Consumable() : base()
    {
    }

    public Consumable(int id, string name, string desc, int cost, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, ConsumableType conType, int[] values)
        : base(id, name, desc, cost, maxStackCnt, stackCnt, type, rarity)
    {
        Health = 0;
        Mana = 0;
        Vitality = 0;
        switch (conType)
        {
            case ConsumableType.H:
                Health = values[0];
                break;
            case ConsumableType.M:
                Mana = values[0];
                break;
            case ConsumableType.HM:
                Health = values[0];
                Mana = values[1];
                break;
            case ConsumableType.MH:
                Mana = values[0];
                Health = values[1];
                break;
        }
    }
    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Consumable";
        return tooltip;
    }
}
