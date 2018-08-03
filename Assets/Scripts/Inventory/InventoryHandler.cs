using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private int _playerSlots;
    private static GUIManager _GUIManager;

    private ItemDatabase _itemDatabase;
    private CharacterManager _characterManager;


    private bool _updateInventory;
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
        _characterManager = CharacterManager.Instance();

        //Todo: make sure we use the new way old way ==>  _GUIManager = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<GUIManager>();
        _GUIManager =  GUIManager.Instance();

        print("###insite Start inventory "+ _characterManager.CharacterSetting.CarryCnt);
        _playerSlots = _characterManager.CharacterSetting.CarryCnt;

        //Inventory Items
        _inventoryPanel = GameObject.Find("Inventory Panel");
        _slotPanel = _inventoryPanel.transform.Find("Slot Panel").gameObject;
        InitInventory(_characterManager.CharacterInventory);

        //Item Mixture
        _itemMixture = ItemMixture.Instance();
        InitMixture(_characterManager.CharacterMixture);

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
        //todo: unleash the inv to be saved 
        _characterManager.SaveCharacterInventory(_invItems);
    }

    public void UpdateInventory(bool value)
    {
        _updateInventory = value;
    }
    
    public bool UseEnergy(int amount)
    {
        if (_characterManager.CharacterSetting.Energy > amount)
        {
            _characterManager.AddCharacterSetting("Energy", -amount);
            return true;
        }
        return false;
    }

    public bool AddItemToInventory(ItemContainer item)
    {
        throw new Exception("AddItemToInventory is not defined.");
    }


    // Update is called once per frame
    void Update () {
	    if (_updateInventory)
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
	        _updateInventory = false;
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
            _itemMixture.LoadEmpty();
        else
            _itemMixture.LoadItem(playerMixture.Item, playerMixture.Time);
    }

    public void SaveCharacterMixture(ItemContainer item, DateTime time)
    {
        _characterManager.SaveCharacterMixture(item, time);
    }

    public Recipe CheckRecipes(int first, int second)
    {
        return _itemDatabase.FindRecipes( first,  second);
    }

    internal void PrintMessage(string message)
    {
        _GUIManager.AddMessage(message);
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
