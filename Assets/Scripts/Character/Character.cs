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
        Slug = 1,
        Slow = 2,
        Regular = 3,
        Fast = 4,
        Rapid = 5
    }

    public enum BodyType
    {
        Tiny = 1,
        Slim,
        Regular,
        Muscle,
        Tank
    }

    public enum CarryType
    {
        Slight = 1,
        Light,
        Normal,
        Heavy,
        Hefty
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    //public Sprite Icon { get; set; }
    public int Cost { get; set; }
    public int Weight { get; set; }
    public CharacterType MoveType { get; set; }
    public AttackType Attack { get; set; }
    public DefenceType Defence { get; set; }
    public SpeedType Speed { get; set; }
    public BodyType Body { get; set; }
    public CarryType Carry { get; set; }
    public string Slug { get; set; }

    public Character(int id, string name, string desc, CharacterType moveType, AttackType attack, DefenceType defence, SpeedType speed, BodyType body, CarryType carry)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = "Somewhere";
        IconId = id;
        MoveType = moveType;
        Attack = attack;
        Defence = defence;
        Speed = speed;
        Body = body;
        Carry = carry;
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
    
}
