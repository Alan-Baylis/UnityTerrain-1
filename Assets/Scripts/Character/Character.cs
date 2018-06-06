using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character {

    public enum CharacterType
    {
        Walk,
        Fly,
        Swim
    }


    public enum AttackType
    {
        Range, 
        Close
    }

    public enum DefenceType
    {
        Range,
        Close
    }

    public enum SpeedType
    {
        Slow=3,
        Fast=5
    }//int Speed = (int)Speed.Slow;

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    //public Sprite Icon { get; set; }
    public CharacterType Type { get; set; }
    public AttackType Attack { get; set; }
    public DefenceType Defence { get; set; }
    public SpeedType Speed { get; set; }
    public string Slug { get; set; }

    public Character(int id, string name, string desc, CharacterType type, AttackType attack, DefenceType defence, SpeedType speed)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = "Somewhere";
        IconId = id;
        Type = type;
        Attack = attack;
        Defence = defence;
        Speed = speed;
        Slug = name.Replace(" ", "_");
    }

    public Character()
    {
        Id = -1;
    }

    public Sprite GetSprite()
    {
        Sprite[] spriteList = Resources.LoadAll<Sprite>(IconPath);
        return spriteList[IconId];
    }


    //Abstarct
    //public abstract void Usage();
}
