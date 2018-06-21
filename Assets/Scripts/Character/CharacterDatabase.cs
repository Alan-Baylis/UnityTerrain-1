using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CharacterDatabase : MonoBehaviour {


    public List<Character> Characters = new List<Character>();

    public CharacterSetting PlayerSetting = new CharacterSetting();

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
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");

        //Read the Characters from Character.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Open);
        Characters = (List<Character>)serializer.Deserialize(fs);
        fs.Close();
        
        //Add Character through the public properties 
        Characters.Add(new Character(Id, Name, Description,
            //IconPath, IconId, 
            Type, Attack, Defence,
            Speed, Body, Carry));

        //Save the new list back in Character.xml file in the streamingAssets folder
        fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Character.xml"), FileMode.Create);
        serializer.Serialize(fs, Characters);
        fs.Close();
    }

    void Start()
    {

        LoadCharacterSetting();

        Characters.Add(new Character(0, "Phoenix", "Phoenix Flyer", Character.CharacterType.Walk, Character.AttackType.Close, Character.DefenceType.Range, 
            Character.SpeedType.Fast, Character.BodyType.Tiny, Character.CarryType.Light
            ));
        
        Characters.Add(new Character(1, "Phoenix2", "Phoenix walker", Character.CharacterType.Fly, Character.AttackType.Close, Character.DefenceType.Range, 
            Character.SpeedType.Fast, Character.BodyType.Tiny, Character.CarryType.Light
            ));
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
