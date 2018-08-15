using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ItemDatabase : MonoBehaviour {

    private static ItemDatabase _itemDatabase;

    private List<ItemContainer> _items = new List<ItemContainer>();
    private List<Recipe> _recipes = new List<Recipe>();  



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


    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();

        ItemContainer tempItem =new ItemContainer();
        DateTime ExpirationTime = DateTime.Now.Add(new TimeSpan(_defaultDurationDays, 0, 0, 0, 0));

        LoadItems();

        //Consumable template
        tempItem = new ItemContainer(
            //Id
            _items.Count,                   
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
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);

        //Equipment template 
        tempItem = new ItemContainer(
            //Id
            _items.Count,
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
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);

        //Weapon template
        tempItem = new ItemContainer(
            //Id
            _items.Count,
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
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);


        //Tools template   //Iron Metal Silver Golden
        tempItem = new ItemContainer(
            //Id
            _items.Count,
            //Name
            "Iron Axe",
            //Desc
            "Iron Axe",
            //IconPath
            "Inventory/InventoryTools",
            //IconID    
            2,
            //Coat                            
            10,
            //Weight                            
            5,
            //MaxStackCnt
            1,
            //StackCnt
            1,
            //Type
            Item.ItemType.Tool,
            //Rarity
            Item.ItemRarity.Common,
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras //todo: ignore for now 
            new int[2] {
                100,  //FavouriteEllement
                2  //FavouriteEllement
            }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);
        //Substance template
        tempItem = new ItemContainer(
            //Id
            _items.Count,
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
            new int[] { }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);    

        SaveItems();
        //PrintItems();
        LoadRecipes();

        //PrintRecipes();
    }

    public ItemContainer FindItem(int id)
    {
        for (int i = 0; i < _items.Count; i++)
            if (_items[i].Id == id)
                return _items[i];  
        return null;
    }

    public List<Recipe> RecipeList()
    {
        return _recipes;
    }

    public List<ItemContainer> RecipeItems(Recipe r)
    {
        return new List<ItemContainer> { FindItem(r.FirstItemId), FindItem(r.SecondItemId), FindItem(r.FinalItemId) };
    }

    public Recipe FindRecipes(int first, int second)
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            Recipe r = _recipes[i];
            if (r.IsEnable && first == r.FirstItemId && second == r.SecondItemId)
                return r;
            if (r.IsEnable && first == r.SecondItemId && second == r.FirstItemId)
                return Reverse(r);
        }
        return null;
    }

    private Recipe Reverse(Recipe r)
    {
        int temp = r.FirstItemId;
        r.FirstItemId = r.SecondItemId;
        r.SecondItemId = temp;
        temp = r.FirstItemCnt;
        r.FirstItemCnt = r.SecondItemCnt;
        r.SecondItemCnt = temp;
        return r;
    }

    private void PrintRecipes()
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            Recipe r = _recipes[i];
            if (r.IsEnable)
                print(i + "- id(" + r.Id + ") " + 
                      r.FirstItemId + "(" + r.FirstItemCnt + ") + " + 
                      r.SecondItemId + "(" + r.SecondItemCnt + ") = " + 
                      r.FinalItemId + "(" + r.FinalItemCnt + ")");
        }
    }

    private void PrintItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            ItemContainer r = _items[i];
            if (r.IsEnable)
                print(i + "- id(" + r.Id + ") " +
                      r.Name + "(" + r.Type + ") + ");
        }
    }

    private void LoadItems()
    {
        //Empty the Items DB
        _items.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _items = (List<ItemContainer>) serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _items);
        fs.Close();
    }

    private void LoadRecipes()
    {
        //Empty the Recipes DB
        _recipes.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Recipe.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _recipes = (List<Recipe>)serializer.Deserialize(fs);
        fs.Close();
    }

    public List<Offer> LoadOffers()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Offer.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Offer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        List<Offer> offers = (List<Offer>)serializer.Deserialize(fs);
        fs.Close();
        return offers;
    }

    //Add item button call this function 
    public void CreateItem()
    {
        LoadItems();
        //Add item through the public properties 
        _items.Add(new ItemContainer(Id, Name, Description,
            IconPath, IconId, 
            Cost, Weight,
            MaxStackCnt, StackCnt,
            Type, Rarity,
            DurationDays, DateTime.Now.Add(new TimeSpan(DurationDays, 0, 0, 0, 0)),
            Values));
        //Save the new list back in Item.xml file in the streamingAssets folder
        SaveItems();
    }

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

}
