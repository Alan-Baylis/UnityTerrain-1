using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class CharacterSetting {
    private CharacterSetting characterSetting;

    public int Id { get; set; }
    public int CharacterId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    //public Sprite Icon { get; set; }
    public int Level { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public bool Updated { get; set; }
    public int Rank { get; set; }
    public int ClanId { get; set; }
    public int Live { get; set; }
    public int Coin { get; set; }
    public int Gem { get; set; }
    public int Experience { get; set; }
    public int HandsCnt { get; set; }
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
    public int CarryCnt { get; set; }
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
         string description = null, string iconPath = null, int iconId = 0, 
        int level = 0, float longitude = 0, float latitude = 0,bool updated = true,
        int rank = 0, int clanId = 0, int live = 0, int coin = 0, int gem = 0,
        int experience = 0, int handsCnt = 2, int maxHealth = 0,  int health = 0, int maxMana = 0, int mana = 0, int maxEnergy = 0,
        int energy = 0, int attackSpeed = 0, int defenceSpeed = 0, int abilityAttack = 0, int abilityDefence = 0,
        int magicAttack = 0, int magicDefence = 0, int poisonAttack = 0, int poisonDefence = 0, float carry = 0, int carryCnt = 0,
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
        Rank = rank;
        ClanId = clanId;
        Live = live;
        Longitude = longitude;
        Latitude = latitude;
        Updated = updated;
        Coin = coin;
        Gem = gem;
        Experience = experience;
        HandsCnt = handsCnt;
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
        CarryCnt = carryCnt;
        Confidence = confidence;
        Intellect = intellect;
        Krafting = krafting;
        Researching = researching;
        Speed = speed;
        Stemina = stemina;
        Strength = strength;
        if (equipments ==null)
        {
            Equipments = new List<int>();
            for (int i = 0; i < 10; i++)
                Equipments.Add(-1);
        }
        else
            Equipments = equipments;
    }
    
    public CharacterSetting()
    {
        Id = -1;
    }

    public CharacterSetting(CharacterSetting characterSetting)
    {
        Id = characterSetting.Id;
        CharacterId = characterSetting.CharacterId;
        Name = characterSetting.Name;
        Description = characterSetting.Description;
        IconPath = characterSetting.IconPath;
        IconId = characterSetting.IconId;
        Level = characterSetting.Level;
        Rank = characterSetting.Rank;
        ClanId = characterSetting.ClanId;
        Live = characterSetting.Live;
        Longitude = characterSetting.Longitude;
        Latitude = characterSetting.Latitude;
        Updated = characterSetting.Updated;
        Coin = characterSetting.Coin;
        Gem = characterSetting.Gem;
        Experience = characterSetting.Experience;
        HandsCnt = characterSetting.HandsCnt;
        MaxHealth = characterSetting.MaxHealth;
        Health = characterSetting.Health;
        MaxMana = characterSetting.MaxMana;
        Mana = characterSetting.Mana;
        MaxEnergy = characterSetting.MaxEnergy;
        Energy = characterSetting.Energy;
        AttackSpeed = characterSetting.AttackSpeed;
        DefenceSpeed = characterSetting.DefenceSpeed;
        AbilityAttack = characterSetting.AbilityAttack;
        AbilityDefence = characterSetting.AbilityDefence;
        MagicAttack = characterSetting.MagicAttack;
        MagicDefence = characterSetting.MagicDefence;
        PoisonAttack = characterSetting.PoisonAttack;
        PoisonDefence = characterSetting.PoisonDefence;
        Agility = characterSetting.Agility;
        Bravery = characterSetting.Bravery;
        Carry = characterSetting.Carry;
        CarryCnt = characterSetting.CarryCnt;
        Confidence = characterSetting.Confidence;
        Intellect = characterSetting.Intellect;
        Krafting = characterSetting.Krafting;
        Researching = characterSetting.Researching;
        Speed = characterSetting.Speed;
        Stemina = characterSetting.Stemina;
        Strength = characterSetting.Strength;
        Equipments = characterSetting.Equipments;
    }


    //public bool AddEquipment(ItemContainer item)
    //{
    //    if (item.Equipment == null)
    //        return false;
    //    Equipments[(int)item.Equipment.PlaceHolder] = item.Id;
    //    return true;
    //}

}
