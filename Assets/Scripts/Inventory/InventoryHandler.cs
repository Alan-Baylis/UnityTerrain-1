using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryHandler : MonoBehaviour
{
    private static InventoryHandler _inv;
    private int _playerSlots;
    private GUIManager _GUIManager;
    private ModalPanel _modalPanel;

    private ItemDatabase _itemDatabase;
    private CharacterManager _characterManager;


    private bool _updateInventory;
    private bool _updateEquipments;
    private ItemMixture _itemMixture;
    private GameObject _inventoryPanel;
    private GameObject _slotPanel;

    [SerializeField]
    private GameObject InventorySlot;
    [SerializeField]
    private GameObject InventorySlotBroken;
    [SerializeField]
    private GameObject InventoryItem;
    [SerializeField]
    private Sprite LockSprite;


    private List<ItemContainer> _invItems = new List<ItemContainer>();
    public List<GameObject> InvSlots = new List<GameObject>();
    public SlotEquipment[] EquiSlots = new SlotEquipment[14];
    private List<ItemContainer> _equipments = new List<ItemContainer>();

    private int _slotAmount = 30;
    private int _basicEnergyUse = 10;

    public static int SceneIdForRecepie = 2;
    public static int SceneIdForStore = 3;
    public static int SceneIdForSetting = 4;
    public static int SceneIdForCredits = 5;
    public bool ShowInventory;


    //private int _slotsX = 5;
    //private int _slotsY = 6;
    //private int _slotsPadding = 10;

    // Use this for initialization
    void Awake()
    {
        _inv = InventoryHandler.Instance();
        _itemDatabase = ItemDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        _GUIManager = GUIManager.Instance();
        _modalPanel = ModalPanel.Instance();

        _inventoryPanel = GameObject.Find("Inventory Panel");
        _slotPanel = _inventoryPanel.transform.Find("Slot Panel").gameObject;
    }

    void Start()
    {
        //print("###insite Start inventory "+ _characterManager.CharacterSetting.CarryCnt);
        _playerSlots = _characterManager.CharacterSetting.CarryCnt;
        print("_playerSlots =" + _playerSlots);
        //Inventory Items
        //InitInventory(_characterManager.CharacterInventory);
        _invItems = _characterManager.CharacterInventory;

        //Equipment
        _equipments = _characterManager.CharacterSetting.Equipments;
        SlotEquipment[] equiSlots = _inventoryPanel.GetComponentsInChildren<SlotEquipment>();
        for (int i = 0; i < equiSlots.Length; i++)
            EquiSlots[(int)equiSlots[i].EquType] = equiSlots[i];
        for (int i = 0; i < EquiSlots.Length; i++)
        {
            //print("index : "+i+"-"+_equiSlots[i].EquType + (int)_equiSlots[i].EquType + " id from in=  "+ _equipments[i]);
            EquiSlots[i].name = "Slot " + EquiSlots[i].EquType;
            EquiSlots[i].GetComponentInChildren<Text>().text = EquiSlots[i].EquType.ToString();
            ItemEquipment equipmentItem = EquiSlots[i].GetComponentInChildren<ItemEquipment>();
            if (_equipments[i].Id == -1)
            {
                equipmentItem.Item = new ItemContainer();
                equipmentItem.name = "Empty";
            }
            else
            {
                ItemContainer tempItem = _equipments[i];
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
        //Inventory
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
            if (i < _playerSlots)
            {
                GameObject itemObject = Instantiate(InventoryItem);

                ItemData data = itemObject.GetComponent<ItemData>();
                data.Item = _invItems[i];
                data.SlotIndex = i;
                //print(_invItems[i].Id + "-" + i);
                itemObject.transform.SetParent(InvSlots[i].transform);
                itemObject.transform.position = Vector2.zero;
                InvSlots[i].name = itemObject.name = _invItems[i].Name;
                if (_invItems[i].Id != -1)
                {
                    itemObject.GetComponent<Image>().sprite = _invItems[i].GetSprite();
                    itemObject.transform.GetChild(0).GetComponent<Text>().text = _invItems[i].StackCnt > 1 ? _invItems[i].StackCnt.ToString() :"";
                }
            }
            //todo: lets user buy a slot 
            else
            {
                if (i == _playerSlots)
                {
                    Button button = InvSlots[i].GetComponentInChildren<Button>();
                    button.GetComponent<Image>().sprite = LockSprite;
                    InvSlots[i].name = button.name = "Lock";
                    button.interactable = true;
                }
            }
            InvSlots[i].transform.localScale = Vector3.one;
        }
    }

    internal float GetKrafting()
    {
        return (_characterManager.CharacterSetting.Krafting % 1000)/1000 ;
    }

    void Update()
    {
        if (ShowInventory)
        {
            _inventoryPanel.SetActive(true);
            ShowInventory = false;
        }

        if (_updateInventory)
        {
            //print("#####_updateInventory " +_invItems.Count);
            //Todo: security vulnerability: might be able to change inv 
            //Refresh _invItems based on the interface 
            for (int i = 0; i < _playerSlots; i++)
            {
                ItemContainer tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>().Item;
                //tmpItem.Print();
                _invItems[i] = tmpItem;
            }
            //Save new inventory 
            _characterManager.SaveCharacterInventory();
            _updateInventory = false;
        }

        if (_updateEquipments)
        {
            for (int i = 0; i < _equipments.Count; i++)
            {
                ItemContainer tmpItem = EquiSlots[i].transform.GetChild(0).GetComponent<ItemEquipment>().Item;
                if (_equipments[i].Id == tmpItem.Id)
                    if (_equipments[i].Type != Item.ItemType.Tool)
                        continue;
                    else if (_equipments[i].Tool.TimeToUse == tmpItem.Tool.TimeToUse)
                            continue;
                _equipments[i] = tmpItem;
            }
            //Save new Equipments 
            _characterManager.SaveCharacterEquipments(_equipments);
            _updateEquipments = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_inventoryPanel.activeSelf)
                _inventoryPanel.SetActive(false);
            else
                _inventoryPanel.SetActive(true);
        }
    }
    internal bool HaveAvailableSlot()
    {
        for (int i = 0; i < _playerSlots; i++)
        {
            ItemData tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>();
            if (tmpItem.Item.Id != -1)
                continue;
            return true;
        }
        return false;
    }
    public void UpdateEquipments(bool value)
    {
        _updateEquipments = value;
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
        PrintMessage("YEL: 3Not enough energy ");
        return false;
    }

    public void UseItem(ItemContainer item)
    {
        if (item.Id == -1)
            return;
        if (_characterManager.CharacterSetting.Energy > _basicEnergyUse)
            _characterManager.CharacterSettingUseItem(item, _basicEnergyUse, true);
        else
            PrintMessage("YEL: 1Not enough energy ");
    }

    public void UnuseItem(ItemContainer item)
    {
        if (item.Id ==-1)
            return;
        _characterManager.CharacterSettingUnuseItem(item, true);
    }

    public bool AddItemToInventory(int itemId)
    {
        ItemContainer item = BuilItemFromDatabase(itemId);
        for (int i = 0; i < _playerSlots; i++)
        {
            ItemData tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>();
            if (tmpItem.Item.Id != -1)
                continue;
            tmpItem.LoadItem(item);
            UpdateInventory(true);
            return true;
        }
        PrintMessage("RED: Not Enough room in inventory");
        return false;
    }

    public void SetActiveMe()
    {
        if (_inventoryPanel.activeSelf)
            _inventoryPanel.SetActive(false);
        else
            _inventoryPanel.SetActive(true);
    }

    public void GoToRecepieScene()
    {
        BuildTrainStarter();
        //switch the scene
        SceneManager.LoadScene(SceneIdForRecepie);
    }
    public void GoToCreditScene()
    {
        BuildTrainStarter();
        //switch the scene
        SceneManager.LoadScene(SceneIdForCredits);
    }
    public void GoToStoreScene()
    {
        BuildTrainStarter();
        //switch the scene
        SceneManager.LoadScene(SceneIdForStore);
    }

    private void BuildTrainStarter()
    {        
        //Preparing to return to terrain
        GameObject go = new GameObject();
        //Make go undestroyable
        GameObject.DontDestroyOnLoad(go);
        var starter = go.AddComponent<TerrainStarter>();
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        starter.PreviousPosition = player.position;
        starter.ShowInventory = true;
        go.name = "Terrain Starter";
    }

    private void InitMixture(CharacterMixture playerMixture)
    {
        if (playerMixture == null)
            return;
        if (playerMixture.Item == null)
            _itemMixture.LoadEmpty();
        else if (playerMixture.Item.Id == -1)
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

    public ItemContainer BuilItemFromDatabase(int id)
    {
        if (id == -1)
            return new ItemContainer();
        return new ItemContainer(_itemDatabase.FindItem(id));
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

    public bool ElementToolUse(EllementIns element=null)
    {
        ItemEquipment existingEquipment = _inv.EquiSlots[(int)Equipment.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
        ItemContainer itemEquipment = existingEquipment.Item;
        EllementIns.EllementType targetType = element != null ? element.Type: EllementIns.EllementType.Hole;
        if (itemEquipment.Id != -1 && 
            itemEquipment.Type == Item.ItemType.Tool && 
            itemEquipment.StackCnt > 0 &&
            targetType == itemEquipment.Tool.FavouriteEllement)
        {
            existingEquipment.Item.UseItem(1);
            UpdateEquipments(true);
            return true;
        }
        else
        { 
            existingEquipment = _inv.EquiSlots[(int)Equipment.PlaceType.Right].GetComponentInChildren<ItemEquipment>();
            itemEquipment = existingEquipment.Item;
            if (itemEquipment.Id != -1 &&
                itemEquipment.Type == Item.ItemType.Tool &&
                itemEquipment.StackCnt > 0 &&
                targetType == itemEquipment.Tool.FavouriteEllement)
            {
                existingEquipment.Item.UseItem(1);
                UpdateEquipments(true);
                return true;
            }
        }
        PrintMessage("YEL: You don't have a right tool to use");
        return false;
    }

    public void AddExperince(int amount)
    {
        _characterManager.AddCharacterSetting("Experince", amount);
    }
}
