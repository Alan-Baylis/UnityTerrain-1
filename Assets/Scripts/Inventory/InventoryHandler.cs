using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private static InventoryHandler _inv;
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
    public GameObject InventoryItem;
    public Sprite LockSprite;


    private List<ItemContainer> _invItems = new List<ItemContainer>();
    public List<GameObject> InvSlots = new List<GameObject>();
    private SlotEquipment[] _equiSlots = new SlotEquipment[13];
    private List<int> _equipments = new List<int>();

    private int _slotAmount = 30;
    //private int _slotsX = 5;
    //private int _slotsY = 6;
    //private int _slotsPadding = 10;

    // Use this for initialization
    void Awake()
    {
        _inv = InventoryHandler.Instance();
        _itemDatabase = ItemDatabase.Instance();
        _characterManager = CharacterManager.Instance();

        //Todo: make sure we use the new way old way ==>  _GUIManager = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<GUIManager>();
        _GUIManager = GUIManager.Instance();
    }

    void Start()
    {
            //print("###insite Start inventory "+ _characterManager.CharacterSetting.CarryCnt);
            _playerSlots = _characterManager.CharacterSetting.CarryCnt;

        //Inventory Items
        _inventoryPanel = GameObject.Find("Inventory Panel");
        _slotPanel = _inventoryPanel.transform.Find("Slot Panel").gameObject;
        InitInventory(_characterManager.CharacterInventory);

        //Equipment
        _equipments = _characterManager.CharacterSetting.Equipments;
        SlotEquipment[] equiSlots = _inventoryPanel.GetComponentsInChildren<SlotEquipment>();

        for (int i = 0; i < equiSlots.Length; i++)
            _equiSlots[(int)equiSlots[i].EquType] = equiSlots[i];
        for (int i = 0; i < _equiSlots.Length; i++)
        {
            //print("index : "+i+"-"+_equiSlots[i].EquType + (int)_equiSlots[i].EquType + " id from in=  "+ _equipments[i]);
            _equiSlots[i].name = "Slot " + _equiSlots[i].EquType;
            _equiSlots[i].GetComponentInChildren<Text>().text = _equiSlots[i].EquType.ToString();
            ItemEquipment equipmentItem = _equiSlots[i].GetComponentInChildren<ItemEquipment>();
            if (_equipments[i] == -1)
            {
                equipmentItem.Item = new ItemContainer();
                equipmentItem.name = "Empty";
            }
            else
            {
                ItemContainer tempItem = _itemDatabase.FindItem(_equipments[i]);
                equipmentItem.Item =
                    new ItemContainer(
                        tempItem.Id, tempItem.Name, tempItem.Description,
                        tempItem.IconPath, tempItem.IconId,
                        tempItem.Cost, tempItem.Weight,
                        tempItem.MaxStackCnt, tempItem.MaxStackCnt, //*** Equipmet only accept maxstacks
                        tempItem.Type, tempItem.Rarity,
                        tempItem.DurationDays, tempItem.ExpirationTime,
                        tempItem.Values);
                equipmentItem.name = tempItem.Name;
                equipmentItem.GetComponent<Image>().sprite = tempItem.GetSprite();
            }
        }

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

            GameObject itemObject = Instantiate(InventoryItem);

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
            else
            {
                data.enabled = false;
                if (i == _playerSlots)
                {
                    itemObject.GetComponent<Image>().sprite = LockSprite;
                    InvSlots[i].name = itemObject.name = "Lock";
                }

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_inventoryPanel.activeSelf)
                _inventoryPanel.SetActive(false);
            else
                _inventoryPanel.SetActive(true);
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


    private bool UseItem(ItemContainer item, int invIndex)
    {
        switch (item.Type)
        {
            case Item.ItemType.Consumable:
                return false;
            case Item.ItemType.Equipment:
                return false;
            case Item.ItemType.Weapon:
                return false;
            case Item.ItemType.Tool:
                return false;
            default:
                _GUIManager.AddMessage("YEL: " + item.Name + " can not be used");
                return false;
        }
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


    public static InventoryHandler Instance()
    {
        if (!_inv)
        {
            _inv = FindObjectOfType(typeof(InventoryHandler)) as InventoryHandler;
            if (!_inv)
                Debug.LogError("There needs to be one active InventoryHandler script on a GameObject in your scene.");
        }
        return _inv;
    }

}
