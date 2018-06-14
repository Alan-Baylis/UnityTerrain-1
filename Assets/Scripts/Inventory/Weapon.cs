using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : Item
{
    public int AttackSpeed { get; set; }
    public int DefenceSpeed { get; set; }
    public int AbilityAttack { get; set; }
    public int AbilityDefence { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefence { get; set; }
    public int PoisonAttack { get; set; }
    public int PoisonDefence { get; set; }

    public Weapon() : base()
    {
    }

    public Weapon(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, expirationTime)
    {
        AttackSpeed = values[0];
        DefenceSpeed = values[1];
        AbilityAttack  = values[2];
        AbilityDefence = values[3];
        MagicAttack = values[4];
        MagicDefence = values[5];
        PoisonAttack = values[6];
        PoisonDefence = values[7];
    }

    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Weapon";
        return tooltip;
    }

}
