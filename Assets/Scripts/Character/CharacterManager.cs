using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour {


    private static CharacterManager _characterManager;

    private CharacterDatabase _characterDatabase;
    private ItemDatabase _itemDatabase;

    public Character Character;
    public CharacterSetting CharacterSetting;
    public CharacterMixture CharacterMixture;
    public List<ItemContainer> CharacterInventory = new List<ItemContainer>();
    

    public int PlayerId=0;

    private int _basicHealth = 1000;
    private int _basicSpeed = 3;
    private int _basicCarry = 100;

    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();
        _characterDatabase = CharacterDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        //print("_availableCharacters.Count =" + _availableCharacters.Characters.Count);
        CharacterSetting = _characterDatabase.FindCharacterSetting(PlayerId);
        print("CharacterSetting "+ CharacterSetting.Id);
        Character = _characterDatabase.FindCharacter(CharacterSetting.CharacterId);
        print("Character " + Character.Id);
        CharacterMixture = _characterDatabase.FindCharacterMixture(PlayerId);
        print("CharacterMixture " + CharacterMixture.Id);
        CharacterInventory = _characterDatabase.FindCharacterInventory(PlayerId);
        print("CharacterInventory " + CharacterInventory.Count);
    }




    //middle man to CharacterDatabase
    public void SaveCharacterSetting()
    {
        _characterDatabase.SaveCharacterSetting(CharacterSetting);
    }
    public void SaveCharacterMixture(ItemContainer item, DateTime time)
    {
        _characterDatabase.SaveCharacterMixture(item, time);
    }
    public void SaveCharacterInventory(List<ItemContainer> inv)
    {
        //TODO: May need to clrat list
        CharacterInventory.Clear();
        for (int i = 0; i < CharacterInventory.Count; i++)
        {
            if (inv[i].Id == -1)
                CharacterInventory.Add(new ItemContainer());
            else
                CharacterInventory.Add(
                    new ItemContainer(
                        inv[i].Id, inv[i].Name, inv[i].Description,
                        inv[i].IconPath, inv[i].IconId,
                        inv[i].Cost, inv[i].Weight,
                        inv[i].MaxStackCnt, inv[i].StackCnt,
                        inv[i].Type, inv[i].Rarity,
                        inv[i].DurationDays, inv[i].ExpirationTime,
                        inv[i].Values)
                );
        }
        //todo: make it async with db
        _characterDatabase.SaveCharacterInventory(CharacterInventory);
    }
    internal void SaveCharacterEquipments(List<ItemContainer> equipments)
    {
        CharacterSetting.Equipments = equipments.ToList();
        //Todo: or this one = > _characterSetting.Equipments = new List<Int32>(equipments);
        CharacterSetting.Updated = true;
        SaveCharacterSetting();
    }





    //Instance
    public static CharacterManager Instance()
    {
        if (!_characterManager)
        {
            _characterManager = FindObjectOfType(typeof(CharacterManager)) as CharacterManager;
            if (!_characterManager)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _characterManager;
    }


    private void CalculateCharacterSetting()
    {
        CharacterSetting.MaxHealth = ((int)Character.Body / 100 + 1) * _basicHealth;
        CharacterSetting.Health = CharacterSetting.MaxHealth;
        CharacterSetting.MaxMana = CharacterSetting.MaxHealth; //Todo: calculate basic Mana
        CharacterSetting.Mana = CharacterSetting.MaxMana;
        CharacterSetting.MaxEnergy = CharacterSetting.MaxHealth;//Todo: calculate basic Energy
        CharacterSetting.Energy = CharacterSetting.MaxEnergy;
        CharacterSetting.AttackSpeed = (int)CharacterSetting.Speed;//Todo: calculate basic AttackSpeed
        CharacterSetting.DefenceSpeed = (int)CharacterSetting.Speed;//Todo: calculate basic DefenceSpeed
        CharacterSetting.Carry = _basicCarry + (int)Character.Carry * 5;
        CharacterSetting.CarryCnt = _basicCarry / 20 + CharacterSetting.Level / 5;
        CharacterSetting.Speed = ((int)Character.Speed / 100 + 1) * _basicSpeed;
        //Equipment setting
        if (CharacterSetting.Equipments != null)
            foreach (var item in CharacterSetting.Equipments)
            {
                if (item.Id == -1)
                    continue;
                CharacterSettingUseItem(item, 0,false);
            }
        SaveCharacterSetting();
    }

    public void AddCharacterSetting(string field, float value)
    {
        switch (field)
        {
            case "Agility":
                CharacterSetting.Agility += value;
                break;
            case "Health":
                CharacterSetting.Health -= (int)value;
                CharacterSetting.Coin += 1;
                break;
            case "Energy":
                CharacterSetting.Energy += (int)value;
                break;
        }
        CharacterSetting.Updated = true;
        SaveCharacterSetting();
    }

    public void CharacterSettingUseItem(ItemContainer item, int energy,bool save)
    {
        if (item == null)
            return;
        switch (item.Type)
        {
            case Item.ItemType.Consumable:
                CharacterSetting.Health += item.Consumable.Health;
                CharacterSetting.Mana += item.Consumable.Mana;
                CharacterSetting.Energy += item.Consumable.Energy;
                break;
            case Item.ItemType.Equipment:
                CharacterSetting.Agility += item.Equipment.Agility;
                CharacterSetting.Bravery += item.Equipment.Bravery;
                CharacterSetting.Carry += item.Equipment.Carry;
                CharacterSetting.CarryCnt += item.Equipment.CarryCnt;
                CharacterSetting.Confidence += item.Equipment.Confidence;
                CharacterSetting.Intellect += item.Equipment.Intellect;
                CharacterSetting.Krafting += item.Equipment.Krafting;
                CharacterSetting.Researching += item.Equipment.Researching;
                CharacterSetting.Speed += item.Equipment.Speed;
                CharacterSetting.Stemina += item.Equipment.Stemina;
                CharacterSetting.Strength += item.Equipment.Strength;
                break;
            case Item.ItemType.Weapon:
                CharacterSetting.AttackSpeed += item.Weapon.AttackSpeed;
                CharacterSetting.DefenceSpeed += item.Weapon.DefenceSpeed;
                CharacterSetting.AbilityAttack += item.Weapon.AbilityAttack;
                CharacterSetting.AbilityDefence += item.Weapon.AbilityDefence;
                CharacterSetting.MagicAttack += item.Weapon.MagicAttack;
                CharacterSetting.MagicDefence += item.Weapon.MagicDefence;
                CharacterSetting.PoisonAttack += item.Weapon.PoisonAttack;
                CharacterSetting.PoisonDefence += item.Weapon.PoisonDefence;
                break;
            case Item.ItemType.Tool:
                return;
        }
        CharacterSetting.Energy -= energy;
        CharacterSetting.Updated = true;
        if (save)
            SaveCharacterSetting();
    }

    internal void CharacterSettingUnuseItem(ItemContainer item, int energy, bool save)
    {
        if (item == null)
            return;
        switch (item.Type)
        {
            case Item.ItemType.Consumable:
                return;
            case Item.ItemType.Equipment:
                CharacterSetting.Agility -= item.Equipment.Agility;
                CharacterSetting.Bravery -= item.Equipment.Bravery;
                CharacterSetting.Carry -= item.Equipment.Carry;
                CharacterSetting.CarryCnt -= item.Equipment.CarryCnt;
                CharacterSetting.Confidence -= item.Equipment.Confidence;
                CharacterSetting.Intellect -= item.Equipment.Intellect;
                CharacterSetting.Krafting -= item.Equipment.Krafting;
                CharacterSetting.Researching -= item.Equipment.Researching;
                CharacterSetting.Speed -= item.Equipment.Speed;
                CharacterSetting.Stemina -= item.Equipment.Stemina;
                CharacterSetting.Strength -= item.Equipment.Strength;
                break;
            case Item.ItemType.Weapon:
                CharacterSetting.AttackSpeed -= item.Weapon.AttackSpeed;
                CharacterSetting.DefenceSpeed -= item.Weapon.DefenceSpeed;
                CharacterSetting.AbilityAttack -= item.Weapon.AbilityAttack;
                CharacterSetting.AbilityDefence -= item.Weapon.AbilityDefence;
                CharacterSetting.MagicAttack -= item.Weapon.MagicAttack;
                CharacterSetting.MagicDefence -= item.Weapon.MagicDefence;
                CharacterSetting.PoisonAttack -= item.Weapon.PoisonAttack;
                CharacterSetting.PoisonDefence -= item.Weapon.PoisonDefence;
                break;
            case Item.ItemType.Tool:
                return;
        }
        CharacterSetting.Energy -= energy;
        CharacterSetting.Updated = true;
        if (save)
            SaveCharacterSetting();
    }
}
