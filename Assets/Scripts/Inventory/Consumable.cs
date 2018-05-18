using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item {
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Vitality { get; set; }


    public Consumable() : base()
    {
    }

    public Consumable(int id, string name, string desc, int cost, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, int[] values =null)
        : base(id, name, desc, cost, maxStackCnt, stackCnt, type, rarity)
    {
        if (values == null)
        {
            Health = Mana = Vitality = 0;
            return;
        }
        Health = values[0];
        Mana = values[1];
        Vitality = values[2];
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
