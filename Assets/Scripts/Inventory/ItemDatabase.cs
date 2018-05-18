using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ItemDatabase : MonoBehaviour {

    public List<ItemContainer> Items = new List<ItemContainer>();


    private int[] _valueArray;


    public int Id;
    public string Name ;
    public string Description ;
    //public string IconPath ;
    //public int IconId ;
    public int Cost ;
    public int MaxStackCnt;
    public int StackCnt;
    public Item.ItemType Type ;
    public Item.ItemRarity Rarity ;
    public int[] Values;


    //Add item button call this function 
    public void CreateItem()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");

        Type[] itemClassTypes = { typeof(List<ItemContainer>), typeof(ItemContainer), typeof(Equipment), typeof(Weapon), typeof(Consumable) };
        //TODO: May need itemClassTypes to be added as a second paramet

        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer =new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        Items = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();


        //Add item through the public properties 
        Items.Add(new ItemContainer(Id, Name, Description,
            //IconPath, IconId, 
            Cost, MaxStackCnt, StackCnt,
            Type, Rarity,
            Values));

        //Save the new list back in Item.xml file in the streamingAssets folder
        fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Item.xml"), FileMode.Create);
        serializer.Serialize(fs, Items);
        fs.Close();
    }

    void Start()
    {


        _valueArray = new int[3] { 11, 12,0 };
        Items.Add(new ItemContainer(0, "Apple", "Apple Consumable", 2, 10, 9, Item.ItemType.Consumable,Item.ItemRarity.Common, _valueArray));

        _valueArray = new int[3] { 14, 0, 0 };
        Items.Add(new ItemContainer(1, "Grape", "Grape Consumable Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable  Grape Consumable ", 1, 10, 8, Item.ItemType.Consumable, Item.ItemRarity.Rare, _valueArray));

        Items.Add(new ItemContainer(2, "Sowrd", "Sowrd Weapon", 10, 1, 1, Item.ItemType.Equipment, Item.ItemRarity.Common));

    }
}
