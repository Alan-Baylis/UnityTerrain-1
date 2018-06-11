using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {


    private static CharacterManager instance;

    private CharacterDatabase _availableCharacters;

    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CharacterManager>();
            return instance;
        }
    }


    // Use this for initialization
    void Start () {
        _availableCharacters = GameObject.FindGameObjectWithTag("Character Database").GetComponent<CharacterDatabase>();
        print("_availableCharacters.Count =" + _availableCharacters.Characters.Count);
    }



    public Character GetCharacterFromDatabase(int id)
    {
        for (int i = 0; i < _availableCharacters.Characters.Count; i++)
        {
            if (_availableCharacters.Characters[i].Id == id)
                return _availableCharacters.Characters[i];
        }
        return new Character();
    }

}
