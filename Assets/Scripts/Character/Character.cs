using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Character {

    public enum CharacterType
    {
        Walk,
        Fly,
        Swim
    }
    public enum AttackRange
    {
        Range, 
        Close
    }
    public enum DefenceRange
    {
        Range,
        Close
    }
    public enum AttackType
    {
        Strength,
        Magic,
        Poison
    }
    public enum DefenceType
    {
        Strength,
        Magic,
        Poison
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

    public enum Ellements
    {
        None,
        Ice,
        Fire,
        Water,
        Earth,
        Metal
    }

    public enum ClanRanks
    {
        None,
        User,
        Commander,
        Chief
    }

    //todo: make all set private to nost screw db ?? 
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    public bool IsAnimated { get; set; }
    public CharacterType MoveT { get; set; }
    public AttackRange AttackR { get; set; }
    public DefenceRange DefenceR { get; set; }
    public AttackType AttackT { get; set; }
    public DefenceType DefenceT { get; set; }
    public float BasicAttack { get; set; }
    public float BasicDefence { get; set; }
    public float BasicSpeed { get; set; }
    public int BasicHealth { get; set; }
    public SpeedType SpeedT { get; set; }
    public BodyType BodyT { get; set; }
    public CarryType CarryT { get; set; }
    public TerrainIns.TerrainType FavouriteTerrainTypes { get; set; }
    public string DropItems { get; set; }
    public float DropChance { get; set; }
    public bool IsEnabled { get; set; }
    public string Slug { get; set; }

    public Character(int id, string name, string desc, 
        CharacterType moveT, AttackRange attackR, DefenceRange defenceR, AttackType attackT, DefenceType defenceT,
        int basicAttack, int basicDefence, 
        SpeedType speedT, BodyType bodyT, CarryType carryT,
        TerrainIns.TerrainType favouriteTerrainTypes, string dropItems,float dropChance)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = "Somewhere";
        IconId = id;
        IsAnimated = true;
        MoveT = moveT;
        AttackR = attackR;
        DefenceR = defenceR;
        AttackT = attackT;
        DefenceT = defenceT;
        BasicAttack = basicAttack;
        BasicDefence = basicDefence;
        BasicSpeed = (int) speedT;
        BasicHealth = (int)bodyT * 1000;
        SpeedT = speedT;
        BodyT = bodyT;
        CarryT = carryT;
        DropItems = dropItems;
        DropChance = dropChance;
        FavouriteTerrainTypes = favouriteTerrainTypes;
        IsEnabled = true;
        Slug = name.Replace(" ", "_");
    }

    public Character()
    {
        Id = -1;
    }

    public Sprite GetSprite()
    {
        Sprite[] characterSprites = Resources.LoadAll<Sprite>("Characters/" + Name);
        // Get specific sprite
        return characterSprites.Single(s => s.name == "right_3");
    }


    public List<Sprite> GetSprites()
    {
        List<Sprite> MoveSprites = new List<Sprite>();
        // Load all sprites in atlas
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>("Characters/" + Name);
        // Get specific sprite
        MoveSprites.Add(abilityIconsAtlas.Single(s => s.name == "right_3"));
        MoveSprites.Add(abilityIconsAtlas.Single(s => s.name == "left_3"));
        MoveSprites.Add(abilityIconsAtlas.Single(s => s.name == "up_3"));
        MoveSprites.Add(abilityIconsAtlas.Single(s => s.name == "down_3"));
        return MoveSprites;
    }



    public RuntimeAnimatorController GetAnimator()
    {
        // Load Animation Controllers
        string animationPath = "Characters/Animations/";
        return (RuntimeAnimatorController)Resources.Load(animationPath + Name + "Controller");
    }


    public bool Exist(List<Character> characters)
    {
        for (int i = 0; i < characters.Count; i++)
            if (characters[i].Name == this.Name)
                return true;
        return false;
    }
    
}
