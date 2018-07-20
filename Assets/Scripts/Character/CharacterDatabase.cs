using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CharacterDatabase : MonoBehaviour {


    public List<Character> Characters = new List<Character>();

    public CharacterSetting PlayerSetting;

    public List<ItemContainer> PlayerInventory = new List<ItemContainer>();
    

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


    //Add Charcter button call this function 
    public void CreateCharacter()
    {
        LoadCharacters();
        //Add Character through the public properties 
        Character tempCharacter = new Character(
            Characters.Count,
            Name,
            Description,
            Type,
            Attack,
            Defence,
            Speed,
            Body,
            Carry
        );
        if (!tempCharacter.Exist(Characters))
            Characters.Add(tempCharacter);
        //Save the new list back in Character.xml file in the streamingAssets folder
        SaveCharacters();
    }

    void Start()
    {
        Character tempCharacter = new Character();

        LoadCharacterSetting();
        //PlayerSetting = new CharacterSetting(0, 0, "Avid2", "Werewolf2");
        //SaveCharacterSetting();

        LoadCharacterInventory();

        LoadCharacters();
        tempCharacter = new Character(
            Characters.Count, 
            "Phoenix", 
            "Phoenix Flyer", 
            Character.CharacterType.Fly, 
            Character.AttackType.Close, 
            Character.DefenceType.Range,
            Character.SpeedType.Fast, 
            Character.BodyType.Tiny, 
            Character.CarryType.Light
        );
        if (!tempCharacter.Exist(Characters))
            Characters.Add(tempCharacter);


        tempCharacter = new Character(
            Characters.Count, 
            "Sailormoon", 
            "Sailormoon walker", 
            Character.CharacterType.Walk, 
            Character.AttackType.Close, 
            Character.DefenceType.Close,
            Character.SpeedType.Regular, 
            Character.BodyType.Slim, 
            Character.CarryType.Light
        );
        if (!tempCharacter.Exist(Characters))
            Characters.Add(tempCharacter);

        SaveCharacters();
    }

    internal void AddCharacterSetting(string field, float value)
    {
        switch (field)
        {
            case "Agility":
                PlayerSetting.Agility += value;
                break;
            case "Health":
                PlayerSetting.Health -= (int) value;
                PlayerSetting.Coin += 1;
                break;
        }
    }

    public int AddEquipment(int index, int id)
    {
        int OldItem = PlayerSetting.Equipments[index];
        PlayerSetting.Equipments[index] = id;
        //Todo: Adjust character based on the new equipment an remove old setting
        AddCharacterSetting("Health", 2);
        //Todo: may need to move 
        PlayerSetting.Updated = true;
        SaveCharacterSetting();
        return OldItem;
    }

    private void LoadCharacterInventory()
    {
        //Empty the Items DB
        PlayerInventory.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerInventory.xml");
        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        PlayerInventory = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();
    }

    private void SaveCharacterInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, PlayerInventory);
        fs.Close();
    }
    internal void SaveCharacterInventory(List<ItemContainer> inv)
    {
        //TODO: May need to clrat list
        //print("####Inside SaveCharacterInventory");
        PlayerInventory.Clear();
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].Id==-1)
                PlayerInventory.Add(new ItemContainer());
            else
                PlayerInventory.Add(
                    new ItemContainer(
                        inv[i].Id, inv[i].Name, inv[i].Description,
                        inv[i].IconPath, inv[i].IconId,
                        inv[i].Cost, inv[i].Weight,
                        inv[i].MaxStackCnt, inv[i].StackCnt,
                        inv[i].Type, inv[i].Rarity,
                        inv[i].DurationDays,inv[i].ExpirationTime,
                        inv[i].Values)
                    );
        }
        //todo: make it async
        SaveCharacterInventory();
    }

    private void LoadCharacters()
    {
        //Empty the Characters DB
        Characters.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        //Read the Characters from Character.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Open);
        Characters = (List<Character>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, Characters);
        fs.Close();
    }

    private void LoadCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        //Read the CharacterSetting from CharacterSetting.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Open);
        PlayerSetting = (CharacterSetting)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, PlayerSetting);
        fs.Close();
    }
}
