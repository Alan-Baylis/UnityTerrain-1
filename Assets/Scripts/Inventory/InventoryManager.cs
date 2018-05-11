using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public int SlotsX=6;
    public int SlotsY=5;
    public int PlayerSlots = 7;
    public int SlotsPadding = 5;
    public int SlotSize = 50;
    public GUISkin Skin;
    public Vector2 InvLocation = Vector2.zero;

    public List<Item> Slots = new List<Item>();
    public List<Item> Inventory = new List<Item>();
    private ItemDatabase _availableItems;
    private bool _showInventory = false;
    private bool _showTooltip = false;
    private string _tooltip = "";
    private bool _dragging = false;
    private Item _draggedItem;
    private int _draggedIndex;


    // Use this for initialization
    void Start ()
    {
        InvLocation = new Vector2(60, 20);

        for (int i = 0; i < SlotsX * SlotsY; i++)
        {
            Slots.Add(new Item());
            Inventory.Add(new Item());
        }

        _availableItems = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        print(_availableItems.Items.Count);
        //print(Resources.Load<Sprite>("Inventory/34x34Icons_2"););

        for (int i = 0; i < _availableItems.Items.Count; i++)
        {
            AddItemToInventory(i);
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
                //Drag&Drop #3/4: on clear inventory
                Inventory[_draggedIndex] = _draggedItem;
                _dragging = false;

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
            Rect dragBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, SlotSize, SlotSize);
            DrawSprite(dragBox, _draggedItem.Icon);
        }
        if ( Event.current.type == EventType.MouseUp && _dragging)
        {
            //Drag&Drop #4/4: outside of the inv
            Inventory[_draggedIndex] = _draggedItem;
            _dragging = false;
        }
    }

    private void SaveInventory()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            PlayerPrefs.SetInt("Inventory_" + i, Inventory[i].Id);
        }
    }

    private void LoadInventory()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            Inventory[i] = GetItemFromDatabase(PlayerPrefs.GetInt("Inventory_" + i, -1));
        }
    }

    void DrawInventory()
    {
        int invIndex;
        Event currentEvent = Event.current;

        Rect invRect = new Rect(InvLocation.x - SlotsPadding, InvLocation.y - SlotsPadding, (SlotSize+ SlotsPadding) * SlotsX  + SlotsPadding, (SlotSize+ SlotsPadding) * SlotsY +  SlotsPadding);
        GUI.Box(invRect, "", Skin.GetStyle("slotNormal"));

        for (int y = 0; y < SlotsY; y++)
        {
            for (int x = 0; x < SlotsX; x++)
            {

                invIndex = x  + y * SlotsX;
                Rect slotRect = new Rect(x * (SlotSize + SlotsPadding) + InvLocation.x, y * (SlotSize + SlotsPadding) + InvLocation.y, SlotSize, SlotSize);
                if (invIndex>PlayerSlots)
                    GUI.Box(slotRect, "", Skin.GetStyle("slotBroken"));
                else
                    GUI.Box(slotRect, "", Skin.GetStyle("slotDamaged"));
                Slots[invIndex] =  Inventory[invIndex];
                if (Slots[invIndex].Name != null)
                {
                    //Draw the item sprite in the slot 
                    DrawSprite(slotRect, Slots[invIndex].Icon);
                    //Draw mouse hover
                    if (slotRect.Contains(currentEvent.mousePosition))
                    {
                        _showTooltip = true;
                        _tooltip = GenerateTooltip(Slots[invIndex]);
                        if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !_dragging) //Right click and drag
                        {
                            _dragging = true;
                            _draggedItem = Slots[invIndex];
                            _draggedIndex = invIndex;
                            Inventory[invIndex] = new Item();
                        }
                        if (currentEvent.type == EventType.MouseUp && _dragging)
                        {
                            //Drag&Drop #1/4: on filled Item
                            Inventory[_draggedIndex] = Inventory[invIndex];
                            Inventory[invIndex] = _draggedItem;
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
                    //Drag&Drop #2/4: on empty slot
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

    private void UseConsumable(Item item, int invIndex, bool deleteItem)
    {
        if (CheckItemInInventory(item.Id))
        {
            switch (item.Id)
            {
                case 0:
                    //do something;
                    print("Consumable " + item.Name);
                    break;
                case 2:
                    //do something;
                    print("Consumable " + item.Name);
                    break;
                default:
                    print("Not a Consumable ");
                    break;
            }
            if (deleteItem)
                RemoveItemFromInventory(invIndex);
        }
        else
        {
            print("Not in inventory");
        }
    }

    //Draw the item sprite in the Rect 
    void DrawSprite(Rect rect, Sprite sprite)
    {
        //Draw the item sprite in the slot 
        Rect spriteRec = sprite.rect;
        var spriteTexture = sprite.texture;
        spriteRec.xMin /= spriteTexture.width;
        spriteRec.xMax /= spriteTexture.width;
        spriteRec.yMin /= spriteTexture.height;
        spriteRec.yMax /= spriteTexture.height;
        GUI.DrawTextureWithTexCoords(rect, spriteTexture, spriteRec);
    }

    string GenerateTooltip(Item item)
    {
        string tooltip = "";
        tooltip = "<color=white>" + item.Name + "</color>\n\n" + item.Description + "\n<color=yellow>Cost:" + item.Cost + "</color>";
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
                        Inventory[i] = _availableItems.Items[j];
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }

    Item GetItemFromDatabase(int id)
    {
        for (int i = 0; i < _availableItems.Items.Count; i++)
        {
            if (_availableItems.Items[i].Id == id)
                return _availableItems.Items[i];
        }
        return new Item();
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
                var emptyItem = new Item();
                Inventory[i] = emptyItem;
                return true;
            }
        }
        return false;
    }


}
