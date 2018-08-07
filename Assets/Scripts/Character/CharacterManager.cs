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

    //private int _basicHealth=1000;
    //private int _basicSpeed = 3;
    //private int _basicCarry = 100;

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


    //public int AddEquipment(int index, int id)
    //{
    //    int OldItem = CharacterSetting.Equipments[index];
    //    CharacterSetting.Equipments[index] = id;
    //    //Todo: Adjust character based on the new equipment an remove old setting
    //    AddCharacterSetting("Health", 2);
    //    //Todo: may need to move 
    //    CharacterSetting.Updated = true;
    //    SaveCharacterSetting();
    //    return OldItem;
    //}


    //public int AddEquipment(ItemContainer item)
    //{
    //    int OldItem=-1;
    //    if (item.Equipment != null)
    //        OldItem = CharacterSetting.AddEquipment((int) item.Equipment.PlaceHolder, item.Id);
    //    return OldItem;
    //}

    //public int AddEquipment(int id)
    //{
    //    return AddEquipment(_itemDb.FindItem(id));
    //}

    //internal CharacterSetting GetPlayerSettings()
    //{
    //    if (CharacterSetting.Level == 0 && CharacterSetting.Experience == 0)
    //    {//Basic Setting
    //        //print("###Inside GetPlayerSettings Basic Setting");
    //        Character character = Character;
    //        CharacterSetting.MaxHealth = ((int)character.Body / 100 + 1) * _basicHealth;
    //        CharacterSetting.Health = CharacterSetting.MaxHealth;
    //        CharacterSetting.MaxMana = CharacterSetting.MaxHealth; //Todo: calculate basic Mana
    //        CharacterSetting.Mana = CharacterSetting.MaxMana;
    //        CharacterSetting.MaxEnergy = CharacterSetting.MaxHealth;//Todo: calculate basic Energy
    //        CharacterSetting.Energy = CharacterSetting.MaxEnergy;
    //        CharacterSetting.AttackSpeed = (int)CharacterSetting.Speed;//Todo: calculate basic AttackSpeed
    //        CharacterSetting.DefenceSpeed = (int)CharacterSetting.Speed;//Todo: calculate basic DefenceSpeed
    //        CharacterSetting.Carry = _basicCarry + (int)character.Carry * 5;
    //        CharacterSetting.CarryCnt = _basicCarry / 20 + CharacterSetting.Level / 5;
    //        CharacterSetting.Speed = ((int)character.Speed / 100 + 1) * _basicSpeed;
    //    }
    //    //Equipment setting
    //    if (CharacterSetting.Equipments != null)
    //        foreach (var id in CharacterSetting.Equipments)
    //        {
    //            if (id == -1)
    //                continue;
    //            ItemContainer item = _itemDatabase.FindItem(id);
    //            if (item.Equipment != null)
    //            {
    //                CharacterSetting.Agility += item.Equipment.Agility;
    //                CharacterSetting.Bravery += item.Equipment.Bravery;
    //                CharacterSetting.Carry += item.Equipment.Carry;
    //                CharacterSetting.CarryCnt += item.Equipment.CarryCnt;
    //                CharacterSetting.Confidence += item.Equipment.Confidence;
    //                CharacterSetting.Intellect += item.Equipment.Intellect;
    //                CharacterSetting.Krafting += item.Equipment.Krafting;
    //                CharacterSetting.Researching += item.Equipment.Researching;
    //                CharacterSetting.Speed += item.Equipment.Speed;
    //                CharacterSetting.Stemina += item.Equipment.Stemina;
    //                CharacterSetting.Strength += item.Equipment.Strength;
    //            }
    //            //print("###Inside GetPlayerSettings Equipment = " + id);
    //        }
    //    return CharacterSetting;
    //    //todo: if not saved return new CharacterSetting();
    //}


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

}
