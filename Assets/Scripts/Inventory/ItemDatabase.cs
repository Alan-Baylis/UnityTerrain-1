﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ItemDatabase : MonoBehaviour {

    public List<ItemContainer> Items = new List<ItemContainer>();



    public int Id;
    public string Name ;
    public string Description ;
    public string IconPath ;
    public int IconId ;
    public int Cost;
    public int Weight;
    public int MaxStackCnt;
    public int StackCnt;
    public Item.ItemType Type ;
    public Item.ItemRarity Rarity ;
    public int[] Values;



    void Start()
    {
        ItemContainer tempItem =new ItemContainer();
        DateTime ExpirationTime = DateTime.Now.Add(new TimeSpan(365, 0, 0, 0, 0));
        LoadItems();

        //Consumable template
        tempItem = new ItemContainer(
            //Id
            Items.Count,                   
            //Name
            "Cherry",
            //Desc
            "One Cherry",
            //IconPath
            "Inventory/InventorySet1",  
            //IconID    
            1,  
            //Coat                            
            0,
            //Weight                            
            0,
            //MaxStackCnt
            20,
            //StackCnt
            1,
            //Type
            Item.ItemType.Consumable,
            //Rarity
            Item.ItemRarity.Common,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[3] {                    
                1,  //Health
                0,  //Mana
                0   //Energy
            }
        );
        if (!tempItem.Exist(Items))
            Items.Add(tempItem);

        //Equipment template 
        tempItem = new ItemContainer(
            //Id
            Items.Count,
            //Name
            "Basic Shoe",
            //Desc
            "Basic shoe from animal leather",
            //IconPath
            "Inventory/InventorySet1",
            //IconID    
            197,
            //Coat                            
            5,
            //Weight                            
            1,
            //MaxStackCnt
            2,
            //StackCnt
            1,
            //Type
            Item.ItemType.Equipment,
            //Rarity
            Item.ItemRarity.Common,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[7] {
                9,//PlaceHolder
                0,//Intellect
                1,//Agility
                0,//Strength
                0,//Stemina
                0,//Speed
                0//Krafting
                //Bravery = 0;
                //Confidence = 0;
            }
        );
        if (!tempItem.Exist(Items))
            Items.Add(tempItem);

        //Weapon template
        tempItem = new ItemContainer(
            //Id
            Items.Count,
            //Name
            "Small Sword",
            //Desc
            "Small Sword",
            //IconPath
            "Inventory/InventorySet1",  
            //IconID    
            73,  
            //Coat                            
            10,
            //Weight                            
            3,
            //MaxStackCnt
            1,
            //StackCnt
            1,
            //Type
            Item.ItemType.Weapon,
            //Rarity
            Item.ItemRarity.Common,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[8] {
                1,  //AttackSpeed
                0,  //DefenceSpeed
                1,  //AbilityAttack
                0,  //AbilityDefence
                0,  //MagicAttack
                0,  //MagicDefence
                0,  //PoisonAttack
                0   //PoisonDefence
            }
        );
        if (!tempItem.Exist(Items))
            Items.Add(tempItem);

        //Substance template
        tempItem = new ItemContainer(
            //Id
            Items.Count,
            //Name
            "Round Rock",
            //Desc
            "Round Rock",
            //IconPath
            "Inventory/InventorySet1",
            //IconID    
            240,
            //Coat                            
            0,
            //Weight                            
            5,
            //MaxStackCnt
            5,
            //StackCnt
            1,
            //Type
            Item.ItemType.Substance,
            //Rarity
            Item.ItemRarity.Common,
            //ExpirationTime
            ExpirationTime,
            //###Extras //todo: ignore for now 
            new int[8] {
                1,  //AttackSpeed
                0,  //DefenceSpeed
                1,  //AbilityAttack
                0,  //AbilityDefence
                0,  //MagicAttack
                0,  //MagicDefence
                0,  //PoisonAttack
                0   //PoisonDefence
            }
        );
        if (!tempItem.Exist(Items))
            Items.Add(tempItem);

        SaveItems();
    }

    private void LoadItems()
    {
        //Empty the Items DB
        Items.Clear();

        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");

        //Type[] itemClassTypes = { typeof(List<ItemContainer>), typeof(ItemContainer), typeof(Equipment), typeof(Weapon), typeof(Consumable) };
        //TODO: May need itemClassTypes to be added as a second paramet

        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        Items = (List<ItemContainer>) serializer.Deserialize(fs);
        fs.Close();
    }

    private void SaveItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, Items);
        fs.Close();
    }

    //Add item button call this function 
    public void CreateItem()
    {
        LoadItems();

        //Add item through the public properties 
        Items.Add(new ItemContainer(Id, Name, Description,
            IconPath, IconId, 
            Cost, Weight,
            MaxStackCnt, StackCnt,
            Type, Rarity,
            DateTime.Now.Add(new TimeSpan(14, 0, 0, 0, 0)),
            Values));

        //Save the new list back in Item.xml file in the streamingAssets folder
        SaveItems();
    }


}
