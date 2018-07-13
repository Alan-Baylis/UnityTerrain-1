using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour {


    private static CharacterManager _instance;

    private CharacterDatabase _characterDb;
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
    public Character GetCharacterFromDatabase()
    {
        return GetCharacterFromDatabase(_characterDb.PlayerSetting.CharacterId);
    }


    internal CharacterSetting GetPlayerSettings()
    {
        if (_characterDb.PlayerSetting.Level == 0)
        {//Basic Setting
            //print("###Inside GetPlayerSettings Basic Setting");
            Character character = GetCharacterFromDatabase(_characterDb.PlayerSetting.CharacterId);
            _characterDb.PlayerSetting.MaxHealth = ((int) character.Body/100 + 1) * _basicHealth;
            _characterDb.PlayerSetting.Health = _characterDb.PlayerSetting.MaxHealth;
            _characterDb.PlayerSetting.MaxMana = _characterDb.PlayerSetting.MaxHealth; //Todo: calculate basic Mana
            _characterDb.PlayerSetting.Mana = _characterDb.PlayerSetting.MaxMana;
            _characterDb.PlayerSetting.MaxEnergy = _characterDb.PlayerSetting.MaxHealth;//Todo: calculate basic Energy
            _characterDb.PlayerSetting.Energy = _characterDb.PlayerSetting.MaxEnergy;
            _characterDb.PlayerSetting.AttackSpeed = (int)_characterDb.PlayerSetting.Speed;//Todo: calculate basic AttackSpeed
            _characterDb.PlayerSetting.DefenceSpeed = (int)_characterDb.PlayerSetting.Speed;//Todo: calculate basic DefenceSpeed
            _characterDb.PlayerSetting.Carry = _basicCarry + (int)character.Carry * 5;
            _characterDb.PlayerSetting.CarryCnt = _basicCarry/20 + _characterDb.PlayerSetting.Level /5;
            _characterDb.PlayerSetting.Speed = ((int)character.Speed / 100 + 1) * _basicSpeed;
        }
        //Equipment setting
        if (_characterDb.PlayerSetting.Equipments !=null)
            foreach (var id in _characterDb.PlayerSetting.Equipments)
            {
                if (id == -1)
                    continue;
                ItemContainer item = _itemDb.FindItem(id);
                if (item.Equipment !=null)
                {
                    _characterDb.PlayerSetting.Agility += item.Equipment.Agility;
                    _characterDb.PlayerSetting.Bravery += item.Equipment.Bravery;
                    _characterDb.PlayerSetting.Carry += item.Equipment.Carry;
                    _characterDb.PlayerSetting.CarryCnt += item.Equipment.CarryCnt;
                    _characterDb.PlayerSetting.Confidence += item.Equipment.Confidence;
                    _characterDb.PlayerSetting.Intellect += item.Equipment.Intellect;
                    _characterDb.PlayerSetting.Krafting += item.Equipment.Krafting;
                    _characterDb.PlayerSetting.Researching += item.Equipment.Researching;
                    _characterDb.PlayerSetting.Speed += item.Equipment.Speed;
                    _characterDb.PlayerSetting.Stemina += item.Equipment.Stemina;
                    _characterDb.PlayerSetting.Strength += item.Equipment.Strength;
                }
                //print("###Inside GetPlayerSettings Equipment = " + id);
            }
        return _characterDb.PlayerSetting;
        //todo: if not saved return new CharacterSetting();
    }

    internal void SavePlayerSettings()
    {
        _characterDb.SaveCharacterSetting();
    }

    //public int AddEquipment(ItemContainer item)
    //{
    //    int OldItem=-1;
    //    if (item.Equipment != null)
    //        OldItem = _characterDb.PlayerSetting.AddEquipment((int) item.Equipment.PlaceHolder, item.Id);
    //    return OldItem;
    //}

    //public int AddEquipment(int id)
    //{
    //    return AddEquipment(_itemDb.FindItem(id));
    //}

}
