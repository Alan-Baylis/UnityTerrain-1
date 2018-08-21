using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MonsterIns
{
    private Character _monsterCharacter;
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int CharacterSettingId { get; set; }
    public int Level { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public float AttackSpeed { get; set; }
    public float DefenceSpeed { get; set; }
    public float AbilityAttack { get; set; }
    public float AbilityDefence { get; set; }
    public float MagicAttack { get; set; }
    public float MagicDefence { get; set; }
    public float PoisonAttack { get; set; }
    public float PoisonDefence { get; set; }
    public float Speed { get; set; }
    public Character.Ellements Ellement { get; set; }

    public MonsterIns(int id, int characterId, int characterSettingId, int level, int maxHealth, int health, float attackSpeed, float defenceSpeed, float abilityAttack, float abilityDefence, float magicAttack, float magicDefence, float poisonAttack, float poisonDefence, float speed, Character.Ellements ellement)
    {
        Id = id;
        CharacterId = characterId;
        CharacterSettingId = characterSettingId;
        Level = level;
        MaxHealth = maxHealth;
        Health = health;
        AttackSpeed = attackSpeed;
        DefenceSpeed = defenceSpeed;
        AbilityAttack = abilityAttack;
        AbilityDefence = abilityDefence;
        MagicAttack = magicAttack;
        MagicDefence = magicDefence;
        PoisonAttack = poisonAttack;
        PoisonDefence = poisonDefence;
        Speed = speed;
        Ellement = ellement;
    }

    public MonsterIns(Character monsterCharacter, int level)
    {
        this._monsterCharacter = monsterCharacter;
        if (level<3)
            level += 3;
        Id = 0;
        CharacterId = monsterCharacter.Id;
        CharacterSettingId = -1;
        Level = UnityEngine.Random.Range(level-3, level+1);
        MaxHealth = monsterCharacter.BasicHealth* Level;
        Health = MaxHealth;
        AttackSpeed = monsterCharacter.BasicSpeed;
        DefenceSpeed = monsterCharacter.BasicSpeed;
        AbilityAttack = monsterCharacter.AttackT == Character.AttackType.Strength ? monsterCharacter.BasicAttack * Level : 0;
        AbilityDefence = monsterCharacter.AttackT == Character.AttackType.Strength ? monsterCharacter.BasicDefence * Level : 0;
        MagicAttack = monsterCharacter.AttackT == Character.AttackType.Magic ? monsterCharacter.BasicAttack * Level : 0;
        MagicDefence = monsterCharacter.AttackT == Character.AttackType.Magic ? monsterCharacter.BasicDefence * Level : 0;
        PoisonAttack = monsterCharacter.AttackT == Character.AttackType.Poison ? monsterCharacter.BasicAttack * Level : 0;
        PoisonDefence = monsterCharacter.AttackT == Character.AttackType.Poison ? monsterCharacter.BasicDefence * Level : 0;
        Speed = monsterCharacter.BasicSpeed;
        Ellement = Character.Ellements.None;
        Level = level;
    }

    public Character GetCharacter()
    {
        return _monsterCharacter;
    }

    internal void Print()
    {
        Debug.Log(
            " Monster:" + Id +
            " CharacterSettingId:" + CharacterSettingId +
            " CharacterId:" + CharacterId 
            );
    }

}
