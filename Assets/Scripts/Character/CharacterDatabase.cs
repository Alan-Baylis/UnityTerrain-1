using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CharacterDatabase : MonoBehaviour {


    public List<Character> Characters = new List<Character>();

    public int Id;
    public string Name;
    public string Description;
    //public string IconPath ;
    //public int IconId ;
    public Character.CharacterType Type;
    public Character.AttackType Attack;
    public Character.DefenceType Defence;
    public Character.SpeedType Speed;


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
            Speed));

        //Save the new list back in Character.xml file in the streamingAssets folder
        fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Character.xml"), FileMode.Create);
        serializer.Serialize(fs, Characters);
        fs.Close();
    }

    void Start()
    {
        Characters.Add(new Character(0, "Phoenix", "Phoenix Flyer", Character.CharacterType.Fly, Character.AttackType.Close, Character.DefenceType.Range, Character.SpeedType.Fast));
        
        Characters.Add(new Character(1, "Phoenix2", "Phoenix walker", Character.CharacterType.Walk, Character.AttackType.Close, Character.DefenceType.Range, Character.SpeedType.Fast));
    }

}
