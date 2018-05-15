using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    public List<Item>  Items = new List<Item>();
    private string jsonData ="";

    
    public void LoadJsonData()
    {
        Item data = JsonUtility.FromJson<Item>(jsonData);
        Debug.Log(data.Name);
        Debug.Log(data.Id);
    }



    void Start()
    {
        //Json not working
        jsonData = File.ReadAllText(Application.dataPath + "/StreamingAssets/item.json");
        //print(jsonData);
        //LoadJsonData();


        Items.Add(new Item(0, "Apple", "Apple Consumable", 1, Item.ItemType.Consumable));
        Items.Add(new Item(1, "Sowrd", "Sowrd Weapon", 10, Item.ItemType.Weapon));
        Items.Add(new Item(2, "Grape", "Grape Consumable Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable ", 1, Item.ItemType.Consumable));
    }
}
