using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private static CharacterDatabase _characterDb;
    private int _playerSlots;

    private ItemDatabase _itemDb;

    private GameObject inventoryPanel;
    private GameObject slotPanel;
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
    void Start ()
    {
        _itemDb = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();

        _characterDb = GameObject.FindGameObjectWithTag("Character Database").GetComponent<CharacterDatabase>();
        CharacterSetting settings = _characterDb.PlayerSetting;
        //print("###insite Start inventory "+ settings.CarryCnt);
        _playerSlots = settings.CarryCnt;

        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
        
        InitInventory(_characterDb.PlayerInventory);

        for (int i = 0; i < _slotAmount; i++)
        {
            if (i < _playerSlots)
            {
                InvSlots.Add(Instantiate(InventorySlot));
                InvSlots[i].GetComponent<SlotData>().SlotIndex = i;
                print(i + "-" +InvSlots[i].GetComponent<SlotData>().SlotIndex );
            }
            else
                InvSlots.Add(Instantiate(InventorySlotBroken));

            InvSlots[i].transform.SetParent(slotPanel.transform);

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
                    if (_invItems[i].StackCnt >1)
                        itemObject.transform.GetChild(0).GetComponent<Text>().text = _invItems[i].StackCnt.ToString();
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
        //Disable the pannel
        //inventoryPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
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
    internal void PrintInventory(List<ItemContainer> inv)
    {
        print("here");
        for (int i = 0; i < inv.Count; i++)
        {
            print(inv[i].Id);
        }
    }

}
