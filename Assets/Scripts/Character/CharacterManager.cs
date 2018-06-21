using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour {


    private static CharacterManager _instance;

    private static CharacterDatabase _characterDb;
    public ItemDatabase _itemDb;

    private int _basicHealth=1000;
    private int _basicSpeed = 3;
    private int _basicCarry = 100;

    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CharacterManager>();
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {
        _itemDb = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();

        _characterDb = GameObject.FindGameObjectWithTag("Character Database").GetComponent<CharacterDatabase>();
        //print("_availableCharacters.Count =" + _availableCharacters.Characters.Count);
    }



    public Character GetCharacterFromDatabase(int id)
    {
        for (int i = 0; i < _characterDb.Characters.Count; i++)
        {
            if (_characterDb.Characters[i].Id == id)
                return _characterDb.Characters[i];
        }
        return new Character();
    }

    internal CharacterSetting GetPlayerSettings()
    {
        CharacterSetting settings;
        if (_characterDb.PlayerSetting == null)
            settings = new CharacterSetting(0, 0, "Avid2", "Werewolf2");
        else
            settings = _characterDb.PlayerSetting;
        if (settings.Level == 0)
        {//Basic Setting
            print("###Inside GetPlayerSettings Basic Setting");
            Character character = GetCharacterFromDatabase(settings.CharacterId);
            settings.MaxHealth = ((int) character.Body/100 + 1) * _basicHealth;
            settings.Health = settings.MaxHealth;
            settings.MaxMana = settings.MaxHealth; //Todo: calculate basic Mana
            settings.Mana = settings.MaxMana;
            settings.MaxEnergy = settings.MaxHealth;//Todo: calculate basic Energy
            settings.Energy = settings.MaxEnergy;
            settings.AttackSpeed = (int) settings.Speed;//Todo: calculate basic AttackSpeed
            settings.DefenceSpeed = (int) settings.Speed;//Todo: calculate basic DefenceSpeed
            settings.Carry = _basicCarry + (int)character.Carry * 5;
            settings.Speed = ((int)character.Speed / 100 + 1) * _basicSpeed;
        }
        //Equipment setting
        if (settings.Equipments !=null)
            foreach (var id in settings.Equipments)
            {
                ItemContainer item = _itemDb.FindItem(id);
                if (item.Equipment !=null)
                {
                    settings.Agility += item.Equipment.Agility;
                    settings.Bravery += item.Equipment.Bravery;
                    settings.Carry += item.Equipment.Carry;
                    settings.Confidence += item.Equipment.Confidence;
                    settings.Intellect += item.Equipment.Intellect;
                    settings.Krafting += item.Equipment.Krafting;
                    settings.Researching += item.Equipment.Researching;
                    settings.Speed += item.Equipment.Speed;
                    settings.Stemina += item.Equipment.Stemina;
                    settings.Strength += item.Equipment.Strength;
                }
                print("###Inside GetPlayerSettings Equipment = " + id);
            }
        return settings;
        //todo: if not saved return new CharacterSetting();
    }

    internal void SavePlayerSettings()
    {
        _characterDb.SaveCharacterSetting();
    }

    public bool AddEquipment(ItemContainer item)
    {
        if (item.Equipment != null)
            return _characterDb.PlayerSetting.AddEquipment((int) item.Equipment.PlaceHolder, item.Id);
        return false;
    }

    public bool AddEquipment(int id)
    {
        return AddEquipment(_itemDb.FindItem(id));
    }

}
