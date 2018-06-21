using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class CharacterSetting {
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    //public Sprite Icon { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int MaxMana { get; set; }
    public int Mana { get; set; }
    public int MaxEnergy { get; set; }
    public int Energy { get; set; }
    public int AttackSpeed { get; set; }
    public int DefenceSpeed { get; set; }
    public int AbilityAttack { get; set; }
    public int AbilityDefence { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefence { get; set; }
    public int PoisonAttack { get; set; }
    public int PoisonDefence { get; set; }
    public float Carry { get; set; }
    public float Speed { get; set; }
    public float Intellect { get; set; }
    public float Agility { get; set; }
    public float Strength { get; set; }
    public float Stemina { get; set; }
    public float Krafting { get; set; }
    public float Researching { get; set; }
    public float Bravery { get; set; }
    public float Confidence { get; set; }
    public List<int> Equipments { get; set; }
    //Todo: add the hashcode

    public CharacterSetting(int id = 0, int characterId = 0, string name = null,
         string description = null, string iconPath = null, int iconId = 0, int level = 0,
        int experience = 0, int maxHealth = 0, int health = 0, int maxMana = 0, int mana = 0, int maxEnergy = 0,
        int energy = 0, int attackSpeed = 0, int defenceSpeed = 0, int abilityAttack = 0, int abilityDefence = 0,
        int magicAttack = 0, int magicDefence = 0, int poisonAttack = 0, int poisonDefence = 0, float carry = 0,
        float speed = 0, float intellect = 0, float agility = 0, float strength = 0, float stemina = 0,
        float krafting = 0, float researching = 0, float bravery = 0, float confidence = 0, List<int> equipments = null)
    {
        Id = id;
        CharacterId = characterId;
        Name = name;
        Description = description;
        IconPath = "Somewhere";
        IconId = id;
        Level = level;
        Experience = experience;
        MaxHealth = maxHealth;
        Health = health;
        MaxMana = maxMana;
        Mana = mana;
        MaxEnergy = maxEnergy;
        Energy = energy;
        AttackSpeed = attackSpeed;
        DefenceSpeed = defenceSpeed;
        AbilityAttack = abilityAttack;
        AbilityDefence = abilityDefence;
        MagicAttack = magicAttack;
        MagicDefence = magicDefence;
        PoisonAttack = poisonAttack;
        PoisonDefence = poisonDefence;
        Agility = agility;
        Bravery = bravery;
        Carry = carry;
        Confidence = confidence;
        Intellect = intellect;
        Krafting = krafting;
        Researching = researching;
        Speed = speed;
        Stemina = stemina;
        Strength = strength;
        if (equipments ==null)
        {
            equipments = new List<int>();
            for (int i = 0; i < 10; i++)
                equipments.Add(-1);
        }
        else
            Equipments = equipments;
    }
    
    public CharacterSetting()
    {
        Id = -1;
    }



    public bool AddEquipment(int index,int id)
    {
        Equipments[index] = id;
        return true;
    }
    public bool AddEquipment(ItemContainer item)
    {
        if (item.Equipment == null)
            return false;
        Equipments[(int)item.Equipment.PlaceHolder] = item.Id;
        return true;
    }

}
