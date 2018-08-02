using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ItemDatabase : MonoBehaviour {

    private static ItemDatabase _itemDatabase;

    public List<ItemContainer> Items = new List<ItemContainer>();

    public List<Recipe> Recipes = new List<Recipe>();  

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
    public Item.ItemRarity Rarity;
    public int DurationDays;
    public int[] Values;


    public int _defaultDurationDays = 365;

    public static ItemDatabase Instance()
    {
        if (!_itemDatabase)
        {
            _itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
            if (!_itemDatabase)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _itemDatabase;
    }

    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();
    }

    void Start()
    {
        ItemContainer tempItem =new ItemContainer();
        DateTime ExpirationTime = DateTime.Now.Add(new TimeSpan(_defaultDurationDays, 0, 0, 0, 0));

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
            //DurationDays
            _defaultDurationDays,
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
            "Basic Glove",
            //Desc
            "Basic glove from animal leather",
            //IconPath
            "Inventory/InventorySet1",
            //IconID    
            204,
            //Coat                            
            3,
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
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[12] {
                9,//PlaceHolder = (PlaceType) values[0];
                0,//Agility = values[1];
                0,//Bravery = values[2];
                0,//Carry = values[3];
                0,//CarryCnt = values[4];
                0,//Confidence = values[5];
                0,//Intellect = values[6];
                1,//Krafting = values[7];
                0,//Researching = values[8];
                0,//Speed = values[9];
                0,//Stemina = values[10];
                0//Strength = values[11];
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
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[9] {
                0,  //CarryType
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
            //DurationDays
            _defaultDurationDays,
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

        LoadRecipes();

        //PrintRecipes();
    }

    public ItemContainer FindItem(int id)
    {
        for (int i = 0; i < Items.Count; i++)
            if (Items[i].Id == id)
                return Items[i];
        return null;
    }

    private void PrintRecipes()
    {
        for (int i = 0; i < Recipes.Count; i++)
        {
            Recipe r = Recipes[i];
            if (r.IsEnable)
                print(i + "- id(" + r.Id + ") " + 
                      r.FirstItemId + "(" + r.FirstItemCnt + ") + " + 
                      r.SecondItemId + "(" + r.SecondItemCnt + ") = " + 
                      r.FinalItemId + "(" + r.FinalItemCnt + ")");
        }
    }

    private void LoadItems()
    {
        //Empty the Items DB
        Items.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
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

    private void LoadRecipes()
    {
        //Empty the Recipes DB
        Recipes.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Recipe.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        Recipes = (List<Recipe>)serializer.Deserialize(fs);
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
            DurationDays, DateTime.Now.Add(new TimeSpan(DurationDays, 0, 0, 0, 0)),
            Values));
        //Save the new list back in Item.xml file in the streamingAssets folder
        SaveItems();
    }



}
