using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private static CharacterDatabase _characterDb;
    private int _playerSlots;
    private static GUIManager _GUIManager;
    public bool UpdateInventory = false;

    private ItemDatabase _itemDatabase;


    private ItemMixture _itemMixture;

    private GameObject _inventoryPanel;
    private GameObject _slotPanel;

    public GameObject InventorySlot;
    public GameObject InventorySlotBroken;
    public GameObject InventoryItam;
    public Sprite LockSprite;


    private List<ItemContainer> _invItems = new List<ItemContainer>();
    public List<GameObject> InvSlots = new List<GameObject>();

    private int _slotAmount = 30;
    //private int _slotsX = 5;
    //private int _slotsY = 6;
    //private int _slotsPadding = 10;

    // Use this for initialization
    void Awake ()
    {
        _itemDatabase = ItemDatabase.Instance();
        //Todo: make sure we use the new way old way ==>  _GUIManager = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<GUIManager>();
        _GUIManager =  GUIManager.Instance(); 

        _characterDb = GameObject.FindGameObjectWithTag("Character Database").GetComponent<CharacterDatabase>();
        CharacterSetting settings = _characterDb.PlayerSetting;
        //print("###insite Start inventory "+ settings.CarryCnt);
        _playerSlots = settings.CarryCnt;


        _inventoryPanel = GameObject.Find("Inventory Panel");
        _slotPanel = _inventoryPanel.transform.Find("Slot Panel").gameObject;
        
        InitInventory(_characterDb.PlayerInventory);


        _itemMixture = GameObject.Find("Item Mixture").GetComponent<ItemMixture>();
        InitMixture(_characterDb.PlayerMixture);

        for (int i = 0; i < _slotAmount; i++)
        {
            if (i < _playerSlots)
            {
                InvSlots.Add(Instantiate(InventorySlot));
                InvSlots[i].GetComponent<SlotData>().SlotIndex = i;
                //print(i + "-" +InvSlots[i].GetComponent<SlotData>().SlotIndex );
            }
            else
                InvSlots.Add(Instantiate(InventorySlotBroken));

            InvSlots[i].transform.SetParent(_slotPanel.transform);

            GameObject itemObject = Instantiate(InventoryItam);

            ItemData data = itemObject.GetComponent<ItemData>();
            data.Item = _invItems[i];
            data.SlotIndex = i;

            //print(_invItems[i].Id + "-" + i);
            itemObject.transform.SetParent(InvSlots[i].transform);
            itemObject.transform.position = Vector2.zero;
            InvSlots[i].name = itemObject.name = _invItems[i].Name;
            if (i < _playerSlots)
            {
                if (_invItems[i].Id != -1)
                {
                    itemObject.GetComponent<Image>().sprite = _invItems[i].GetSprite();
                    itemObject.transform.GetChild(0).GetComponent<Text>().text = _invItems[i].StackCnt > 1 ? _invItems[i].StackCnt.ToString() :"";
                }
            }
            //todo: lets user buy a slot 
            else if (i == _playerSlots)
            {
                itemObject.GetComponent<Image>().sprite = LockSprite;
                InvSlots[i].name = itemObject.name = "Lock";
            }
        }
        _characterDb.SaveCharacterInventory(_invItems);
    }


    public bool UseEnergy(int amount)
    {
        if (_characterDb.PlayerSetting.Energy > amount)
        {
            _characterDb.AddCharacterSetting("Energy", -amount);
            return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update () {
	    if (UpdateInventory)
	    {
            //Refresh _invItems based on the interface 
            //Todo: security vulnerability: might be able to change inv 
            _invItems.Clear();
            for (int i = 0; i < _slotAmount; i++)
	        {
	            ItemContainer tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>().Item;
	            _invItems.Add(tmpItem);
	        }
            //Save new inventory 
	        //_characterDb.SaveCharacterInventory(_invItems);
	        UpdateInventory = false;
	    }
    }

    internal void InitInventory(List<ItemContainer> sourceInv)
    {
        _invItems.Clear();
        for (int i = 0; i < _slotAmount; i++)
        {
            if (sourceInv.Count <= i)
                _invItems.Add(new ItemContainer());
            else
                if (sourceInv[i].Id == -1)
                    _invItems.Add(new ItemContainer());
                else
                    _invItems.Add(
                        new ItemContainer(
                            sourceInv[i].Id, sourceInv[i].Name, sourceInv[i].Description,
                            sourceInv[i].IconPath, sourceInv[i].IconId,
                            sourceInv[i].Cost, sourceInv[i].Weight,
                            sourceInv[i].MaxStackCnt, sourceInv[i].StackCnt,
                            sourceInv[i].Type, sourceInv[i].Rarity,
                            sourceInv[i].DurationDays, sourceInv[i].ExpirationTime,
                            sourceInv[i].Values)
                    );
        }
    }

    private void InitMixture(CharacterMixture playerMixture)
    {
        if (playerMixture == null)
            return;
        if (playerMixture.Item == null)
            return;
        if (playerMixture.Item.Id == -1)
            return;
        _itemMixture.LoadItem(playerMixture.Item, playerMixture.Time);
    }

    public void SaveCharacterMixture(ItemContainer item, DateTime time)
    {
        _characterDb.SaveCharacterMixture(item, time);
    }

    public Recipe CheckRecipes(int first, int second)
    {
        for (int i = 0; i < _itemDatabase.Recipes.Count; i++)
        {
            Recipe r = _itemDatabase.Recipes[i];
            if (r.IsEnable && first == r.FirstItemId && second == r.SecondItemId)
                return r;
            if (r.IsEnable && first == r.SecondItemId && second == r.FirstItemId)
                return Reverse(r);
        }
        return null;
    }

    internal void PrintMessage(string message)
    {
        _GUIManager.AddMessage(message);
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
    public ItemContainer GetItemFromDatabase(int id)
    {
        if (id == -1)
            return new ItemContainer();
        return _itemDatabase.FindItem(id);
    }

    internal void PrintInventory(List<ItemContainer> inv)
    {
        string invStrt = "Print Inventory: ";
        for (int i = 0; i < inv.Count; i++)
        {
            invStrt+= inv[i].Id+'-';
        }
        print(invStrt);
    }

}
