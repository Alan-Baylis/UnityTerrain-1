using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CharacterDatabase : MonoBehaviour {


    private static CharacterDatabase _characterDatabase;

    private List<Character> _characters = new List<Character>();
    private List<ItemContainer> _characterInventory = new List<ItemContainer>();
    private CharacterSetting _characterSetting;
    private CharacterMixture _characterMixture;



    //Add Charcter button call this function 
    public int Id;
    public string Name;
    public string Description;
    //public string IconPath ;
    //public int IconId ;
    public Character.CharacterType Type;
    public Character.AttackType Attack;
    public Character.DefenceType Defence;
    public Character.SpeedType Speed;
    public Character.BodyType Body;
    public Character.CarryType Carry;
    public void CreateCharacter()
    {
        LoadCharacters();
        //Add Character through the public properties 
        Character tempCharacter = new Character(
            _characters.Count,
            Name,
            Description,
            Type,
            Attack,
            Defence,
            Speed,
            Body,
            Carry
        );
        if (!tempCharacter.Exist(_characters))
            _characters.Add(tempCharacter);
        //Save the new list back in Character.xml file in the streamingAssets folder
        SaveCharacters();
    }
    
    void Awake()
    {
        _characterDatabase = CharacterDatabase.Instance();
        Character tempCharacter = new Character();

        LoadCharacterSetting();
        //PlayerSetting = new CharacterSetting(0, 0, "Avid2", "Werewolf2");
        //SaveCharacterSetting();
        LoadCharacterInventory();

        LoadCharacterMixture();

        LoadCharacters();

        tempCharacter = new Character(
            _characters.Count,
            "Phoenix",
            "Phoenix Flyer",
            Character.CharacterType.Fly,
            Character.AttackType.Close,
            Character.DefenceType.Range,
            Character.SpeedType.Fast,
            Character.BodyType.Tiny,
            Character.CarryType.Light
        );
        if (!tempCharacter.Exist(_characters))
        {
            _characters.Add(tempCharacter);
            SaveCharacters();
        }
    }

    //CharacterInventory
    public List<ItemContainer> FindCharacterInventory(int playerId)
    {
        return _characterInventory;
    }
    public void SaveCharacterInventory(List<ItemContainer> inv)
    {
        //TODO: May need to clrat list
        //print("####Inside SaveCharacterInventory");
        _characterInventory.Clear();
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].Id == -1)
                _characterInventory.Add(new ItemContainer());
            else
                _characterInventory.Add(
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
        //SaveCharacterInventory();
    }
    private void LoadCharacterInventory()
    {
        //Empty the Items DB
        _characterInventory.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterInventory.xml");
        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterInventory = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveCharacterInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterInventory);
        fs.Close();
    }
    //Characters
    public Character FindCharacter(int id)
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            if (_characters[i].Id == id)
                return _characters[i];
        }
        return null;
    }
    private void LoadCharacters()
    {
        //Empty the Characters DB
        _characters.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        //Read the Characters from Character.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characters = (List<Character>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characters);
        fs.Close();
    }
    //CharacterSetting
    public CharacterSetting FindCharacterSetting(int id)
    {
        return _characterSetting;
    }
    public void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        _characterSetting = new CharacterSetting(characterSetting);
        SaveCharacterSetting();
    }
    private void LoadCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        //Read the CharacterSetting from CharacterSetting.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterSetting = (CharacterSetting)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterSetting);
        fs.Close();
    }
    //CharacterMixture
    internal CharacterMixture FindCharacterMixture(int id)
    {
        return _characterMixture;
    }
    public void SaveCharacterMixture(ItemContainer item, DateTime durationMinutes)
    {
        _characterMixture = new CharacterMixture(item, durationMinutes);
        SaveCharacterMixture();
    }
    private void LoadCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        //Read the CharacterMixture from CharacterMixture.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterMixture = (CharacterMixture)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterMixture);
        fs.Close();
    }

    //Instance
    public static CharacterDatabase Instance()
    {
        if (!_characterDatabase)
        {
            _characterDatabase = FindObjectOfType(typeof(CharacterDatabase)) as CharacterDatabase;
            if (!_characterDatabase)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _characterDatabase;
    }


}
