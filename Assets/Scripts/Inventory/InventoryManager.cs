using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{


    public int SlotsCount = 21;

    public int PlayerSlots = 7;
    public int SlotsPadding = 5;
    public int SlotSize = 50;
    public GUISkin Skin;
    public Vector2 InvLocation = Vector2.zero;



    public List<InventoryItem> Inventory = new List<InventoryItem>();


    private int _slotsX = 0;
    private int _slotsY = 0;
    private ItemDatabase _availableItems;
    private bool _showInventory = false;
    private bool _showTooltip = false;
    private string _tooltip = "";
    private bool _dragging = false;
    private InventoryItem _draggedItem;
    private int _draggedIndex;
    private static GameObject _player; 


    // Use this for initialization
    void Start ()
    {
        //Slot x,y number calculations 
        _slotsX = 5;
        _slotsY = SlotsCount / _slotsX +1;

        //Inv location
        InvLocation = new Vector2(5, 5);

        //_player = GameObject.Find(Player);
        
        for (int i = 0; i < _slotsX * _slotsY; i++)
        {
            Inventory.Add(new InventoryItem());
        }

        _availableItems = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        print(_availableItems.Items.Count);

        for (int i = 0; i < _availableItems.Items.Count; i++)
        {
            if (AddItemToInventory(i))
            {
                Debug.Log("Add item failed");
            }
        }
    }


    void Update()
    {
        //if (Input.GetButtonDown("Inventory"))
        if (Input.GetKeyDown(KeyCode.I))
        {
            _showInventory = !_showInventory;
            if (_dragging)
            {
                PutItemBack();

            }

        }
        
    }

    void OnGUI()
    {
        //for (int i = 0; i < Inventory.Count; i++)
        //{
        //    GUI.Label(new Rect( 10,10*i,200,50),Inventory[i].Name);
        //}

        _showTooltip = false;
        GUI.skin = Skin;

        // You can only call GUI ellements only in the OnGUI
        if (_showInventory)
        {
            if (GUI.Button(new Rect(300, 50, 100, 40), "Save"))
            {
                print("SaveInventory");
                SaveInventory();
            }
            if (GUI.Button(new Rect(300, 100, 100, 40), "Load"))
            {
                print("LoadInventory");
                LoadInventory();
            }

            //print(Slots.Count);
            DrawInventory();
            if (_showTooltip && !_dragging)
            {
                float dynamicHeight = Skin.box.CalcHeight(new GUIContent(_tooltip), 200);
                Rect tooltipBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, dynamicHeight);
                GUI.Box(tooltipBox, _tooltip, Skin.GetStyle("box"));
            }
        }
        if (_dragging)
        {
            //10% bigger than normal
            Rect dragBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, width: SlotSize * 11 / 10, height: SlotSize*11/10); 
            DrawSprite(dragBox, _draggedItem);
        }
        if ( Event.current.type == EventType.MouseUp && _dragging)
        {
            PutItemBack();
        }
    }

    private void SaveInventory()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            PlayerPrefs.SetInt("Inventory_" + i, Inventory[i].Id);
            PlayerPrefs.SetInt("InventoryCnt_" + i, Inventory[i].StackCnt);
        }
    }

    private void LoadInventory()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            Inventory[i] = GetItemFromDatabase(PlayerPrefs.GetInt("Inventory_" + i, -1));
            Inventory[i].StackCnt = PlayerPrefs.GetInt("InventoryCnt_" + i, 1);
        }
    }

    void DrawInventory()
    {
        Event currentEvent = Event.current;

        Rect invRect = new Rect(InvLocation.x - SlotsPadding, InvLocation.y - SlotsPadding, (SlotSize+ SlotsPadding) * _slotsX  + SlotsPadding, (SlotSize+ SlotsPadding) * _slotsY +  SlotsPadding);
        GUI.Box(invRect, "", Skin.GetStyle("slotNormal"));

        for (int y = 0; y < _slotsY; y++)
        {
            for (int x = 0; x < _slotsX; x++)
            {
                int invIndex = x  + y * _slotsX;
                if (invIndex > SlotsCount)
                    break;
                Rect slotRect = new Rect(x * (SlotSize + SlotsPadding) + InvLocation.x, y * (SlotSize + SlotsPadding) + InvLocation.y, SlotSize, SlotSize);
                if (invIndex>PlayerSlots)
                    GUI.Box(slotRect, "", Skin.GetStyle("slotBroken"));
                else
                    GUI.Box(slotRect, "", Skin.GetStyle("slotDamaged"));
                if (Inventory[invIndex].Name != null)
                {
                    //Draw the item sprite in the slot 
                    DrawSprite(slotRect, Inventory[invIndex]);
                    //Draw mouse hover
                    if (slotRect.Contains(currentEvent.mousePosition))
                    {
                        _showTooltip = true;
                        _tooltip = GenerateTooltip(Inventory[invIndex]);
                        if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !_dragging) //Right click and drag
                        {
                            _dragging = true;
                            _draggedItem = Inventory[invIndex];
                            _draggedIndex = invIndex;
                            Inventory[invIndex] = new InventoryItem();
                        }
                        if (currentEvent.type == EventType.MouseUp && _dragging)
                        {
                            //Drag&Drop #1/3: on filled Item
                            //Same items Stack them together 
                            if (Inventory[_draggedIndex].Id == Inventory[invIndex].Id)
                            {
                                //Todo: if higher than MaxStackCnt don't let them do it 
                                Inventory[invIndex].StackCnt += _draggedItem.StackCnt;
                                Inventory[_draggedIndex] = new InventoryItem();
                            }
                            //not Same items swap them
                            else
                            {
                                if (MixAbleItems())
                                {
                                    print("MixAbleItems");
                                }
                                else
                                {
                                    Inventory[_draggedIndex] = Inventory[invIndex];
                                    Inventory[invIndex] = _draggedItem;
                                }
                            }
                            _dragging = false;
                        }

                        //Right clicked 
                        if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
                        {
                            if (Inventory[invIndex].Type == Item.ItemType.Consumable)
                            {
                                //Use Consumable
                                UseConsumable(Inventory[invIndex], invIndex,true);
                            }
                        }
                    }
                }
                else if (slotRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseUp && _dragging)
                {
                    //Drag&Drop #2/3: on empty slot
                    if (invIndex > PlayerSlots)
                        Inventory[_draggedIndex] = _draggedItem;
                    else
                    {
                        Inventory[_draggedIndex] = Inventory[invIndex];
                        Inventory[invIndex] = _draggedItem;
                    }
                    _dragging = false;
                }
            }
        }
    }

    private bool MixAbleItems()
    {
        return false;
    }

    private void PutItemBack()
    {
        //Drag&Drop #3/3: on clear inventory
        Inventory[_draggedIndex] = _draggedItem;
        _dragging = false;
    }

    private void UseConsumable(InventoryItem item, int invIndex, bool deleteItem)
    {
        if (CheckItemInInventory(item.Id))
        {
            switch (item.Id)
            {
                case 0:
                    //do something;
                    print("Consumable " + invIndex +" " + item.Name);
                    break;
                case 2:
                    //do something;
                    print("Consumable " + invIndex + " " + item.Name);
                    break;
                default:
                    print(invIndex + " Not a Consumable ");
                    break;
            }
            if (deleteItem)
                if (!RemoveItemFromInventory(item.Id))
                    Debug.Log("Remove Item From Inventory Failed");
        }
        else
        {
            print("Not in inventory");
        }
    }

    //Draw the item sprite in the Rect 
    void DrawSprite(Rect rect, InventoryItem item)
    {
        //Draw the item sprite in the slot 
        Sprite sprite = item.Icon;
        Rect spriteRec = sprite.rect;
        var spriteTexture = sprite.texture;
        spriteRec.xMin /= spriteTexture.width;
        spriteRec.xMax /= spriteTexture.width;
        spriteRec.yMin /= spriteTexture.height;
        spriteRec.yMax /= spriteTexture.height;
        GUI.DrawTextureWithTexCoords(rect, spriteTexture, spriteRec);
        if (item.StackCnt >1)
            GUI.Label(rect, "<color=black> " + item.StackCnt + "</color>");
    }

    string GenerateTooltip(InventoryItem item)
    {
        string color;
        switch (item.Type)
        {
            case Item.ItemType.Weapon:
                color = "Blue";
                break;
            case Item.ItemType.Consumable:
                color = "Blue";
                break;
            case Item.ItemType.Useable:
                color = "Blue";
                break;
            default:
                color = "white";
                break;
        }
        var tooltip = "<color="+ color +">" + item.Name + "</color>\n\n" + item.Description + "\n<color=green>Available:" + item.StackCnt + "</color>" + "\n<color=yellow>Cost:" + item.Cost + "</color>";
        return tooltip;
    }

    bool AddItemToInventory(int id)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].Name == null) //empty Slot
            {
                for (int j = 0; j < _availableItems.Items.Count; j++)
                {
                    if (_availableItems.Items[j].Id == id)
                    {
                        Inventory[i] = ConvertItemToInventory(_availableItems.Items[j]);
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }

    InventoryItem GetItemFromDatabase(int id)
    {
        for (int i = 0; i < _availableItems.Items.Count; i++)
        {
            if (_availableItems.Items[i].Id == id)
                return ConvertItemToInventory(_availableItems.Items[i]);
        }
        return new InventoryItem();
    }

    private InventoryItem ConvertItemToInventory(Item item)
    {
        InventoryItem inventoryItem = new InventoryItem( item.Id, item.Name, item.Description, item.Cost, item.Type);
        return inventoryItem;
    }

    bool CheckItemInInventory(int id)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].Id == id) 
                return true;
        }
        return false;
    }

    bool RemoveItemFromInventory(int id)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].Id == id)
            {
                if (Inventory[i].StackCnt > 1)
                    Inventory[i].StackCnt--;
                else
                    Inventory[i] = new InventoryItem();
                return true;
            }
        }
        return false;
    }

}
