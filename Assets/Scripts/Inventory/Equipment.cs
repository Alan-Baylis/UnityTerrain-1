using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item {

    public int Intellect { get; set; }
    public int Agility { get; set; }
    public int Stemina { get; set; }
    public int Strength { get; set; }
    public int Power { get; set; }
    public int Speed { get; set; }

    public Equipment() : base()
    {
    }

    public Equipment(int id, string name, string desc, int cost, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity)
        : base(id, name, desc, cost, maxStackCnt, stackCnt, type, rarity)
    {
        Intellect = 0;
        Agility = 0;
        Stemina = 0;
        Strength = 0;
        Power = 0;
        Speed = 0;
    }

    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Equipment";
        return tooltip;
    }
}
