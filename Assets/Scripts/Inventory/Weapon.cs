using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{

    public int Attack { get; set; }
    public int AttackSpeed { get; set; }
    public int Defence { get; set; }
    public int DefenceSpeed { get; set; }

    public Weapon() : base()
    {
    }

    public Weapon(int id, string name, string desc, int cost, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity)
        : base(id, name, desc, cost, maxStackCnt, stackCnt, type, rarity)
    {
        Attack = 0;
        AttackSpeed = 0;
        Defence = 0;
        DefenceSpeed = 0;
    }


    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Weapon";
        return tooltip;
    }

}
