using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipment : Item {

    public enum PlaceType
    {
        Head,   // Hat, Crown
        Face,   // Mask, Helmet
        Neck,
        Chest,
        Arms,
        Hands,
        Waist,
        Tail,
        Legs,
        Feet,
        Shadow
    }

    public PlaceType PlaceHolder { get; set; }
    public int Intellect { get; set; }
    public int Agility { get; set; }
    public int Strength { get; set; }
    public int Stemina { get; set; }
    public int Speed { get; set; }
    public int Krafting { get; set; }
    public int Bravery { get; set; }
    public int Confidence { get; set; }
    

    public Equipment() : base()
    {
    }

    public Equipment(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, expirationTime)
    {
        PlaceHolder = (PlaceType) values[0];
        Intellect = values[1];
        Agility = values[2];
        Strength = values[3];
        Stemina = values[4];
        Speed = values[5];
        Krafting = values[6];
        Bravery = 0;
        Confidence = 0;
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
