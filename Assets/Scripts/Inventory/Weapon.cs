using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : Item
{
    public enum Hands
    {
        OneHand,  
        TwoHands,
        ThreeHands,
        FourHands
    }

    public Hands CarryType { get; set; }
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

    public Weapon(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, int durationDays, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, durationDays, expirationTime)
    {
        CarryType = (Hands)values[0];
        AttackSpeed = values[1];
        DefenceSpeed = values[2];
        AbilityAttack  = values[3];
        AbilityDefence = values[4];
        MagicAttack = values[5];
        MagicDefence = values[6];
        PoisonAttack = values[7];
        PoisonDefence = values[8];
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
